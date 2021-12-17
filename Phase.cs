/*
using System;

namespace SoundModem
{
    public static class Phase
    {
        public static int Write(double[] samples, int startPos, int length, double frequency, int sampleRate, bool data, ref int phaseShift, int phases, ref double lastPhaseAngle)
        {
            double samplesPerCycle = sampleRate / (double)frequency;
            //Keep the carrier wave in sync
            double phaseOffset = lastPhaseAngle + ((1 / samplesPerCycle) * Math.Tau);
            int midpoint = length / 2;
            for (int i = 0; i < length; i++)
            {
                double amplitude = 1;
                if (data == false)
                {
                    amplitude = (1d + Math.Cos(Math.Tau * (i / (double)length))) / 2d;
                    if (i == midpoint)
                    {
                        //Shift phase
                        phaseShift++;
                        if (phaseShift == phases)
                        {
                            phaseShift = 0;
                        }
                    }
                }
                double phaseAngle = Math.Tau * (i / samplesPerCycle);
                //Save unshifted phase angle
                if (i == length - 1)
                {
                    lastPhaseAngle = (phaseAngle + phaseOffset) % Math.Tau;
                }
                phaseAngle += Math.Tau * (phaseShift / (double)phases);
                double value = 0.5 * amplitude * Math.Sin(phaseAngle + phaseOffset);
                samples[startPos + i] = value;
            }
            return length;
        }
    }
}
*/