using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirHeater.Common.Models;
using AirHeater.Common.ModelViews;
using Microsoft.EntityFrameworkCore;


namespace AirHeater.AlarmMonitor.Data
{
    public class PlantContext: DbContext
    {
        public PlantContext(DbContextOptions<PlantContext> options) : base(options) { }
        public DbSet<Temperature> Temperature { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<AlarmType> AlarmType { get; set; }
        public DbSet<TemperatureAlarm> TemperatureAlarm { get; set; }

        public DbSet<TemperatureAlarmView> TemperatureAlarmView { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TemperatureAlarmView>()
                .ToTable("TemperatureAlarmView").HasKey(ta => ta.TemperatureAlarmId);
        }
    }
}
