using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirHeater.ControlSystem.PlantCom
{
    public interface IAirHeaterCom
    {
        double GetFilteredTemperature();
        double ReadTemperature();
        void SetGain(double gain);
    }
}
