using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirHeater.Common.ModelViews
{
    public class AlarmView
    {
        public int AlarmId { get; set; }
        public DateTimeOffset ActivationDate { get; set; }
        public bool Acknowledged { get; set; }
        public DateTimeOffset? AcknowledgeDate { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public bool Shelved { get; set; }
        public string  TypeName { get; set; }
    }
}
