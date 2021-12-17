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
            for (int i = 0; i < 100; i++)
            {
                sb.Append("The quick brown fox jumps over the lazy dog 1234567890\n");
            }
            byte[] sendMessage = Encoding.ASCII.GetBytes(sb.ToString());
            MemoryStream ms = new MemoryStream(sendMessage);

            //Analog data
            FileStream fs = new FileStream("input.wav", FileMode.Open);
            fs.Seek(44, SeekOrigin.Begin);

            //Inputs
            IInput input = null;
            //input = new RTTY(SAMPLE_RATE, 500, 170, 45.45, ms);
            //input = new CW(SAMPLE_RATE, 24, ms);
            //input = new MFSK(SAMPLE_RATE);
            //input = new BPSK(SAMPLE_RATE, 31.25, ms);
            //input = new BPSK(SAMPLE_RATE, 1000, ms);
            //input = new QPSK(SAMPLE_RATE, 31.25, ms);
            //input = new QPSK(SAMPLE_RATE, 500, ms);
            //input = new AM(2000, SAMPLE_RATE, fs);
            //input = new FM(2000, SAMPLE_RATE, fs);
            input = new SSB(5000, SAMPLE_RATE, fs);

            //Output format
            IOutput output = new S16LEOut();

            //Sinks
            //ISink sink = new OpenALSink();
            ISink sink = new Wav();

            //Sound buffers
            double[] sampleBuffer = new double[480000];
            byte[] outputBuffer = new byte[960000];

            //Main loop
            bool running = true;
            while (running)
            {
                int numberOfSamples = input.GetInput(sampleBuffer);
                if (numberOfSamples == 0)
                {
                    running = false;
                    break;
                }
                int numberOfBytes = output.GetOutput(sampleBuffer, numberOfSamples, outputBuffer);
                sink.Write(outputBuffer, numberOfBytes);
            }

            sink.Close();
        }
    }
}