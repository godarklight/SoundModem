//Reference https://en.wikipedia.org/wiki/Butterworth_filter
//Not working well...
using System;
namespace SoundModem
{
    class Butterworth
    {
        double[] inputCoeff = new double[3];
        double[] filterCoeff = new double[3];
        double[] inputValues = new double[3];
        double[] filterValues = new double[3];
        public Butterworth(double frequency, double sampleRate)
        {
            //https://www.meme.net.au/butterworth.html
            //https://stackoverflow.com/questions/20924868/calculate-coefficients-of-2nd-order-butterworth-low-pass-filter


            double QcRaw = (2 * Math.PI * frequency) / sampleRate; // Find cutoff frequency in [0..PI]
            double QcWarp = Math.Tan(QcRaw); // Warp cutoff frequency

            double gain = 1 / (1 + Math.Sqrt(2) / QcWarp + 2 / (QcWarp * QcWarp));
            inputCoeff[0] = 1 * gain;
            inputCoeff[1] = 2 * gain;
            inputCoeff[2] = 1 * gain;
            filterCoeff[0] = 0;
            filterCoeff[1] = (2 - 2 * 2 / (QcWarp * QcWarp)) * gain;
            filterCoeff[2] = (1 - Math.Sqrt(2) / QcWarp + 2 / (QcWarp * QcWarp)) * gain;
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