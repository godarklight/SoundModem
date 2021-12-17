using System;
using System.IO;
using System.Text;

namespace SoundModem
{
    public class Wav : ISink
    {
        FileStream fs;
        public Wav()
        {
            fs = new FileStream("output.wav", FileMode.Create);
            byte[] wavHeader = new byte[44];
            Encoding.ASCII.GetBytes("RIFF").CopyTo(wavHeader, 0);
            Encoding.ASCII.GetBytes("WAVE").CopyTo(wavHeader, 8);
            Encoding.ASCII.GetBytes("fmt ").CopyTo(wavHeader, 12);
            BitConverter.GetBytes(16).CopyTo(wavHeader, 16);
            BitConverter.GetBytes((short)1).CopyTo(wavHeader, 20);
            BitConverter.GetBytes((short)1).CopyTo(wavHeader, 22);
            BitConverter.GetBytes(48000).CopyTo(wavHeader, 24);
            BitConverter.GetBytes(96000).CopyTo(wavHeader, 28);
            BitConverter.GetBytes((short)2).CopyTo(wavHeader, 32);
            BitConverter.GetBytes((short)16).CopyTo(wavHeader, 34);
            Encoding.ASCII.GetBytes("data").CopyTo(wavHeader, 36);
            fs.Write(wavHeader, 0, 44);
        }

        public void Write(byte[] sinkData, int sinkLength)
        {
            fs.Write(sinkData, 0, sinkLength);
        }

        public void Close()
        {
            int writePos = (int)fs.Position;
            byte[] totalSize = BitConverter.GetBytes((int)fs.Position);
            byte[] dataSize = BitConverter.GetBytes((int)fs.Position - 44);
            fs.Seek(4, SeekOrigin.Begin);
            fs.Write(totalSize, 0, 4);
            fs.Seek(40, SeekOrigin.Begin);
            fs.Write(totalSize, 0, 4);
            fs.Close();
        }
    }
}