using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirHeater.AlarmMonitor.Data;
using AirHeater.Common.Models;
using AirHeater.Common.ModelViews;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AirHeater.AlarmMonitor.Controllers
{
    [Route("api/[controller]")]
    public class AlarmController : Controller
    {
        private PlantContext _ctx;

        public AlarmController(PlantContext ctx)
        {
            _ctx = ctx;
        }

        // GET api/alarm
        [HttpGet, Route("All")]
        public IEnumerable<AlarmView> All()
        {
            return  _ctx.AlarmView.FromSql("select * from dbo.AlarmView")
                .Where(av => !av.Shelved)
                .OrderBy(av => !av.Active)
                .ThenBy(av => av.Acknowledged)
                .ToList();
        }

        [HttpGet, Route("EnabledAlarms")]
        public IEnumerable<AlarmView> EnabledAlarms()
        {
            return _ctx.AlarmView.FromSql("select * from dbo.AlarmView")
                .Where(av => !av.Shelved && (!av.Acknowledged || av.Active))
                .OrderBy(av => !av.Active)
                .ThenBy(av => av.Acknowledged)
                .ToList();
        }

        [HttpGet, Route("AcknowledgeAlarm/{id}")]
        public Alarm AcknowledgeAlarm([FromRoute]int id)
        {
            var alarm = _ctx.Alarm
                .FirstOrDefault(ta => ta.AlarmId == id);
            if (alarm != null)
            {
                alarm.Acknowledged = true;
                alarm.AcknowledgeDate = DateTimeOffset.Now;
                _ctx.SaveChanges();
                return alarm;
            }
            return null;
        }
    }
}
