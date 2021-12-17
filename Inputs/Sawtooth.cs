namespace SoundModem
{
    public class Sawtooth : IInput
    {
        long sampleNumber;
        long sampleCycle;
        public Sawtooth(long sampleRate, long frequency)
        {
            sampleCycle = sampleRate / frequency;
        }

        public int GetInput(double[] samples)
        {
            for (int i = 0; i < samples.Length; i++)
            {
                samples[i] = -1f + 2 * (sampleNumber / (double)sampleCycle);
                sampleNumber++;
                if (sampleCycle == sampleNumber)
                {
                    sampleNumber = 0;
                }
            }
            return samples.Length;
        }
    }
}