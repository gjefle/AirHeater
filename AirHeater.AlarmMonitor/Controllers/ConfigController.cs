using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirHeater.AlarmMonitor.Data;
using AirHeater.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AirHeater.AlarmMonitor.Controllers
{
    [Route("api/[controller]")]
    public class ConfigController : Controller
    {
        private PlantContext _ctx;

        public ConfigController(PlantContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]
        public TemperatureConfig TemperatureConfig()
        {
            return _ctx.TemperatureConfig
                .AsNoTracking()
                .FirstOrDefault();
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] TemperatureConfig config)
        {
            if (config == null || config.TemperatureConfigId != id)
            {
                return BadRequest();
            }
            var temperatureConfig = _ctx.TemperatureConfig.Find(id);
            if (temperatureConfig == null)
            {
                return NotFound();
            }
            temperatureConfig.HighAlarmTemperature = config.HighAlarmTemperature;
            temperatureConfig.LowAlarmTemperature = config.LowAlarmTemperature;
            var saves = _ctx.SaveChanges();
            return new NoContentResult();
        }
    }
}
