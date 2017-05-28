using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirHeater.AlarmMonitor.Data;
using AirHeater.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace AirHeater.AlarmMonitor.Controllers
{
    [Route("api/[controller]")]
    public class AlarmtypeController : Controller
    {
        private PlantContext _ctx;

        public AlarmtypeController(PlantContext ctx)
        {
            _ctx = ctx;
        }
        // GET api/alarmType
        [HttpGet, Route("All")]
        public IEnumerable<AlarmType> All()
        {
            return _ctx.AlarmType.ToList();
        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] AlarmType alarmType)
        {
            if (alarmType == null || alarmType.AlarmTypeId != id)
            {
                return BadRequest();
            }
            var dbAlarmType = _ctx.AlarmType.Find(id);
            if (dbAlarmType == null)
            {
                return NotFound();
            }
            dbAlarmType.Shelved = alarmType.Shelved;
            _ctx.SaveChanges();
            return new NoContentResult();
        }
    }
}
