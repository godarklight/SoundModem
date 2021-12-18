using System;
using System.IO;

namespace SoundModem
{
    public class S16LE : IFormat
    {
        Stream stream;
        int loop;

        /// <summary>
        /// Convert between S16LE and byte[]
        /// </summary>
        /// <param name="stream">The stream to read or write</param>
        /// <param name="loop">The position to seek to in order to loop the stream. -1 for no loop.</param>
        public S16LE(Stream stream, int loop)
        {
            this.stream = stream;
            this.loop = loop;
        }

        public double? ReadInput()
        {
            int byte1 = stream.ReadByte();
            int byte2 = stream.ReadByte();
            if (byte1 == -1 || byte2 == -1)
            {
                if (loop > -1)
                {
                    stream.Seek(loop, SeekOrigin.Begin);
                    byte1 = stream.ReadByte();
                    byte2 = stream.ReadByte();
                }
                else
                {
                    return null;
                }
            }
            short value = (short)(byte2 << 8 | byte1);
            return value / 32767d;

        }
        public void WriteOutput(double[] samples, int samplesLength)
        {
            for (int i = 0; i < samplesLength; i++)
            {
                short value = (short)(samples[i] * 32767);
                byte byte1 = (byte)(value & 0xFF);
                byte byte2 = (byte)(value >> 8);
                stream.WriteByte(byte1);
                stream.WriteByte(byte2);
            }
        }

        public void WriteOutput(double sample)
        {
            short value = (short)(sample * 32767);
            byte byte1 = (byte)(value & 0xFF);
            byte byte2 = (byte)(value >> 8);
            stream.WriteByte(byte1);
            stream.WriteByte(byte2);
        }
    }
}