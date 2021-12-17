//This is a first order filter. We need better than this.
using System;
namespace SoundModem
{
    class LowPassFilter
    {
        double filterValue;
        double weight;

        public LowPassFilter(double frequency, double sampleRate)
        {
            double topHalf = Math.Tau * (1d / sampleRate) * frequency;
            double bottomHalf = topHalf + 1;
            weight = topHalf / bottomHalf;
        }

        public void AddSample(double input)
        {
            filterValue = (weight * input) + ((1 - weight) * filterValue);
        }

        public double GetSample()
        {
            return filterValue;
        }
    }
}