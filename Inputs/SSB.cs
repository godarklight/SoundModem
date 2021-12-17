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
        Stream inData;
        NotchFilter voiceNotchFilter;
        LayeredNotchFilter filterI;
        LayeredNotchFilter filterQ;
        const double voiceFilter = 2000;

        public SSB(double carrier, double sampleRate, Stream inData)
        {
            this.carrier = carrier;
            this.inData = inData;
            this.sampleRate = sampleRate;
            double center = voiceFilter * 0.5;
            double bandwidth = voiceFilter * 0.45;
            voiceNotchFilter = new NotchFilter(voiceFilter / 2, voiceFilter / 2, sampleRate);
            filterI = new LayeredNotchFilter(center, bandwidth, sampleRate, 5);
            filterQ = new LayeredNotchFilter(center, bandwidth, sampleRate, 5);
        }

        public int GetInput(double[] samples)
        {
            int currentSample = 0;
            for (int i = 0; i < 128; i++)
            {
                int byte1 = inData.ReadByte();
                int byte2 = inData.ReadByte();
                if (byte1 == -1 || byte2 == -1)
                {
                    inData.Seek(44, SeekOrigin.Begin);
                    byte1 = inData.ReadByte();
                    byte2 = inData.ReadByte();
                    return 0;
                }
                short s16le = (short)((byte2 << 8) + byte1);
                double amplitude = s16le / 32767d;

                //Notch filter voice frequencies
                voiceNotchFilter.AddSample(amplitude);

                //Shift from -1:1 to 0:1
                double amplitudeIQ = 0.5 + (voiceNotchFilter.GetSample() / 2d);

                //Calculate phases
                voiceAngle = voiceAngle + (Math.Tau * voiceFilter / sampleRate);
                carrierAngle = carrierAngle + (Math.Tau * (carrier + voiceFilter) / sampleRate);

                //Calculate I and Q
                double valuei = amplitudeIQ * Math.Cos(voiceAngle);
                double valueq = amplitudeIQ * Math.Sin(voiceAngle);
                filterI.AddSample(valuei);
                filterQ.AddSample(valueq);

                //Construct carrier
                double samplei = filterI.GetSample() * Math.Cos(carrierAngle);
                double sampleq = filterQ.GetSample() * Math.Sin(carrierAngle);

                samples[i] = samplei + sampleq;
                //samples[i] = realFilter.GetSample();
                //samples[i] = filterI2.GetSample();
                currentSample++;
            }
            return currentSample;
        }
    }
}