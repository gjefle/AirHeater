using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AirHeater.ControlSystem.PID;
using AirHeater.ControlSystem.PlantCom;
using NationalInstruments;
using NationalInstruments.Net;

namespace AirHeater.ControlSystem.OpcCom
{
    public class OpcClient : IDisposable
    {
        private string _url = "opc://localhost/Matrikon.OPC.Simulation.1/.";
        private DataSocket _socket;
        private CancellationTokenSource token;
        private System.Threading.Tasks.Task task;
        private IAirHeaterCom _heaterCom;
        private IPidCom _pidController;

        public OpcClient(IAirHeaterCom heaterCom, IPidCom pidController)
        {
            _heaterCom = heaterCom;
            _pidController = pidController;
            _socket = new DataSocket();
            StartAirHeater();
        }

        public void WriteToServer(string tag, double value)
        {
            try
            {
                if (_socket.IsConnected) _socket.Disconnect();
                _socket.Connect(_url + tag, AccessMode.Write);
                
                _socket.Data.Value = value;
                _socket.Update();
                _socket.Disconnect();
                //_socket.SyncWrite(value, 2000);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
           
        }


        private void StartAirHeater()
        {
            token = new CancellationTokenSource();

            task = System.Threading.Tasks.Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    var waitTask = Task.Delay(1000, token.Token);
                    var temperature = Math.Round(_heaterCom.GetFilteredTemperature(), 2);
                    var gain = Math.Round(_pidController.GetCurrentGain(), 2);
                    WriteToServer("Temperature", temperature);
                    WriteToServer("Gain", gain);
                    await waitTask;
                }
            });
        }
        void StopOpcWriter()
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
            StopOpcWriter();
        }
    }
}
