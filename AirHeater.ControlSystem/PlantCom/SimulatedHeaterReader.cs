using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirHeater.ControlSystem.PlantCom
{
    public class SimulatedHeaterReader : IDataReader
    {
        public double GetTemperature()
        {
            throw new NotImplementedException();
        }

        public void SetTemperature(double temperature)
        {
            throw new NotImplementedException();
        }
    }
}
