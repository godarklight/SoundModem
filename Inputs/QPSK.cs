using System;
using System.IO;

namespace SoundModem
{
    public class QPSK : IInput
    {
        bool syncSent = false;
        bool endSent = false;
        double phaseAngle;
        int samplesPerUnit;
        int sampleRate;
        double fromI = 1;
        double fromQ = 1;
        PSKAlphabet pska;
        PSKConvolution pskc;
        public QPSK(int sampleRate, double baud, Stream inputData)
        {
            pska = new PSKAlphabet(inputData);
            pskc = new PSKConvolution();
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
                    int phase = pskc.GetPhase(0);
                    IQ.Write(samples, ref currentSample, samplesPerUnit, 1500, sampleRate, ref fromI, ref fromQ, GetI(phase), GetQ(phase), ref phaseAngle);
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
                    int phase = pskc.GetPhase(1);
                    IQ.Write(samples, ref currentSample, samplesPerUnit, 1500, sampleRate, ref fromI, ref fromQ, GetI(phase), GetQ(phase), ref phaseAngle);
                }
                endSent = true;
                return currentSample;
            }

            while (sendData > 0)
            {
                int sendBit = sendData.Value & 1;
                sendData = sendData >> 1;
                int phase = pskc.GetPhase(sendBit);
                IQ.Write(samples, ref currentSample, samplesPerUnit, 1500, sampleRate, ref fromI, ref fromQ, GetI(phase), GetQ(phase), ref phaseAngle);
            }

            //Interspace
            for (int i = 0; i < 2; i++)
            {
                int phase = pskc.GetPhase(0);
                IQ.Write(samples, ref currentSample, samplesPerUnit, 1500, sampleRate, ref fromI, ref fromQ, GetI(phase), GetQ(phase), ref phaseAngle);
            }

            return currentSample;
        }

        //IQ Modulation
        //45DEG = 1, 1
        //135DEG = -1, 1
        //225DEG = -1, -1
        //315DEG = 1, -1

        public double GetI(int phase)
        {
            if (phase == 0 || phase == 3)
            {
                return 1;
            }
            return -1;
        }

        public double GetQ(int phase)
        {
            if (phase == 0 || phase == 1)
            {
                return 1;
            }
            return -1;
        }
    }
}