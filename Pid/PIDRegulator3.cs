

namespace Brewtal2.Pid
{

    // Source: http://playground.arduino.cc/Main/BarebonesPIDForEspresso#pid
    // Credits and lots of gratitude to Tim Hirzel for sharing his code (December 2007). 
    // Tim Hirzels Arduino code was ported to .Net Microframework by Roy Ingar Hansen (July 2014)
    // Then ported to .net core to run on Linux on i.e. a Raspberry PI by Roy Ingar Hansen (May 2018)
    // PID tuning help: http://en.wikipedia.org/wiki/PID_controller
    public class PIDRegulator3 : IPIDRegulator
    {
        private const double WindupGuardGain = 1.0;

        //Gains
        private readonly double _kp;
        private readonly double _ki;
        private readonly double _kd;

        private double _pTerm;
        private double _iTerm;
        private double _dTerm;
        public double ErrorSum { get; private set; }
        private double _lastTemp;


        public PIDRegulator3(double kp, double ki, double kd)
        {
            _kp = kp;
            _ki = ki;
            _kd = kd;
        }

        public void Reset()
        {
            ErrorSum = 0;
        }

        public double Compute(double pv, double sp)
        {

            // determine how badly we are doing
            double error = sp - pv;

            // the pTerm is the view from now, the pgain judges 
            // how much we care about error we are this instant.
            _pTerm = _kp * error;

            // iState keeps changing over time; it's 
            // overall "performance" over time, or accumulated error
            ErrorSum += error;

            // to prevent the iTerm getting huge despite lots of 
            //  error, we use a "windup guard" 
            // (this happens when the machine is first turned on and
            // it cant help be cold despite its best efforts)

            // not necessary, but this makes windup guard values 
            // relative to the current iGain
            double windupGaurd = WindupGuardGain / _ki;

            if (ErrorSum > windupGaurd)
                ErrorSum = windupGaurd;
            else if (ErrorSum < -windupGaurd)
                ErrorSum = -windupGaurd;
            _iTerm = _ki * ErrorSum;

            // the dTerm, the difference between the temperature now
            //  and our last reading, indicated the "speed," 
            // how quickly the temp is changing. (aka. Differential)
            _dTerm = (_kd * (pv - _lastTemp));

            // now that we've use lastTemp, put the current temp in
            // our pocket until for the next round
            _lastTemp = pv;

            // the magic feedback bit
            var outReal = _pTerm + _iTerm - _dTerm;
            if (outReal > 100)
                outReal = 100;
            if (outReal < 0)
                outReal = 0;

            //Write it out to the world            
            return outReal;
        }

    }
}
