using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirHeater.ControlSystem.Simulation;

namespace AirHeater.ControlSystem.PlantCom
{
    public class SimulatedHeaterReader : IDataReader
    {
        private readonly AirHeaterSimulation _airHeater;

        public SimulatedHeaterReader(AirHeaterSimulation airHeater)
        {
            _airHeater = airHeater;
        }
        public double GetTemperature()
        {
            return _airHeater.Tout;
        }

        public void SetGain(double gain)
        {
            _airHeater.u = gain;
        }
    }
}
