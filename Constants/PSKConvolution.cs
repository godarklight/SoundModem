//Reference http://www.arrl.org/psk31-spec

using System;
using System.Collections.Generic;

namespace SoundModem
{
    public class PSKConvolution
    {
        Dictionary<int, int> rotations = new Dictionary<int, int>();
        int phase = 0;
        int varicode = 0;
        public PSKConvolution()
        {
            rotations[0] = 2;
            rotations[1] = 1;
            rotations[2] = -1;
            rotations[3] = 0;
            rotations[4] = -1;
            rotations[5] = 0;
            rotations[6] = 2;
            rotations[7] = 1;
            rotations[8] = 0;
            rotations[9] = -1;
            rotations[10] = 1;
            rotations[11] = 2;
            rotations[12] = 1;
            rotations[13] = 2;
            rotations[14] = 0;
            rotations[15] = -1;
            rotations[16] = 1;
            rotations[17] = 2;
            rotations[18] = 0;
            rotations[19] = -1;
            rotations[20] = 0;
            rotations[21] = -1;
            rotations[22] = 1;
            rotations[23] = 2;
            rotations[24] = -1;
            rotations[25] = 0;
            rotations[26] = 2;
            rotations[27] = 1;
            rotations[28] = 2;
            rotations[29] = 1;
            rotations[30] = -1;
            rotations[31] = 0;

        }

        public int GetPhase(int newBit)
        {
            varicode = varicode << 1;
            varicode = varicode | newBit;
            varicode = varicode & 0x1F;
            phase = (phase - rotations[varicode]);
            if (phase < 0)
            {
                phase += 4;
            }
            if (phase >= 4)
            {
                phase -= 4;
            }
            if (phase < 0)
            {
                Console.WriteLine("Error");
            }
            if (phase >= 4)
            {
                Console.WriteLine("Error");
            }
            return phase;
        }
    }
}