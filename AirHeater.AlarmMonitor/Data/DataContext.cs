using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirHeater.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace AirHeater.AlarmMonitor.Data
{
    public class PlantContext: DbContext
    {
        public PlantContext(DbContextOptions<PlantContext> options) : base(options) { }
        public DbSet<Temperature> Temperature { get; set; }
        public DbSet<Tag> Logger { get; set; }
        public DbSet<AlarmType> AlarmType { get; set; }
        public DbSet<Alarm> Alarm { get; set; }
        
    }
}
