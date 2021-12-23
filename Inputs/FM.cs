using System;
using System.IO;

namespace SoundModem
{
    //Bandwidth of FM channels is 9khz in real life, for this demo I will make it 1khz.
    public class FM : IInput
    {
        double phaseAngle;
        double carrier;
        double sampleRate;
        IFormat inData;
        IFilter voiceFilter;

        public FM(double carrier, double sampleRate, IFormat inData)
        {
            this.carrier = carrier;
            this.inData = inData;
            this.sampleRate = sampleRate;
            voiceFilter = new WindowedSinc(1000, 2048, sampleRate);
        }

        public bool GetInput(IFormat output)
        {
            int currentSample = 0;
            for (int i = 0; i < 128; i++)
            {
                double? amplitude = inData.ReadInput();
                if (amplitude == null)
                {
                    return i > 0;
                }
                voiceFilter.AddSample(amplitude.Value);
                double freqShift = voiceFilter.GetSample() * 1000;
                phaseAngle = phaseAngle + (Math.Tau * (carrier + freqShift) / sampleRate);
                double value = Math.Sin(phaseAngle);
                output.WriteOutput(value);
                currentSample++;
            }
            return true;
        }
    }
}