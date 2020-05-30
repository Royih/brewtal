using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Brewtal2.Pid.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;

namespace Brewtal2.Pid
{
    // This is mainly code converted to c# from python library: https://github.com/steve71/MAX31865
    // (MIT) Copyright (c) 2015 Stephen P. Smith
    // Uses Unosquare library to manipulate the IO pins. 
    // Uses SPI to read temperature from two MAX31865 chips and return the calculated temparatures. 
    public class TempReader : ITempReader
    {
        private readonly ILogger<TempReader> _logger;

        private readonly IGpioPin CsPin1;
        private readonly IGpioPin CsPin2;
        private readonly IGpioPin MisoPin;
        private readonly IGpioPin MosiPin;
        private readonly IGpioPin ClkPin;
        private readonly IGpioPin TempReadHeartbeat;

        public TempReader(ILogger<TempReader> logger)
        {
            _logger = logger;
            CsPin1 = Pi.Gpio[7];
            CsPin2 = Pi.Gpio[8];
            MisoPin = Pi.Gpio[9];
            MosiPin = Pi.Gpio[10];
            ClkPin = Pi.Gpio[11];
            TempReadHeartbeat = Pi.Gpio[5];

            CsPin1.PinMode = GpioPinDriveMode.Output;
            CsPin2.PinMode = GpioPinDriveMode.Output;
            MisoPin.PinMode = GpioPinDriveMode.Input;
            MosiPin.PinMode = GpioPinDriveMode.Output;
            ClkPin.PinMode = GpioPinDriveMode.Output;
            TempReadHeartbeat.PinMode = GpioPinDriveMode.Output;

            CsPin1.Write(true);
            CsPin2.Write(true);
        }

        public TempReaderResultDto ReadTemp()
        {
            return new TempReaderResultDto
            {
                Temp1 = ReadTemp(0),
                    Temp2 = ReadTemp(1)
            };
        }

        private double ReadTemp(int pidId)
        {
            var csPin = pidId == 0 ? CsPin1 : CsPin2;
            //
            // b10000000 = 0x80
            // 0x8x to specify 'write register value'
            // 0xx0 to specify 'configuration register'
            //
            // 0b10110010 = 0xB2
            // Config Register
            // ---------------
            // bit 7: Vbias -> 1 (ON)
            // bit 6: Conversion Mode -> 0 (MANUAL)
            // bit5: 1-shot ->1 (ON)
            // bit4: 3-wire select -> 1 (3 wire config)
            // bits 3-2: fault detection cycle -> 0 (none)
            // bit 1: fault status clear -> 1 (clear any fault)
            // bit 0: 50/60 Hz filter select -> 0 (60Hz)
            //
            // 0b11010010 or 0xD2 for continuous auto conversion 
            // at 60Hz (faster conversion)
            //

            //one shot
            WriteRegister(0x00, 0xB2, csPin);

            // conversion time is less than 100ms
            Thread.Sleep(100);

            // read all registers
            var myOut = ReadRegisters(0, 8, csPin);
            string[] b = myOut.Select(x => Convert.ToString(x, 2).PadLeft(8, '0')).ToArray();

            var conf_reg = myOut[0];
            //_logger.LogInformation("config register byte: %x%", conf_reg);

            var rtd_msb = myOut[1];
            var rtd_lsb = myOut[2];

            var rtd_ADC_Code = ((rtd_msb << 8) | rtd_lsb) >> 1;

            var temp_C = CalcPT100Temp(rtd_ADC_Code);

            var hft_msb = myOut[3];
            var hft_lsb = myOut[4];

            var hft = ((hft_msb << 8) | hft_lsb) >> 1;
            //_logger.LogInformation("high fault threshold: %d", hft);

            var lft_msb = myOut[5];
            var lft_lsb = myOut[6];
            var lft = ((lft_msb << 8) | lft_lsb) >> 1;
            //_logger.LogInformation("low fault threshold:{0}", lft);

            var status = myOut[7];

            //
            // 10 Mohm resistor is on breakout board to help
            // detect cable faults
            // bit 7: RTD High Threshold / cable fault open 
            // bit 6: RTD Low Threshold / cable fault short
            // bit 5: REFIN- > 0.85 x VBias -> must be requested
            // bit 4: REFIN- < 0.85 x VBias (FORCE- open) -> must be requested
            // bit 3: RTDIN- < 0.85 x VBias (FORCE- open) -> must be requested
            // bit 2: Overvoltage / undervoltage fault
            // bits 1,0 don't care	
            //print "Status byte: %x" % status

            if ((status & 0x80) == 1)
            {
                throw new Exception("High threshold limit (Cable fault/open)");
            }

            if ((status & 0x40) == 1)
            {
                throw new Exception("Low threshold limit (Cable fault/short)");
            }
            if ((status & 0x04) == 1)
            {
                throw new Exception("Overvoltage or Undervoltage Error");
            }
            _logger.LogInformation($"Temp: {temp_C}°C");
            TempReadHeartbeat.Write(!TempReadHeartbeat.Read());
            return temp_C;
        }

