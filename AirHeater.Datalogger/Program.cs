using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AirHeater.Datalogger.OPC;
using NationalInstruments.Net;

namespace AirHeater.Datalogger
{
    class Program : IDisposable
    {
        private static OpcReader opcReader;
        private static CancellationTokenSource token;
        private static System.Threading.Tasks.Task task;
        private static string connectionString;
        static void Main(string[] args)
        {
            connectionString = ConfigurationManager.ConnectionStrings["PlantContext"].ConnectionString;
            opcReader = new OpcReader(new DataSocket());
            //var temperature = GetTemperature();
            //Console.WriteLine(temperature);
            StartLogger();
            Console.ReadLine();
        }
        private static void StartLogger()
        {
            token = new CancellationTokenSource();

            task = System.Threading.Tasks.Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    var waitTask = Task.Delay(1000, token.Token);
                    LogTemperatureData();
                    LogGainData();
                    await waitTask;
                }
            });
        }

        public static void LogTemperatureData()
        {
            var temperature = GetTemperature();
            if(temperature == null) return;
            try
            {
                //var connectionString = ConfigurationManager.ConnectionStrings["PlantContext"].ConnectionString;
                using (SqlConnection dbCon= new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("AddTemperature", dbCon);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Value", Math.Round((double)temperature, 2)));
                    cmd.Parameters.Add(new SqlParameter("@TagName", "Temperature"));
                    dbCon.Open();
                    cmd.ExecuteNonQuery();
                    dbCon.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public static void LogGainData()
        {
            var gain = GetGain();
            if (gain == null) return;
            try
            {
                //var connectionString = ConfigurationManager.ConnectionStrings["PlantContext"].ConnectionString;
                using (SqlConnection dbCon = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("AddGain", dbCon);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Value", Math.Round((double)gain, 2)));
                    cmd.Parameters.Add(new SqlParameter("@TagName", "Gain"));
                    dbCon.Open();
                    cmd.ExecuteNonQuery();
                    dbCon.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        void StopLogger()
        {
            token.Cancel();
            try
            {
                task.Wait();
            }
            catch (AggregateException)
            {
            }
        }

        private static double? GetGain()
        {
            return opcReader?.ReadFloatingTag("Gain") ?? null;
        }
        private static double? GetTemperature()
        {
            return opcReader?.ReadFloatingTag("Temperature") ?? null;
        }
        public void Dispose()
        {
            StopLogger();
        }
    }
}
