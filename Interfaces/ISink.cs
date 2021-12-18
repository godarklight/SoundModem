using System;
using System.IO;

namespace SoundModem
{
    public interface ISink
    {
        /// <summary>
        /// Write data to the output
        /// </summary>
        /// <param name="inData">Input data</param>
        void Write(Stream inData);
        void Close();
    }
}