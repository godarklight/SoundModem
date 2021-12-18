using System;

namespace SoundModem
{
    public static class Tone
    {
        public static void Write(IFormat output, int length, double frequency, int sampleRate, ref double lastAngle)
        {
            for (int i = 0; i < length; i++)
            {
                lastAngle = (lastAngle + (Math.Tau * frequency / sampleRate)) % Math.Tau;
                output.WriteOutput(0.5d * Math.Sin(lastAngle));
            }
        }

        public static void WriteToZero(IFormat output, double frequency, int sampleRate, ref double lastAngle)
        {
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
                output.WriteOutput(0.5d * Math.Sin(lastAngle));
            }
            lastAngle = 0;
        }

        public static void WriteSilence(IFormat output, int length)
        {
            for (int i = 0; i < length; i++)
            {
                output.WriteOutput(0);
            }
        }
    }
}