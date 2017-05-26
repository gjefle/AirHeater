using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AirHeater.ControlSystem.PlantCom;

namespace AirHeater.ControlSystem.PID
{
    public interface IPidCom
    {
        double GetCurrentGain();

    }
    public class PidController : IDisposable, IPidCom
    {
        public double SetPoint { get; set; } = 25;
        private double _u;

        private const double MinGain = 0;
        private const double MaxGain = 5;
        private double Tout;
        private double Kp = 0.515; // T/K(Tc+Td);
        private double Ti = 18.3; // min(T, c(Tc +Td)
        private const double Ts = 0.1; // seconds
        private const int WaitTime = 100; // Ts in milliseconds
        private double ek;
        private IAirHeaterCom _plantReader;
        private CancellationTokenSource pidToken;
        private System.Threading.Tasks.Task pidTask;

        public PidController(IAirHeaterCom plantReader)
        {
            _plantReader = plantReader;
            StartPid();
        }

        public double GetCurrentGain()
        {
            return _u;
        }
        private double CalculateGain()
        {
            var e_change = SetPoint - Tout;
            var u = Kp * e_change + (Kp / Ti) * ek;
            ek = ek + Ts * e_change;
            return Math.Max(Math.Min(u, MaxGain), MinGain); // Make sure gain is within 0-5V range
        }

        private void UpdatePlant(double setPoint)
        {
            Tout = _plantReader.GetFilteredTemperature();
            _u = CalculateGain();
            _plantReader.SetGain(_u);
        }

        private void StartPid()
        {
            pidToken = new CancellationTokenSource();

            pidTask = Task.Run(async () =>
            {
                while (!pidToken.IsCancellationRequested)
                {
                    var delayTask = Task.Delay(WaitTime, pidToken.Token);
                    UpdatePlant(SetPoint);
                    await delayTask;
                }
            });
        }
        private void StopPid()
        {
            pidToken.Cancel();
            try
            {
                pidTask.Wait();
            }
            catch (AggregateException) { }
        }

        public void Dispose()
        {
            StopPid();
        }
    }
}
