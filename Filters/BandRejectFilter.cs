//Reference http://dspguide.com/ch19/3.htm

using System;
namespace SoundModem
{
    public class BandRejectFilter : IFilter
    {
        double[] inputCoeff = new double[3];
        double[] filterCoeff = new double[3];
        double[] inputValues = new double[3];
        double[] filterValues = new double[3];
        public BandRejectFilter(double frequency, double bandwidth, double sampleRate)
        {
            double cosValue = Math.Cos(Math.Tau * (frequency / sampleRate));
            double Rvalue = 1 - 3 * (bandwidth / sampleRate);
            double KvalueTop = 1 - (2 * Rvalue * cosValue) + (Rvalue * Rvalue);
            double KvalueBottom = 2 - (2 * cosValue);
            double Kvalue = KvalueTop / KvalueBottom;
            inputCoeff[0] = Kvalue;
            inputCoeff[1] = -2 * Kvalue * cosValue;
            inputCoeff[2] = Kvalue;
            filterCoeff[0] = 0;
            filterCoeff[1] = 2 * Rvalue * cosValue;
            filterCoeff[2] = -(Rvalue * Rvalue);
        }

        public void AddSample(double input)
        {
            //Shift values
            inputValues[2] = inputValues[1];
            inputValues[1] = inputValues[0];
            inputValues[0] = input;
            filterValues[2] = filterValues[1];
            filterValues[1] = filterValues[0];


            //Calculate
            double newValue = 0;
            newValue += inputCoeff[0] * inputValues[0];
            newValue += inputCoeff[1] * inputValues[1];
            newValue += inputCoeff[2] * inputValues[2];
            newValue += filterCoeff[1] * filterValues[1];
            newValue += filterCoeff[2] * filterValues[2];
            filterValues[0] = newValue;
        }

        public double GetSample()
        {
            return filterValues[0];
        }
    }
}