        private double CalcPT100Temp(int rtd_ADC_Code)
        {
            const double R_REF = 430.0; // Reference Resistor
            const double Res0 = 100.0; // Resistance at 0 degC for 430ohm R_Ref
            var a = .00390830;
            var b = -.000000577500;
            //_logger.LogInformation("RTD ADC Code: %d", rtd_ADC_Code);

            var res_RTD = (rtd_ADC_Code * R_REF) / 32768.0; // PT100 Resistance
            //_logger.LogInformation("PT100 Resistance: %f ohms", res_RTD);

            var temp_C = -(a * Res0) + Math.Sqrt(a * a * Res0 * Res0 - 4 * (b * Res0) * (Res0 - res_RTD));
            temp_C = temp_C / (2 * (b * Res0));
            var temp_C_line = (rtd_ADC_Code / 32.0) - 256.0;

            //_logger.LogInformation("Straight Line Approx. Temp: %f degC", temp_C_line);
            //_logger.LogInformation("Callendar-Van Dusen Temp (degC > 0): %f degC", temp_C);

            if (temp_C < 0) //use straight line approximation if less than 0
            {
                temp_C = (rtd_ADC_Code / 32) - 256;
            }
            return temp_C;
        }

        private void WriteRegister(byte regNum, byte dataByte, IGpioPin csPin)
        {
            csPin.Write(false);

            // 0x8x to specify 'write register value'
            var addressByte = (0x80 | regNum); //todo: Verify this.

            // first byte is address byte
            SendByte((byte)addressByte);

            // the rest are data bytes
            SendByte(dataByte);

            csPin.Write(true);
        }

        private byte[] ReadRegisters(byte regNumStart, int numRegisters, IGpioPin csPin)
        {
            var returnByte = new List<byte>();

            csPin.Write(false);

            SendByte(regNumStart);

            for (var i = 0; i < numRegisters; i++)
            {
                var data = RecvByte();
                returnByte.Add(data);
            }

            csPin.Write(true);
            return returnByte.ToArray();
        }

        private void SendByte(byte @byte)
        {
            for (var i = 0; i < 8; i++)
            {
                ClkPin.Write(true);

                if ((@byte & 0x80) > 0)
                {
                    MosiPin.Write(true);
                }
                else
                {
                    MosiPin.Write(false);
                }

                @byte = unchecked((Byte)(@byte << 1));

                ClkPin.Write(false);
            }
        }

        private byte RecvByte()
        {
            byte @byte = 0x00;

            for (var i = 0; i < 8; i++)
            {
                ClkPin.Write(true);
                @byte = unchecked((Byte)(@byte << 1));

                if (MisoPin.Read())
                {
                    @byte = (byte)(@byte | 0x1);
                }
                ClkPin.Write(false);
            }
            return @byte;
        }

    }

}