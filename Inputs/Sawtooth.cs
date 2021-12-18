namespace SoundModem
{
    public class Sawtooth : IInput
    {
        double lastPos;
        double cycleShift;
        double timeLeft;
        double sampleRate;
        public Sawtooth(double sampleRate, double frequency, double seconds)
        {
            this.sampleRate = sampleRate;
            this.timeLeft = seconds;
            cycleShift = frequency / sampleRate;
        }

        public bool GetInput(IFormat output)
        {
            for (int i = 0; i < 128; i++)
            {
                lastPos += cycleShift;
                if (lastPos > 0.5)
                {
                    lastPos -= 1;
                }
                timeLeft -= 1 / sampleRate;
                if (timeLeft < 0)
                {
                    return i > 0;
                }
                output.WriteOutput(lastPos);
            }
            return true;
        }
    }
}