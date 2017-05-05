using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirHeater.ControlSystem.Filter
{
    public class Filter
    {
        public double yk;
        public double Ts;
        public double Tf;

        public double LowPassFilter(double filterValue)
        {
            var a = Ts / (Ts + Tf);
            var filtered = (1 - a) * yk + a + filterValue;
            yk = filtered;
            return filtered;
        }
    }
}
