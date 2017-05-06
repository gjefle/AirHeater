using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AirHeater.ControlSystem.PlantCom;

namespace AirHeater.ControlSystem.PID
{
    public interface IPidController
    {
        double CurrentGain { get; set; }
        double SetPoint { get; set; }
    }
    public class PidController : IDisposable, IPidController
    {
        public double SetPoint { get; set; } = 25;
        public double CurrentGain { get; set; }

        private const double MinGain = 0;
        private const double MaxGain = 5;
        private double Tout;
        private double Kp = 0.515; // T/K(Tc+Td);
        private double Ti = 18.3; // min(T, c(Tc +Td)
        private int Ts = 100; // ms
        private double z;
        private IAirHeaterCom _plantReader;
        private CancellationTokenSource pidToken;
        private System.Threading.Tasks.Task pidTask;

        public PidController(IAirHeaterCom plantReader)
        {
            _plantReader = plantReader;
            StartPid();
        }

        private double CalculateGain()
        {
            var e = SetPoint - Tout;
            var u = Kp * e + (Kp / Ti) * z;
            z = z + Ts/1000.0 * e;
            return Math.Max(Math.Min(u, MaxGain), MinGain); // Make sure gain is within 0-5V range
        }

        private void UpdatePlant(double setPoint)
        {
            Tout = _plantReader.GetFilteredTemperature();
            CurrentGain = CalculateGain();
            _plantReader.SetGain(CurrentGain);
        }

        private void StartPid()
        {
            pidToken = new CancellationTokenSource();

            pidTask = Task.Run(async () =>
            {
                while (!pidToken.IsCancellationRequested)
                {
                    var delayTask = Task.Delay(Ts, pidToken.Token);
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
