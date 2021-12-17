using System;

namespace SoundModem
{
    public class Sine : IInput
    {
        long sampleNumber;
        double sampleCycle;
        public Sine(double sampleRate, double frequency)
        {
            sampleCycle = sampleRate / frequency;
        }

        public int GetInput(double[] samples)
        {
            for (int i = 0; i < samples.Length; i++)
            {
                double cycleAngle = (sampleNumber / sampleCycle) * 2 * Math.PI;
                samples[i] = Math.Sin(cycleAngle);
                sampleNumber++;
                if (sampleNumber > sampleCycle)
                {
                    sampleNumber = 0;
                }
            }
            return samples.Length;
        }
    }
}