using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
        private string _dbConnectionString;
        public OpcReader(DataSocket socket)
        {
            _dbConnectionString = ConfigurationManager.ConnectionStrings["PlantContext"].ConnectionString;
            _socket = socket;
        }
        public double? ReadFloatingTag(string tag)
        {
            try
            {
                if (_socket.IsConnected) _socket.Disconnect();
                _socket.Connect(_url + tag, AccessMode.Read);
                if (_socket.IsConnected)
                {
                    _socket.Update();
                    var value = Convert.ToDouble(_socket.Data.Value);
                    _socket.Disconnect();
                    ClearConnectionError(tag);
                    return value;
                }
                else
                {
                    LogConnectionError(tag);
                    return null;
                }
               
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }

        public void ClearConnectionError(string tag)
        {
            try
            {
                using (SqlConnection dbCon = new SqlConnection(_dbConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("ClearConnectionError", dbCon);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Tag", tag));
                    dbCon.Open();
                    cmd.ExecuteNonQuery();
                    dbCon.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public void LogConnectionError(string tag)
        {
            try
            {
                using (SqlConnection dbCon = new SqlConnection(_dbConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("ConnectionError", dbCon);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Tag", tag));
                    dbCon.Open();
                    cmd.ExecuteNonQuery();
                    dbCon.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
