using System.Collections.Generic;
using System.IO;

namespace SoundModem
{
    public class PSKAlphabet
    {
        public Stream inputData;
        public Dictionary<char, int> chars = new Dictionary<char, int>();
        public PSKAlphabet(Stream inputData)
        {
            this.inputData = inputData;
            chars[(char)0] = 0b1010101011;
            chars[(char)1] = 0b1011011011;
            chars[(char)2] = 0b1011101101;
            chars[(char)3] = 0b1101110111;
            chars[(char)4] = 0b1011101011;
            chars[(char)5] = 0b1101011111;
            chars[(char)6] = 0b1011101111;
            chars[(char)7] = 0b1011111101;
            chars[(char)8] = 0b1011111111;
            chars[(char)9] = 0b11101111;
            chars[(char)10] = 0b11101;
            chars[(char)11] = 0b1101101111;
            chars[(char)12] = 0b1011011101;
            chars[(char)13] = 0b11111;
            chars[(char)14] = 0b1101110101;
            chars[(char)15] = 0b1110101011;
            chars[(char)16] = 0b1011110111;
            chars[(char)17] = 0b1011110101;
            chars[(char)18] = 0b1110101101;
            chars[(char)19] = 0b1110101111;
            chars[(char)20] = 0b1101011011;
            chars[(char)21] = 0b1101101011;
            chars[(char)22] = 0b1101101101;
            chars[(char)23] = 0b1101010111;
            chars[(char)24] = 0b1101111011;
            chars[(char)25] = 0b1101111101;
            chars[(char)26] = 0b1110110111;
            chars[(char)27] = 0b1101010101;
            chars[(char)28] = 0b1101011101;
            chars[(char)29] = 0b1110111011;
            chars[(char)30] = 0b1011111011;
            chars[(char)31] = 0b1101111111;
            chars[' '] = 0b1;
            chars['!'] = 0b111111111;
            chars['"'] = 0b101011111;
            chars['#'] = 0b111110101;
            chars['$'] = 0b111011011;
            chars['%'] = 0b1011010101;
            chars['&'] = 0b1010111011;
            chars['\''] = 0b101111111;
            chars['('] = 0b11111011;
            chars[')'] = 0b11110111;
            chars['*'] = 0b101101111;
            chars['+'] = 0b111011111;
            chars[','] = 0b1110101;
            chars['-'] = 0b110101;
            chars['.'] = 0b1010111;
            chars['/'] = 0b110101111;
            chars['0'] = 0b10110111;
            chars['1'] = 0b10111101;
            chars['2'] = 0b11101101;
            chars['3'] = 0b11111111;
            chars['4'] = 0b101110111;
            chars['5'] = 0b101011011;
            chars['6'] = 0b101101011;
            chars['7'] = 0b110101101;
            chars['8'] = 0b110101011;
            chars['9'] = 0b110110111;
            chars[':'] = 0b11110101;
            chars[';'] = 0b110111101;
            chars['<'] = 0b111101101;
            chars['='] = 0b1010101;
            chars['>'] = 0b111010111;
            chars['?'] = 0b1010101111;
            chars['@'] = 0b1010111101;
            chars['A'] = 0b1111101;
            chars['B'] = 0b11101011;
            chars['C'] = 0b10101101;
            chars['D'] = 0b10110101;
            chars['E'] = 0b1110111;
            chars['F'] = 0b11011011;
            chars['G'] = 0b11111101;
            chars['H'] = 0b101010101;
            chars['I'] = 0b1111111;
            chars['J'] = 0b111111101;
            chars['K'] = 0b101111101;
            chars['L'] = 0b11010111;
            chars['M'] = 0b10111011;
            chars['N'] = 0b11011101;
            chars['O'] = 0b10101011;
            chars['P'] = 0b11010101;
            chars['Q'] = 0b111011101;
            chars['R'] = 0b10101111;
            chars['S'] = 0b1101111;
            chars['T'] = 0b1101101;
            chars['U'] = 0b101010111;
            chars['V'] = 0b110110101;
            chars['W'] = 0b101011101;
            chars['X'] = 0b101110101;
            chars['Y'] = 0b101111011;
            chars['Z'] = 0b1010101101;
            chars['['] = 0b111110111;
            chars['\\'] = 0b111101111;
            chars[']'] = 0b111111011;
            chars['^'] = 0b1010111111;
            chars['_'] = 0b101101101;
            chars['`'] = 0b1011011111;
            chars['a'] = 0b1011;
            chars['b'] = 0b1011111;
            chars['c'] = 0b101111;
            chars['d'] = 0b101101;
            chars['e'] = 0b11;
            chars['f'] = 0b111101;
            chars['g'] = 0b1011011;
            chars['h'] = 0b101011;
            chars['i'] = 0b1101;
            chars['j'] = 0b111101011;
            chars['k'] = 0b10111111;
            chars['l'] = 0b11011;
            chars['m'] = 0b111011;
            chars['n'] = 0b1111;
            chars['o'] = 0b111;
            chars['p'] = 0b111111;
            chars['q'] = 0b110111111;
            chars['r'] = 0b10101;
            chars['s'] = 0b10111;
            chars['t'] = 0b101;
            chars['u'] = 0b110111;
            chars['v'] = 0b1111011;
            chars['w'] = 0b1101011;
            chars['x'] = 0b11011111;
            chars['y'] = 0b1011101;
            chars['z'] = 0b111010101;
            chars['{'] = 0b1010110111;
            chars['|'] = 0b110111011;
            chars['}'] = 0b1010110101;
            chars['~'] = 0b1011010111;
            chars[(char)127] = 0b1110110101;
            FlipAllBits();
        }

        public int? GetData()
        {
            int b = inputData.ReadByte();
            if (b == -1)
            {
                return null;
            }
            return chars[(char)b];
        }

        private void FlipAllBits()
        {
            for (int i = 0; i < 128; i++)
            {
                int orig = chars[(char)i];
                int replace = 0;
                while (orig > 0)
                {
                    replace = replace << 1;
                    int bit = orig & 1;
                    orig = orig >> 1;
                    replace = replace | bit;
                }
                chars[(char)i] = replace;
            }
        }
    }
}