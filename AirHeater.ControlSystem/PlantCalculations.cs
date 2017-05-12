using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirHeater.ControlSystem
{
    public static class PlantCalculations
    {
        public static double v0 = 1;
        public static double v1 = 5;

        public static double t0 = 20;
        public static double t1 = 50;
        public static double CalcTemperature(double v)
        {
            var temperature = t0 + (v - v0) * (t1 - t0) / (v1 - v0);
            return temperature;
        }

        public static double CalcTemperatureToVolt(double t)
        {
            var volt = v0 + (t - t0) * (v1 - v0) / (t1 - t0);
            return Math.Min(Math.Max(volt, 1), 5);
        }
        //public static double CalcGain(double temperature)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
