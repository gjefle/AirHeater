using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirHeater.ControlSystem.Filtering;
using AirHeater.ControlSystem.Simulation;

namespace AirHeater.ControlSystem.PlantCom
{
    public class SimulatedHeaterReader : IAirHeaterCom
    {
        private readonly AirHeaterSimulation _airHeater;
        private IFilter _filter;

        public SimulatedHeaterReader(IFilter filter, AirHeaterSimulation airHeater)
        {
            _airHeater = airHeater;
            _filter = filter;
        }
        public double GetFilteredTemperature()
        {
            var temperature =  _airHeater.Tout;
            return _filter.FilterNewValue(temperature);
        }

        public double ReadTemperature()
        {
            var temperature = NoiseCreator(_airHeater.Tout);
            return temperature;
        }
        private double NoiseCreator(double val)
        {
            var rnd = new Random();
            var randomNumber = rnd.Next(-3, 3) / 10.0;
            return val + randomNumber;
        }

        public void SetGain(double gain)
        {
            _airHeater.u = gain;
        }
    }
}
