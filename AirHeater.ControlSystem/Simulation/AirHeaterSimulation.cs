using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NationalInstruments;


namespace AirHeater.ControlSystem.Simulation
{
    public class AirHeaterSimulation : IDisposable
    {
        private CancellationTokenSource token;
        private System.Threading.Tasks.Task task;

        private const double Td = 2; //Time delay (s).
        private const double Tc = 22; // Time constant (s).
        private double Kh = 3.5; // Time constant (s).
        private readonly double Tenv; // Room/environmental temperature (Celsius).
        public double Tout; // Temperature at tube outlet.
        public double steps = Td / 0.1; // (Td/0.1)
        public int stepCount = 0;
        public Queue<double> u_delay;
        public AirHeaterSimulation(double roomTemperature, double u)
        {
            int delaySteps = Convert.ToInt32(Math.Round(steps));
            u_delay = new Queue<double>(delaySteps);
            for (int i = 0; i < delaySteps; i++)
            {
                u_delay.Enqueue(u);
            }
            Tenv = roomTemperature;
            token = new CancellationTokenSource();
            Tout = roomTemperature;
            StartAirHeater();
        }

        //public double u
        //{
        //    get
        //    {
        //        var gain = (u_delay.Dequeue());
        //        return Math.Round(double.IsNaN(gain) ? 0 : gain, 2);
        //    }
        //    set
        //    {
        //        u_delay.Enqueue(value);
        //    }
        //}

        public double u { get; set; }

        public void T_update(double gain)
        {
            var tchange = (-Tout + (Kh * gain + Tenv)) / Tc;
            Tout = tchange * 0.1 + Tout;
        }

        private void StartAirHeater()
        {
            token = new CancellationTokenSource();

            task = System.Threading.Tasks.Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    var waitTask = Task.Delay(100, token.Token);
                    var gain = u_delay.Dequeue();
                    u_delay.Enqueue(u);
                    T_update(gain);
                    await waitTask;
                }
            });
        }
        void StopAirHeater()
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

        public void Dispose()
        {
            StopAirHeater();
        }
    }
}