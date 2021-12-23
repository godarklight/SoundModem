//http://www.dspguide.com/ch16/1.htm
using System;
using System.IO;
namespace SoundModem
{
    public class WindowedSinc : IFilter
    {
        private double[] inputValues;
        ///Sinc wave
        private double[] filterValues;
        //Sinc points
        private int writePos = 0;
        private double filterValue;

        public WindowedSinc(double frequency, double transistionBandwidth, double sampleRate)
        {
            //Odd length
            int points = (int)(4 * sampleRate / transistionBandwidth);
            if (points % 2 == 1)
            {
                points++;
            }

            inputValues = new double[points + 1];
            filterValues = new double[points + 1];

            double filterFreq = frequency / sampleRate;
            for (int i = 0; i < filterValues.Length; i++)
            {
                int adjustI = i - (filterValues.Length / 2);
                //Hamming window
                double window = 0.54 - (0.46 * Math.Cos(Math.Tau * i / (double)points));
                //Blackman
                //double window = 0.42 - (0.5 * Math.Cos(Math.Tau * i / (double)points)) + (0.08 * Math.Cos(2 * Math.Tau * i / (double)points));

                //Mid point
                if (adjustI == 0)
                {
                    filterValues[i] = Math.Tau * filterFreq;
                }
                else
                {
                    double filterValue = Math.Sin(Math.Tau * filterFreq * adjustI) / adjustI;
                    filterValues[i] = window * filterValue;
                }
            }

            Normalise();
        }

        private void Normalise()
        {
            double sum = 0;
            for (int i = 0; i < filterValues.Length; i++)
            {
                sum += filterValues[i];
            }

            for (int i = 0; i < filterValues.Length; i++)
            {
                filterValues[i] = filterValues[i] / sum;
            }
        }

        public void AddSample(double input)
        {
            double newValue = 0;
            inputValues[writePos] = input;
            for (int i = 0; i < filterValues.Length; i++)
            {
                int adjustI = (writePos + i) % filterValues.Length;
                newValue += inputValues[adjustI] * filterValues[i];
            }
            writePos = (writePos + 1) % inputValues.Length;
            filterValue = newValue;
        }

        public double GetSample()
        {
            return filterValue;
        }
    }
}