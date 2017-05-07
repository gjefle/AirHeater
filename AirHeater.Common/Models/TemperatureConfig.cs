using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirHeater.Common.Models
{
    public class TemperatureConfig
    {
        public int TemperatureConfigId { get; set; }
        public double HighAlarmTemperature { get; set; }
        public double LowAlarmTemperature { get; set; }
    }
}
