using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private CancellationTokenSource pidToken;
        private IAirHeaterCom airHeaterCom;
        private PidController pidControl;
        private OpcClient opcClient;
        private CancellationTokenSource token;
        private System.Threading.Tasks.Task pidTask;
        private AnalogWaveform<double> analogWaveform;
        //private AnalogWaveform<double> unfilteredAnalogWaveform;

        public MainWindow()
        {
            airHeaterCom = new SimulatedHeaterReader(new LowPassFilter(21.5), new AirHeaterSimulation(21.5, 0));
            //airHeater = new DaqReader(new LowPassFilter(21.5));
            analogWaveform = new AnalogWaveform<double>(0);
            //unfilteredAnalogWaveform = new AnalogWaveform<double>(0);
            InitializeComponent();
            TemperatureGraph.DataSource = analogWaveform;
            //TemperatureGraphUnfiltered.DataSource = unfilteredAnalogWaveform;
            DataContext = this;
            RunViewUpdater();
            pidControl = new PidController(airHeaterCom);
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
                    await Task.Delay(200, token.Token);
                }
            });

        }
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

        private double _temperature;
        private double _setPoint;

        public double Temperature
        {
            get => Math.Round(_temperature, 2);
            set
            {
                _temperature = value;
                OnPropertyChanged("Temperature");
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
                OnPropertyChanged("Temperature");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    double val;
        //    if (double.TryParse(Gainbox.Text, out val))
        //    {
        //        if (pidControl != null)
        //            pidControl.SetPoint = val;
        //    }

        //}

        //private void ScrollBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        //{
        //    SetPoint = SetPointScrollBar.Value;
        //}
    }
}