namespace SoundModem
{
    public interface IFilter
    {
        /// <summary>
        /// Add data to the filter
        /// </summary>
        /// <param name="input">The input sample to add</param>
        void AddSample(double input);

        /// <summary>
        /// Get data from the filter
        /// </summary>
        /// <returns>The filter amplitude</returns>
        double GetSample();
    }
}