//Reference http://dspguide.com/ch19/3.htm

using System;
namespace SoundModem
{
    class LayeredFilter : IFilter
    {
        IFilter[] filters;
        public LayeredFilter(Func<int, IFilter> generator, int iterations)
        {
            filters = new IFilter[iterations];
            for (int i = 0; i < iterations; i++)
            {
                filters[i] = generator(i);
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