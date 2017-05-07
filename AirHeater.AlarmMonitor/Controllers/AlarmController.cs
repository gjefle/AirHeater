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

        // GET api/values
        [HttpGet, Route("TemperatureAlarms")]
        public IEnumerable<TemperatureAlarmView> TemperatureAlarms()
        {
            return  _ctx.TemperatureAlarmView.FromSql("select * from dbo.TemperatureAlarmView").ToList();
        }

        [HttpGet, Route("EnabledTemperatureAlarms")]
        public IEnumerable<TemperatureAlarmView> EnabledTemperatureAlarms()
        {
            return _ctx.TemperatureAlarmView.FromSql("select * from dbo.TemperatureAlarmView")
                .Where(taw => taw.Active || !taw.Acknowledged)
                .ToList();
        }

        [HttpGet, Route("{AcknowledgeAlarm}/{id}")]
        public IEnumerable<TemperatureAlarmView> AcknowledgeAlarm([FromRoute]int id)
        {
            var alarm = _ctx.TemperatureAlarm
                .FirstOrDefault(ta => ta.TemperatureAlarmId == id);
            if (alarm != null)
            {
                alarm.Acknowledged = true;
                alarm.AcknowledgeDate = DateTimeOffset.Now;
                _ctx.SaveChanges();
                return _ctx.TemperatureAlarmView.FromSql("select * from dbo.TemperatureAlarmView").ToList();
            }
            return null;
        }
    }
}
