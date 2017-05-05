﻿using System;
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
        private readonly Stopwatch stopwatch;

        public AirHeaterSimulation(double roomTemperature, double u)
        {
            _u = u;
            Tenv = roomTemperature;
            token = new CancellationTokenSource();
            Tout = roomTemperature;
            stopwatch = new Stopwatch();
            StartAirHeater();
        }

        private const double Td = 2; //Time delay (s).
        private const double Tc = 22; // Time constant (s).
        private double Kh = 3.5; // Time constant (s).
        private readonly double Tenv; // Room/environmental temperature (Celsius).
        public double Tout; // Temperature at tube outlet.
        public double steps = Td / 0.1; // (Td/0.1)
        public int stepCount = 0;
        private double u_start;

        private double u_actual;

        //public double T;
        private double _u;

        public double u
        {
            get => Math.Round(_u, 2);
            set
            {
                _u = value;
                u_start = u_actual;
                stepCount = 0;
                stopwatch.Reset();
                stopwatch.Start();
            }
        }


        public double T_update(double u, double t)
        {
            var tchange = (-Tout + (Kh * u_actual + Tenv)) / Tc;
            Tout = tchange * 0.1 + Tout;
            return tchange;
        }

        public void UpdateGain(double u, double t)
        {
            if (stepCount < steps)
            {
                u_actual += (u - u_start) / steps;
                stepCount++;
            }
            else
            {
                u_actual = u;
            }
        }

        private void StartAirHeater()
        {
            token = new CancellationTokenSource();

            task = System.Threading.Tasks.Task.Run(async () =>
            {
                stopwatch.Start();
                while (!token.IsCancellationRequested)
                {
                    var waitTask = Task.Delay(100, token.Token);
                    var t = stopwatch.ElapsedMilliseconds / 1000.0;
                    UpdateGain(u, t);
                    T_update(u, t);
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