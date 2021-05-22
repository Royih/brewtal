

namespace Brewtal2.Pid
{

    // Source: http://playground.arduino.cc/Main/BarebonesPIDForEspresso#pid
    // Credits and lots of gratitude to Tim Hirzel for sharing his code (December 2007). 
    // Tim Hirzels Arduino code was ported to .Net Microframework by Roy Ingar Hansen (July 2014)
    // Then ported to .net core to run on Linux on i.e. a Raspberry PI by Roy Ingar Hansen (May 2018)
    // PID tuning help: http://en.wikipedia.org/wiki/PID_controller
    public class PIDRegulatorFridge : IPIDRegulator
    {
        public double ErrorSum { get; private set; }


        public PIDRegulatorFridge()
        {

        }

        public void Reset()
        {
            ErrorSum = 0;
        }

        private bool _cooling;

        public double Compute(double actualTemp, double preferredTemp)
        {
            if (_cooling && (actualTemp + 0.5) > preferredTemp)
            {
                return 100;
            }
            else if (!_cooling && (actualTemp - 0.5) > preferredTemp)
            {
                _cooling = true;
                return 100;
            }
            _cooling = false;
            return 0;
        }

    }
}
