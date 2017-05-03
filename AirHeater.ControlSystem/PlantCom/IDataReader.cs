using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirHeater.ControlSystem.PlantCom
{
    public interface IDataReader
    {
        double GetTemperature();
        void SetTemperature(double temperature);
    }
}
