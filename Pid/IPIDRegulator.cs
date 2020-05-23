namespace Brewtal2.Pid
{
    public interface IPIDRegulator
    {
        double ErrorSum { get; }
        void Reset();
        double Compute(double processVal, double setPoint);
    }
}