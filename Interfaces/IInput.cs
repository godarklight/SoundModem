namespace SoundModem
{
    public interface IInput
    {
        /// <summary>
        /// Write the input data to the formatted output
        /// </summary>
        /// <param name="output">The formatted stream to write to</param>
        bool GetInput(IFormat output);
    }
}