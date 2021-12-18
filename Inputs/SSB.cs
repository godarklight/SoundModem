using System;
using System.Numerics;
using System.IO;

namespace SoundModem
{
    public class SSB : IInput
    {
        bool upperSideBand;
        double voiceAngle;
        double carrierAngle;
        double carrier;
        double sampleRate;
        IFormat inData;
        IFilter voiceFilter;
        IFilter filterI;
        IFilter filterQ;
        const double voiceBandwidth = 2000;

        public SSB(double carrier, double sampleRate, IFormat inData, bool upperSideBand)
        {
            this.upperSideBand = upperSideBand;
            this.carrier = carrier;
            this.inData = inData;
            this.sampleRate = sampleRate;
            voiceFilter = new WindowedSinc(voiceBandwidth * 0.5, 2048, sampleRate);
            filterI = new LayeredFilter(FilterGenerator, 2);
            filterQ = new LayeredFilter(FilterGenerator, 2);
        }

        //Need tighter rolloff for IQ
        private IFilter FilterGenerator(int iteration)
        {
            IFilter retVal = null;
            switch (iteration)
            {
                case 0:
                    //Remove upper side band
                    retVal = new WindowedSinc(voiceBandwidth * 0.85, 1024, sampleRate);
                    break;
                case 1:
                    //Supress the carrier frequency
                    retVal = new BandRejectFilter(voiceBandwidth, 50, sampleRate);
                    break;
                default:
                    //Keep voice range
                    break;
            }
            return retVal;
        }

        public bool GetInput(IFormat output)
        {
            double carrierShift = carrier + voiceBandwidth;
            if (!upperSideBand)
            {
                carrierShift = carrier - voiceBandwidth;
            }

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
                carrierAngle = carrierAngle + (Math.Tau * carrierShift / sampleRate);

                //Calculate I and Q
                double valuei = amplitudeIQ * Math.Cos(voiceAngle);
                double valueq = amplitudeIQ * Math.Sin(voiceAngle);
                filterI.AddSample(valuei);
                filterQ.AddSample(valueq);

                //Modulate at carrier frequency
                double samplei = 4 * filterI.GetSample() * Math.Cos(carrierAngle);
                double sampleq = 4 * filterQ.GetSample() * Math.Sin(carrierAngle);

                if (upperSideBand)
                {
                    output.WriteOutput(samplei + sampleq);
                }
                else
                {
                    output.WriteOutput(samplei - sampleq);
                }
            }
            return true;
        }
    }
}