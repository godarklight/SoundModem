using System;
using System.IO;

namespace SoundModem
{
    public class RTTY : IInput
    {
        Baudot baudot;
        int samplesPerUnit;
        int sampleRate;
        int freqLow;
        int freqHigh;
        double lastAngle = 0;

        public RTTY(int sampleRate, int frequency, int shift, double baud, Stream inputData)
        {
            this.sampleRate = sampleRate;
            this.freqLow = frequency - (shift / 2);
            this.freqHigh = frequency + (shift / 2);
            samplesPerUnit = (int)(sampleRate / baud);
            baudot = new Baudot(inputData);
        }
        public bool GetInput(IFormat output)
        {
            byte[] sendData = baudot.GetData();
            if (sendData == null)
            {
                return false;
            }

            Tone.Write(output, samplesPerUnit, freqLow, sampleRate, ref lastAngle);

            //Data bits
            for (int i = 0; i < sendData.Length; i++)
            {
                byte current = sendData[i];
                int freqToSend = freqLow;
                if (current == 1)
                {
                    freqToSend = freqHigh;
                }
                Tone.Write(output, samplesPerUnit, freqToSend, sampleRate, ref lastAngle);
            }
            
            //Stop bit
            Tone.Write(output, (int)(samplesPerUnit * 1.5), freqHigh, sampleRate, ref lastAngle);
            return true;
        }
    }
}