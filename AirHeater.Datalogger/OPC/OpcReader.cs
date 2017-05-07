using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NationalInstruments.Net;

namespace AirHeater.Datalogger.OPC
{
    

    public class OpcReader
    {
        private string _url = "opc://localhost/Matrikon.OPC.Simulation.1/.";
        private DataSocket _socket;

        public OpcReader(DataSocket socket)
        {
            _socket = socket;
        }
        public double ReadFloatingTag(string tag)
        {
            try
            {
                if (_socket.IsConnected) _socket.Disconnect();
                _socket.Connect(_url + tag, AccessMode.Read);
                _socket.Update();
                var value = Convert.ToDouble(_socket.Data.Value);
                _socket.Disconnect();
                return value;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
