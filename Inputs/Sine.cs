using System;

namespace SoundModem
{
    public class Sine : IInput
    {
        double phaseAngle;
        double cycleShift;
        double timeLeft;
        double sampleRate;
        public Sine(double sampleRate, double frequency, double seconds)
        {
            this.sampleRate = sampleRate;
            this.timeLeft = seconds;
            cycleShift = Math.Tau * frequency / sampleRate;
        }

        public bool GetInput(IFormat output)
        {
            for (int i = 0; i < 128; i++)
            {
                phaseAngle += cycleShift % Math.Tau;
                timeLeft -= 1 / sampleRate;
                if (timeLeft < 0)
                {
                    return i > 0;
                }
                output.WriteOutput(0.5 * Math.Sin(phaseAngle));
            }
            return true;
        }
    }
}