using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NationalInstruments;
using NationalInstruments.Net;

namespace AirHeater.ControlSystem.OpcCom
{
    public class OpcClient
    {
        private string _url = "opc://localhost/Matrikon.OPC.Simulation/Bucket Brigade.Real4";
        private DataSocket _socket;

        public OpcClient()
        {
            _socket = new DataSocket();
        }

        public void WriteToServer(string tag, double value)
        {
            if(_socket.IsConnected) _socket.Disconnect();
            _socket.Connect(_url, AccessMode.Write);
            _socket.Data.Value = value;
            _socket.Update();
            _socket.Disconnect();
            //_socket.SyncWrite(value, 2000);
        }
    }
}
