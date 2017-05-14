using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using AirHeater.ControlSystem.Filtering;
using AirHeater.ControlSystem.OpcCom;
using AirHeater.ControlSystem.PID;
using AirHeater.ControlSystem.PlantCom;
using AirHeater.ControlSystem.Simulation;
using NationalInstruments;
using NationalInstruments.Controls;
using NationalInstruments.DAQmx;
using Task = System.Threading.Tasks.Task;

namespace AirHeater.ControlSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDisposable, INotifyPropertyChanged
    {
        private CancellationTokenSource pidToken;
        private IAirHeaterCom airHeaterCom;
        private PidController pidControl;
        //private PidReader realPid;
        private OpcClient opcClient;
        private CancellationTokenSource token;
        private System.Threading.Tasks.Task pidTask;
        private AnalogWaveform<double> analogWaveform;
        //private AnalogWaveform<double> unfilteredAnalogWaveform;

        public MainWindow()
        {
            airHeaterCom = new SimulatedHeaterReader(new LowPassFilter(21.5), new AirHeaterSimulation(21.5, 0));
            //airHeaterCom = new AirHeaterReader(new LowPassFilter(21.5));
            //airHeater = new DaqReader(new LowPassFilter(21.5));
            analogWaveform = new AnalogWaveform<double>(0);
            //unfilteredAnalogWaveform = new AnalogWaveform<double>(0);
            InitializeComponent();
            TemperatureGraph.DataSource = analogWaveform;
            //TemperatureGraphUnfiltered.DataSource = unfilteredAnalogWaveform;
            DataContext = this;
            RunViewUpdater();
            pidControl = new PidController(airHeaterCom);
            //realPid = new PidReader();
            opcClient = new OpcClient(airHeaterCom, pidControl);
            SetPoint = 23;
        }
        private void RunViewUpdater()
        {
            token = new CancellationTokenSource();
            var task = System.Threading.Tasks.Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    UpdateTemperatureData();
                    UpdatePid();
                    //UpdateSimulator();
                    await Task.Delay(500, token.Token);
                }
            });
        }

        public void UpdatePid()
        {
            Gain = pidControl.GetCurrentGain();
          

            //Gain = realPid.GetCurrentGain();
            //airHeaterCom.SetGain(Gain);
            //var t = airHeaterCom.GetFilteredTemperature();
            //realPid.SetProcessValue(t);
            OnPropertyChanged("Gain");
            OnPropertyChanged("GainLabel");
        }

        private double _gain;

        public double Gain
        {
            get => _gain;
            set
            {
                _gain = value;
                OnPropertyChanged("Gain");
            }
        }

        public string GainLabel => Math.Round(_gain, 1) + " V";
        public void UpdateTemperatureData()
        {
            //if (IsConnected)
            //{
            try
            {
                Temperature = airHeaterCom?.GetFilteredTemperature() ?? 0;
                analogWaveform.Append(AnalogWaveform<double>.FromArray1D(new double[] { Temperature }));
                //unfilteredAnalogWaveform.Append(AnalogWaveform<double>.FromArray1D(new double[] { airHeater?.ReadTemperature() ?? 0 }));
            }
            catch (Exception e) { throw e; }
            //}
        }

        private static double _temperature;
        private static double _setPoint;

        public double Temperature
        {
            get => Math.Round(_temperature, 2);
            set
            {
                _temperature = value;
                OnPropertyChanged("Temperature");
                OnPropertyChanged("TemperatureLabel");
            }
        }

        public double SetPoint
        {
            get => _setPoint;
            set
            {
                _setPoint = value;
                if (pidControl != null)
                    pidControl.SetPoint = value;
                //if (realPid != null)
                //{
                //    realPid.SetSetPoint(_setPoint);
                //}

                OnPropertyChanged("SetPoint");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose()
        {
            token.Cancel();
            pidToken.Cancel();
        }

        private void ArrowButton_Click(object sender, RoutedEventArgs e)
        {
            this.SetPoint += 1;
            OnPropertyChanged("SetPoint");
        }

        
        public string TemperatureLabel => Math.Round(_temperature, 1) + " C";

        private void ArrowButton_Click_1(object sender, RoutedEventArgs e)
        {
            this.SetPoint -= 1;
            OnPropertyChanged("SetPoint");
        }
    }
}