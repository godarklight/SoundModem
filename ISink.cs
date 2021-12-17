namespace SoundModem
{
    public interface ISink
    {
        /// <summary>
        /// Translate the input data to the output.
        /// </summary>
        /// <param name="samples">Input samples</param>
        /// /// <param name="samplesLength">Number of samples to transcode</param>
        /// <param name="buffer">Buffer to write to</param>
        /// <returns>The number of bytes written to the buffer</returns>
        void Write(byte[] buffer, int length);
        void Close();
    }
}