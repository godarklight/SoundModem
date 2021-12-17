namespace SoundModem
{
    public interface IOutput
    {
        /// <summary>
        /// Translate the input data to the output.
        /// </summary>
        /// <param name="samples">Input samples</param>
        /// /// <param name="samplesLength">Number of samples to transcode</param>
        /// <param name="buffer">Buffer to write to</param>
        /// <returns>The number of bytes written to the buffer</returns>
        int GetOutput(double[] samples, int samplesLength, byte[] buffer);
    }
}