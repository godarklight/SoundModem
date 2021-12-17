using System;
using System.IO;

namespace SoundModem
{
    public class AM : IInput
    {
        double phaseAngle;
        double carrier;
        double sampleRate;
        Stream inData;

        public AM(double carrier, double sampleRate, Stream inData)
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
                double amplitude = 0.5d + (s16le / 65535d);
                phaseAngle = phaseAngle + (Math.Tau * carrier / sampleRate);
                double value = amplitude * Math.Sin(phaseAngle);
                samples[i] = value;
                currentSample++;
            }
            return currentSample;
        }
    }
}