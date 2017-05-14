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
                "Dev9/ai0",
                "Gain",
                AITerminalConfiguration.Differential,
                0,
                5,
                AIVoltageUnits.Volts
            );
            var reader = new AnalogSingleChannelReader(niTask.Stream);
            return reader.ReadSingleSample();
        }

        public void SetProcessValue(double temperature)
        {
            var gain = PlantCalculations.CalcTemperatureToVolt(temperature);
            var niTask = new NationalInstruments.DAQmx.Task();
            var analogOutput = niTask.AOChannels.CreateVoltageChannel(
                "Dev9/ao0",
                "PV",
                1,
                5,
                AOVoltageUnits.Volts
            );
            var writer = new AnalogSingleChannelWriter(niTask.Stream);
            writer.WriteSingleSample(true, Math.Min(Math.Max(gain, 1), 5));
        }

        public void SetSetPoint(double temperature)
        {
            var gain = PlantCalculations.CalcTemperatureToVolt(temperature);
            var niTask = new NationalInstruments.DAQmx.Task();
            var analogOutput = niTask.AOChannels.CreateVoltageChannel(
                "Dev9/ao1",
                "SV",
                1,
                5,
                AOVoltageUnits.Volts
            );
            var writer = new AnalogSingleChannelWriter(niTask.Stream);
            writer.WriteSingleSample(true, Math.Min(Math.Max(gain, 1), 5));
        }
    }
}
