using System;

namespace SoundModem
{
    public static class Tone
    {
        public static void Write(double[] samples, ref int startPos, int length, double frequency, int sampleRate, ref double lastAngle)
        {
            for (int i = 0; i < length; i++)
            {
                lastAngle = (lastAngle + (Math.Tau * frequency / sampleRate)) % Math.Tau;
                samples[startPos + i] = 0.5d * Math.Sin(lastAngle);
            }
            startPos = startPos + length;
        }

        public static void WriteToZero(double[] samples, ref int startPos, double frequency, int sampleRate, ref double lastAngle)
        {
            int i = 0;
            bool running = true;
            while (running)
            {
                double newAngle = (lastAngle + (Math.Tau * frequency / sampleRate)) % Math.Tau;
                //Detect the wrap around
                if (lastAngle > 6 && newAngle < 1)
                {
                    running = false;
                }
                lastAngle = newAngle;
                samples[i] = 0.5d * Math.Sin(lastAngle);
                i++;
            }
            lastAngle = 0;
            startPos = startPos + i;
        }

        public static void WriteSilence(double[] samples, ref int startPos, int length)
        {
            for (int i = 0; i < length; i++)
            {
                samples[startPos + i] = 0;
            }
            startPos = startPos + length;
        }
    }
}