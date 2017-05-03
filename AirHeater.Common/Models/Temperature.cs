using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirHeater.Common.Models
{
    public class Temperature
    {
        public int TemperatureId { get; set; }
        public int TagId { get; set; }

        public Tag Logger { get; set; }
        public DateTimeOffset LogDate { get; set; }
        public double Value { get; set; }
    }
}
