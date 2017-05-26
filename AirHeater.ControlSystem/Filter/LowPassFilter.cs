using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirHeater.ControlSystem.Filter
{
    public interface IFilter
    {
        double FilterNewValue(double filterValue);
    }
    public class LowPassFilter : IFilter
    {
        public LowPassFilter(double startValue)
        {
            yk = startValue;
        }
        public double yk;
        public double Ts = 0.1;
        public double Tf = 0.5; // Tf>=Ts*5 -> Tf>=0.1*5

        public double FilterNewValue(double filterValue)
        {
            var a = Ts / (Ts + Tf);
            var filtered = (1 - a) * yk + a*filterValue;
            yk = filtered;
            return filtered;
        }
    }
}