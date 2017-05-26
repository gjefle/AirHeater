using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirHeater.ControlSystem.Filter;
using NationalInstruments.DAQmx;

namespace AirHeater.ControlSystem.PlantCom
{
    public class AirHeaterReader : IAirHeaterCom
    {
        private IFilter _filter;

        public AirHeaterReader(IFilter filter)
        {
            _filter = filter;
        }

        public double GetFilteredTemperature()
        {
            var temperature = ReadTemperature();
            return _filter.FilterNewValue(temperature);
        }
        public double ReadTemperature()
        {
            var niTask = new NationalInstruments.DAQmx.Task();
            AIChannel analogInput = niTask.AIChannels.CreateVoltageChannel(
                "Dev8/ai0",
                "Temperature",
                AITerminalConfiguration.Rse,
                1,
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
                "Dev8/ao0",
                "SetGain",
                0,
                5,
                AOVoltageUnits.Volts
            );
            var writer = new AnalogSingleChannelWriter(niTask.Stream);
            writer.WriteSingleSample(true, gain);
            
        }
    }
}
