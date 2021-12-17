using System;
using System.IO;

namespace SoundModem
{
    public class BPSK : IInput
    {
        bool syncSent = false;
        bool endSent = false;
        double phaseAngle;
        int samplesPerUnit;
        int sampleRate;
        double fromI = 1;
        //Q wave is set to 0 in BPSK
        double fromQ = 0;
        PSKAlphabet pska;
        public BPSK(int sampleRate, double baud, Stream inputData)
        {
            pska = new PSKAlphabet(inputData);
            this.sampleRate = sampleRate;
            this.samplesPerUnit = (int)(sampleRate / baud);
        }

        public int GetInput(double[] samples)
        {
            int currentSample = 0;
            if (endSent)
            {
                return 0;
            }
            //Send sync pulses
            if (!syncSent)
            {
                syncSent = true;
                for (int i = 0; i < 32; i++)
                {
                    IQ.Write(samples, ref currentSample, samplesPerUnit, 1500, sampleRate, ref fromI, ref fromQ, -fromI, 0, ref phaseAngle);
                }
                return currentSample;
            }

            //Send data;
            int? sendData = pska.GetData();
            if (sendData == null)
            {
                //Ends with carrier
                for (int i = 0; i < 32; i++)
                {
                    IQ.Write(samples, ref currentSample, samplesPerUnit, 1500, sampleRate, ref fromI, ref fromQ, fromI, 0, ref phaseAngle);
                }
                endSent = true;
                return currentSample;
            }

            while (sendData > 0)
            {
                int sendBit = sendData.Value & 1;
                sendData = sendData >> 1;
                double newI = fromI;
                if (sendBit == 0)
                {
                    newI = -newI;
                }
                IQ.Write(samples, ref currentSample, samplesPerUnit, 1500, sampleRate, ref fromI, ref fromQ, newI, 0, ref phaseAngle);
            }

            //Interspace
            for (int i = 0; i < 2; i++)
            {
                IQ.Write(samples, ref currentSample, samplesPerUnit, 1500, sampleRate, ref fromI, ref fromQ, -fromI, 0, ref phaseAngle);
            }

            return currentSample;
        }
    }
}