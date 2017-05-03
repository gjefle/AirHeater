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
            return _ctx.Temperature.AsNoTracking().ToList();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
