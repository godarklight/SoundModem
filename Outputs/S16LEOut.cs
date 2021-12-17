using System;

namespace SoundModem
{
    public class S16LEOut : IOutput
    {
        public int GetOutput(double[] samples, int samplesLength, byte[] buffer)
        {
            for (int i = 0; i < samplesLength; i++)
            {
                short value = (short)(samples[i] * 32767);
                buffer[i * 2] = (byte)(value & 0xFF);
                buffer[i * 2 + 1] = (byte)(value >> 8);
            }
            return samplesLength * 2;
        }
    }
}