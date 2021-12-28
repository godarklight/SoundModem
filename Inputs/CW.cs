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

        public bool GetInput(IFormat output)
        {
            if (bytesToSend == null)
            {
                int data = inputData.ReadByte();
                if (data == -1)
                {
                    return false;
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
                        Tone.WriteSilence(output, 17 * samplesPerUnit);
                        return true;
                    }
                }
                else
                {
                    //7 samples between words
                    Tone.WriteSilence(output, 4 * samplesPerUnit);
                    return true;
                }
            }

            byte b = bytesToSend[byteToSendPos];

            //Write dit
            if (b == 0)
            {
                double phase = 0;
                Tone.WriteSoft(output, (int)(samplesPerUnit * 0.8), 700, 48000, 0.5d, ref phase);
            }

            //Write dah
            if (b == 1)
            {
                double phase = 0;
                Tone.WriteSoft(output, (int)(samplesPerUnit * 2.8), 700, 48000, 0.5d, ref phase);
            }

            //Write character space
            Tone.WriteSilence(output, samplesPerUnit);

            //Next word
            byteToSendPos++;
            if (bytesToSend.Length == byteToSendPos)
            {
                //3 samples between characters
                Tone.WriteSilence(output, 2 * samplesPerUnit);
                bytesToSend = null;
                byteToSendPos = 0;
            }
            return true;
        }
    }
}