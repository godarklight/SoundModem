using System;
using System.IO;

namespace SoundModem
{
    public class FM : IInput
    {
        double phaseAngle;
        double carrier;
        double sampleRate;
        Stream inData;

        public FM(double carrier, double sampleRate, Stream inData)
        {
            this.carrier = carrier;
            this.inData = inData;
            this.sampleRate = sampleRate;
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
                }
                short s16le = (short)((byte2 << 8) + byte1);
                double amplitude = s16le / 32767d;
                double freqShift = amplitude * 1500d;
                phaseAngle = phaseAngle + (Math.Tau * (carrier + freqShift) / sampleRate);
                double value = Math.Sin(phaseAngle);
                samples[i] = value;
                currentSample++;
            }
            return currentSample;
        }
    }
}