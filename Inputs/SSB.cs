using System;
using System.Numerics;
using System.IO;

namespace SoundModem
{
    public class SSB : IInput
    {
        double voiceAngle;
        double carrierAngle;
        double carrier;
        double sampleRate;
        IFormat inData;
        IFilter voiceFilter;
        IFilter filterI;
        IFilter filterQ;
        const double voiceBandwidth = 2000;

        public SSB(double carrier, double sampleRate, IFormat inData)
        {
            this.carrier = carrier;
            this.inData = inData;
            this.sampleRate = sampleRate;
            voiceFilter = new LayeredFilter(FilterGenerator, 2);
            filterI = new LayeredFilter(FilterGenerator, 2);
            filterQ = new LayeredFilter(FilterGenerator, 2);
        }

        //FilterI and FilterQ return the lower side band
        private IFilter FilterGenerator(int iteration)
        {
            IFilter retVal = null;
            switch (iteration)
            {
                case 0:
                    //Remove upper side band
                    retVal = new WindowedSinc(voiceBandwidth * 0.5, 2048, sampleRate);
                    break;
                case 1:
                    //Supress the carrier frequency
                    retVal = new BandRejectFilter(voiceBandwidth, voiceBandwidth * 0.1, sampleRate);
                    break;
                default:
                    //Keep voice range
                    break;
            }
            return retVal;
        }

        public bool GetInput(IFormat output)
        {
            for (int i = 0; i < 128; i++)
            {
                double? amplitude = inData.ReadInput();
                if (amplitude == null)
                {
                    return i > 0;
                }

                //Filter out frequencies we can't encode
                voiceFilter.AddSample(amplitude.Value);

                //Shift from -1:1 to 0:1 (AM modulation)
                double amplitudeIQ = 0.5 + (voiceFilter.GetSample() / 2d);

                //Calculate phases
                voiceAngle = voiceAngle + (Math.Tau * voiceBandwidth / sampleRate);
                carrierAngle = carrierAngle + (Math.Tau * (carrier + voiceBandwidth) / sampleRate);

                //Calculate I and Q
                double valuei = amplitudeIQ * Math.Cos(voiceAngle);
                double valueq = amplitudeIQ * Math.Sin(voiceAngle);
                filterI.AddSample(valuei);
                filterQ.AddSample(valueq);

                //Modulate at carrier frequency
                double samplei = 2 * filterI.GetSample() * Math.Cos(carrierAngle);
                double sampleq = 2 * filterQ.GetSample() * Math.Sin(carrierAngle);

                //output.WriteOutput(voiceFilter.GetSample());
                output.WriteOutput(samplei + sampleq);
            }
            return true;
        }
    }
}