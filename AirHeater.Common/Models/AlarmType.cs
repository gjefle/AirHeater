using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirHeater.Common.Models
{
    public class AlarmType
    {
        public int AlarmTypeId { get; set; }
        public int TypeEnum { get; set; }
        public string TypeName { get; set; }
        public bool Shelved { get; set; }
    }
}
