using System;

namespace SoundModem
{
    public static class Tone
    {
        public static void Write(IFormat output, int length, double frequency, int sampleRate, double amplitude, ref double lastAngle)
        {
            for (int i = 0; i < length; i++)
            {
                lastAngle = (lastAngle + (Math.Tau * frequency / sampleRate)) % Math.Tau;
                output.WriteOutput(amplitude * Math.Sin(lastAngle));
            }
        }

        public static void WriteSoft(IFormat output, int length, double frequency, int sampleRate, double amplitude, ref double lastAngle)
        {
            double rampUp = length * 0.1;
            double rampDown = length * 0.9;
            for (int i = 0; i < length; i++)
            {
                double amplitudeAdjust = 1d;
                if (i < rampUp)
                {
                    amplitudeAdjust = i / rampUp;
                }
                if (i > rampDown)
                {
                    amplitudeAdjust = (length - i) / rampUp;
                }
                lastAngle = (lastAngle + (Math.Tau * frequency / sampleRate)) % Math.Tau;
                output.WriteOutput(amplitudeAdjust * amplitude * Math.Sin(lastAngle));
            }
        }

        public static void WriteToZero(IFormat output, double frequency, int sampleRate, double amplitude, ref double lastAngle)
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
                output.WriteOutput(amplitude * Math.Sin(lastAngle));
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