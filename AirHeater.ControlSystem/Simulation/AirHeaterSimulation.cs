using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirHeater.ControlSystem.Simulation
{
    public class AirHeaterSimulation
    {
        public AirHeaterSimulation(double roomTemperature)
        {
            Tenv = roomTemperature;
            Tout = roomTemperature;
        }
        private const double Td = 2; //Time delay (ms).
        private const double Tc = 22; // Time constant (ms).
        private readonly double Tenv; // Room/environmental temperature (Celsius).
        public double Tout; // Temperature at tube outlet.

        /// <summary>
        /// Get temperature based on gain(C/V) and constant values
        /// </summary>
        /// <param name="Kh"></param>
        /// <param name="u"></param>
        /// <returns></returns>
        public double NewTemperature(double Kh, double u, double t)
        {
            return (-Tout + (Kh * u * (t - Td) + Tenv)) / Tc;
        }
    }
}
