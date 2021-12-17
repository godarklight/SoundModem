//Reference http://dspguide.com/ch19/3.htm

using System;
namespace SoundModem
{
    class LayeredNotchFilter
    {
        NotchFilter[] filters;
        public LayeredNotchFilter(double frequency, double bandwidth, double sampleRate, int iterations)
        {
            filters = new NotchFilter[iterations];
            for (int i = 0; i < iterations; i++)
            {
                filters[i] = new NotchFilter(frequency, bandwidth, sampleRate);
            }
        }

        public void AddSample(double input)
        {
            for (int i = 0; i < filters.Length; i++)
            {
                filters[i].AddSample(input);
                input = filters[i].GetSample();
            }
        }

        public double GetSample()
        {
            return filters[filters.Length - 1].GetSample();
        }
    }
}