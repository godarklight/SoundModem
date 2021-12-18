using System;
using System.IO;

namespace SoundModem
{
    public class AM : IInput
    {
        double phaseAngle;
        double carrier;
        double sampleRate;
        IFormat inData;

        public AM(double carrier, double sampleRate, IFormat inData)
        {
            this.carrier = carrier;
            this.inData = inData;
            this.sampleRate = sampleRate;
        }

        public bool GetInput(IFormat output)
        {
            for (int i = 0; i < 128; i++)
            {
                double? amplitude = inData.ReadInput();
                if (amplitude == null)
                {
                    return false;
                }

                //Shift from -1:1 to 0:1
                double amplitudeIQ = 0.5 + (amplitude.Value / 2d);

                phaseAngle = phaseAngle + (Math.Tau * carrier / sampleRate);
                double value = amplitudeIQ * Math.Sin(phaseAngle);
                output.WriteOutput(value);
            }
            return true;
        }
    }
}