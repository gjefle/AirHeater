using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AirHeater.Common.Models;
using AirHeater.AlarmMonitor.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AirHeater.AlarmMonitor.Controllers
{
    [Route("api/[controller]")]
    public class TemperatureController : Controller
    {
        private PlantContext _ctx;

        public TemperatureController(PlantContext ctx)
        {
             _ctx = ctx;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<Temperature> Get()
        {
            var today = DateTime.Today;
            return _ctx.Temperature.AsNoTracking()
                .Where(t => t.LogDate.DateTime >= today)
                .ToList();
        }
        [HttpGet]
        [Route("{UpdateTemperature}/{date}")]
        public IEnumerable<Temperature> UpdateTemperature([FromRoute]DateTime date)
        {
            var dtOffset = new DateTimeOffset(date);
            return _ctx.Temperature
                .AsNoTracking()
                .Where(t => t.LogDate > dtOffset)
                .ToList();
        }
    }
}
