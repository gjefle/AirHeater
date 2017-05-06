using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NationalInstruments.DAQmx;

namespace AirHeater.ControlSystem.PID
{
    public class PidReader: IPidCom
    {
        
        public double GetCurrentGain()
        {
            var niTask = new NationalInstruments.DAQmx.Task();
            AIChannel analogInput = niTask.AIChannels.CreateVoltageChannel(
                "Dev1/ai0",
                "myAIChannel",
                AITerminalConfiguration.Differential,
                0,
                5,
                AIVoltageUnits.Volts
            );
            var reader = new AnalogSingleChannelReader(niTask.Stream);
            return reader.ReadSingleSample();
        }
    }
}
