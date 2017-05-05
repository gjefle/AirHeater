using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AirHeater.ControlSystem.PlantCom;

namespace AirHeater.ControlSystem.PID
{
    public class PidController
    {
        private double Tout;
        private double Kp = 0.515; // T/K(Tc+Td);
        private double Ti = 18.3; // min(T, c(Tc +Td)
        private int Ts = 100;
        private double z;
        private IDataReader _plantReader;
        private CancellationTokenSource pidToken;
        private System.Threading.Tasks.Task pidTask;
        public double SetPoint { get; set; } = 25;
        public PidController(IDataReader plantReader)
        {
            _plantReader = plantReader;
            StartPid();
        }

        public double PiController()
        {
            var e = SetPoint - Tout;
            var u = Kp * e + (Kp / Ti) * z;
            z = z + Ts/1000.0 * e;
            return u;
        }

        private void UpdatePlant(double setPoint)
        {
            Tout = _plantReader.GetTemperature();
            var u = PiController();
            _plantReader.SetGain(u);
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
        void StopPid()
        {
            pidToken.Cancel();
            try
            {
                pidTask.Wait();
            }
            catch (AggregateException) { }
        }
    }
}
