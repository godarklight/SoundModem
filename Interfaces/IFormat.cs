using System;
using System.IO;

namespace SoundModem
{
    public interface IFormat
    {
        /// <summary>
        /// Reads data from a stream in S16LE format
        /// </summary>
        /// <returns>The sample read, or null if we have no more data to read</returns>
        double? ReadInput();

        /// <summary>
        /// Writes data to a stream in S16LE format
        /// </summary>
        /// <param name="samples">The sample data to write</param>
        /// <param name="samplesLength">The number of samples to write</param>
        /// <returns>The number of samples written</returns>
        void WriteOutput(double[] samples, int samplesLength);
        
        /// <summary>
        /// Writes data to a stream in S16LE format
        /// </summary>
        /// <param name="sample">The sample to write</param>
        /// <returns>The number of samples written</returns>
        void WriteOutput(double sample);

    }
}