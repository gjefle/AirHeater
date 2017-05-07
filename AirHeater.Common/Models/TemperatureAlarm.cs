using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirHeater.Common.Models
{
    public class TemperatureAlarm
    {
        public int TemperatureAlarmId { get; set; }
        public int AlarmTypeId { get; set; }
        public AlarmType AlarmType { get; set; }
        public DateTimeOffset ActivationDate { get; set; }
        public bool Acknowledged { get; set; }
        public DateTimeOffset? AcknowledgeDate { get; set; }
        public bool Active { get; set; }
        public string Description { get; set; }
    }
}
