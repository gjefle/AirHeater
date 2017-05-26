using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirHeater.Common.Models
{
    public class Gain
    {
        public int GainId { get; set; }
        public int TagId { get; set; }

        public DateTimeOffset LogDate { get; set; }
        public double Value { get; set; }
    }
}
