using System;
using System.IO;
using System.Text;
using System.Threading;

namespace SoundModem
{
    public class Program
    {
        public const int SAMPLE_RATE = 48000;
        public static void Main()
        {
            //Digital data
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 2; i++)
            {
                sb.Append("The quick brown fox jumps over the lazy dog 1234567890\n");
            }
            byte[] sendMessage = Encoding.ASCII.GetBytes(sb.ToString());
            MemoryStream digitalData = new MemoryStream(sendMessage);

            //Analog data
            FileStream fs = new FileStream("input.wav", FileMode.Open);
            S16LE analogData = new S16LE(fs, -1);

            //Inputs
            IInput input = null;
            //input = new Sine(SAMPLE_RATE, 440, 5);
            //input = new Sawtooth(SAMPLE_RATE, 440, 5);
            //input = new RTTY(SAMPLE_RATE, 500, 170, 45.45, digitalData);
            input = new CW(SAMPLE_RATE, 24, digitalData);
            //input = new MFSK(SAMPLE_RATE);
            //input = new BPSK(SAMPLE_RATE, 31.25, digitalData);
            //input = new BPSK(SAMPLE_RATE, 1000, digitalData);
            //input = new QPSK(SAMPLE_RATE, 31.25, digitalData);
            //input = new QPSK(SAMPLE_RATE, 500, digitalData);
            //input = new AM(2000, SAMPLE_RATE, analogData);
            //input = new FM(2000, SAMPLE_RATE, analogData);
            //input = new SSB(2000, SAMPLE_RATE, analogData, true);

            //Output format
            MemoryStream outStream = new MemoryStream();
            IFormat output = new S16LE(outStream, -1);

            //Sinks
            ISink sink = new OpenALSink();
            //ISink sink = new Wav("Examples/rtty.wav");

            //Main loop
            bool running = true;
            while (running)
            {
                if (!input.GetInput(output))
                {
                    running = false;
                    break;
                }
                sink.Write(outStream);
            }

            sink.Close();
        }
    }
}