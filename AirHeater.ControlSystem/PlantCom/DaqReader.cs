using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NationalInstruments.DAQmx;

namespace AirHeater.ControlSystem.PlantCom
{
    public class DaqReader : IDataReader
    {
        public double GetTemperature()
        {
            var niTask = new NationalInstruments.DAQmx.Task();
            AIChannel analogInput = niTask.AIChannels.CreateVoltageChannel(
                "dev1/ai0",
                "myAIChannel",
                AITerminalConfiguration.Differential,
                0,
                5,
                AIVoltageUnits.Volts
            );
            var reader = new AnalogSingleChannelReader(niTask.Stream);
            return PlantCalculations.CalcTemperature(reader.ReadSingleSample());
        }

        public void SetGain(double gain)
        {
            var niTask = new NationalInstruments.DAQmx.Task();
            var analogOutput= niTask.AOChannels.CreateVoltageChannel(
                "dev1/ao0",
                "myAIChannel",
                0,
                5,
                AOVoltageUnits.Volts
            );
            var writer = new AnalogSingleChannelWriter(niTask.Stream);
            writer.WriteSingleSample(true, gain);
            
        }
    }
}
