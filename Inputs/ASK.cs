using System;
using System.IO;

namespace SoundModem
{
    public class CW : IInput
    {
        Morse m = new Morse();
        int samplesPerUnit;
        int sampleRate;
        Stream inputData;
        byte[] bytesToSend;
        int byteToSendPos;
        public CW(int sampleRate, int wpm, Stream inputData)
        {
            this.sampleRate = sampleRate;
            this.inputData = inputData;
            samplesPerUnit = (sampleRate * 60) / (50 * wpm);
        }

        public int GetInput(double[] samples)
        {
            int currentSample = 0;
            if (bytesToSend == null)
            {
                int data = inputData.ReadByte();
                if (data == -1)
                {
                    return 0;
                }
                if (data != ' ')
                {
                    if (data != '\n')
                    {
                        if (char.IsLetter((char)data))
                        {
                            data = char.ToUpper((char)data);
                        }
                        bytesToSend = m.chars[(char)data];
                    }
                    else
                    {
                        //20 for newlines?
                        Tone.WriteSilence(samples, ref currentSample, 17 * samplesPerUnit);
                        return currentSample;
                    }
                }
                else
                {
                    //7 samples between words
                    Tone.WriteSilence(samples, ref currentSample, 4 * samplesPerUnit);
                    return currentSample;
                }
            }

            byte b = bytesToSend[byteToSendPos];

            //Write dit
            if (b == 0)
            {
                double phase = 0;
                Tone.Write(samples, ref currentSample, samplesPerUnit, 700, 48000, ref phase);
                Tone.WriteToZero(samples, ref currentSample, 700, 48000, ref phase);
            }

            //Write dah
            if (b == 1)
            {
                double phase = 0;
                Tone.Write(samples, ref currentSample, 3 * samplesPerUnit, 700, 48000, ref phase);
                Tone.WriteToZero(samples, ref currentSample, 700, 48000, ref phase);
            }

            //Write character space
            Tone.WriteSilence(samples, ref currentSample, samplesPerUnit);

            //Next word
            byteToSendPos++;
            if (bytesToSend.Length == byteToSendPos)
            {
                //3 samples between characters
                Tone.WriteSilence(samples, ref currentSample, 2 * samplesPerUnit);
                bytesToSend = null;
                byteToSendPos = 0;
            }
            return currentSample;
        }
    }
}