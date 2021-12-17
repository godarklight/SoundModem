using System;

namespace SoundModem
{
    public static class IQ
    {
        private static double lastValue = 0;
        public static void Write(double[] samples, ref int startPos, int length, double frequency, int sampleRate, ref double fromI, ref double fromQ, double toI, double toQ, ref double lastPhaseAngle)
        {
            double samplesPerCycle = sampleRate / (double)frequency;
            //Keep in sync with the carrier
            double phaseOffset = lastPhaseAngle + ((1 / samplesPerCycle) * Math.Tau);
            int midpoint = length / 2;
            for (int i = 0; i < length; i++)
            {
                double phasePercent = (i / samplesPerCycle) % samplesPerCycle;
                double phaseAngle = ((Math.Tau * phasePercent) + phaseOffset) % Math.Tau;
                double cyclePercent = i / (double)(length - 1);
                //Sine ramp smoothing
                double smoothCyclePercent = (1f + Math.Sin((-Math.PI / 2) + (Math.PI * cyclePercent))) / 2f;

                //I wave
                double iAmplitude = fromI + (smoothCyclePercent * (toI - fromI));
                double iValue = iAmplitude * Math.Cos(phaseAngle);

                //Q wave
                double qAmplitude = fromQ + (smoothCyclePercent * (toQ - fromQ));
                double qValue = qAmplitude * Math.Sin(phaseAngle);

                //Total, normalised
                double value = 0.25 * (iValue + qValue);
                double delta = Math.Abs(value - lastValue);
                lastValue = value;


                samples[startPos + i] = value;

                //Sum
                if (i == length - 1)
                {
                    lastPhaseAngle = phaseAngle;
                }
            }
            fromI = toI;
            fromQ = toQ;
            startPos += length;
        }
    }
}