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
        public double r;
        public double Kp;
        public double Ti;
        public double Ts;
        public double z;
        private IDataReader _plantReader;
        private CancellationTokenSource pidToken;
        private System.Threading.Tasks.Task pidTask;
        public double SetPoint { get; set; }
        public PidController(IDataReader plantReader)
        {
            _plantReader = plantReader;
            StartPid();
        }

        public double PiController(double y)
        {
            var e = r - y;
            var u = Kp * e + (Kp / Ti) * z;
            z = z + Ts * e;
            return u;
        }

        private void UpdatePlant(double setPoint)
        {
            
        }

        private void StartPid()
        {
            pidToken = new CancellationTokenSource();

            pidTask = Task.Run(async () =>
            {
                while (!pidToken.IsCancellationRequested)
                {
                    var delayTask = Task.Delay(100, pidToken.Token);
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
