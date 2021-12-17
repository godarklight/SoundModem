using System.Collections.Generic;
using System.IO;

namespace SoundModem
{
    public class Baudot
    {
        public Stream inputData;
        public Dictionary<char, byte[]> chars = new Dictionary<char, byte[]>();
        public Dictionary<char, byte[]> charsDigit = new Dictionary<char, byte[]>();
        private byte[] switchToDigit = new byte[] { 1, 1, 0, 1, 1 };
        private byte[] switchToLetter = new byte[] { 1, 1, 1, 1, 1 };
        bool wasDigit = false;
        byte[] overrideNext;
        public Baudot(Stream inputData)
        {
            this.inputData = inputData;
            chars.Add((char)0, new byte[] { 0, 0, 0, 0, 0 });
            charsDigit[(char)0] = chars[(char)0];
            chars.Add(' ', new byte[] { 0, 0, 1, 0, 0 });
            charsDigit[' '] = chars[' '];
            chars.Add('Q', new byte[] { 1, 1, 1, 0, 1 });
            charsDigit['1'] = chars['Q'];
            chars.Add('W', new byte[] { 1, 1, 0, 0, 1 });
            charsDigit['2'] = chars['W'];
            chars.Add('E', new byte[] { 1, 0, 0, 0, 0 });
            charsDigit['3'] = chars['E'];
            chars.Add('R', new byte[] { 0, 1, 0, 1, 0 });
            charsDigit['4'] = chars['R'];
            chars.Add('T', new byte[] { 0, 0, 0, 0, 1 });
            charsDigit['5'] = chars['T'];
            chars.Add('Y', new byte[] { 1, 0, 1, 0, 1 });
            charsDigit['6'] = chars['Y'];
            chars.Add('U', new byte[] { 1, 1, 1, 0, 0 });
            charsDigit['7'] = chars['U'];
            chars.Add('I', new byte[] { 0, 1, 1, 0, 0 });
            charsDigit['8'] = chars['I'];
            chars.Add('O', new byte[] { 0, 0, 0, 1, 1 });
            charsDigit['9'] = chars['O'];
            chars.Add('P', new byte[] { 0, 1, 1, 0, 1 });
            charsDigit['0'] = chars['P'];
            chars.Add('A', new byte[] { 1, 1, 0, 0, 0 });
            charsDigit['-'] = chars['A'];
            chars.Add('S', new byte[] { 1, 0, 1, 0, 0 });
            charsDigit[(char)7] = chars['S'];
            chars.Add('D', new byte[] { 1, 0, 0, 1, 0 });
            charsDigit['$'] = chars['D'];
            chars.Add('F', new byte[] { 1, 0, 1, 1, 0 });
            charsDigit['!'] = chars['F'];
            chars.Add('G', new byte[] { 0, 1, 0, 1, 1 });
            charsDigit['&'] = chars['G'];
            chars.Add('H', new byte[] { 0, 0, 1, 0, 1 });
            charsDigit['#'] = chars['H'];
            chars.Add('J', new byte[] { 1, 1, 0, 1, 0 });
            charsDigit['\''] = chars['J'];
            chars.Add('K', new byte[] { 1, 1, 1, 1, 0 });
            charsDigit['('] = chars['K'];
            chars.Add('L', new byte[] { 0, 1, 0, 0, 1 });
            charsDigit[')'] = chars['L'];
            chars.Add('Z', new byte[] { 1, 0, 0, 0, 1 });
            charsDigit['"'] = chars['Z'];
            chars.Add('X', new byte[] { 1, 0, 1, 1, 1 });
            charsDigit['/'] = chars['X'];
            chars.Add('C', new byte[] { 0, 1, 1, 1, 0 });
            charsDigit[':'] = chars['C'];
            chars.Add('V', new byte[] { 0, 1, 1, 1, 1 });
            charsDigit[';'] = chars['V'];
            chars.Add('B', new byte[] { 1, 0, 0, 1, 1 });
            charsDigit['?'] = chars['B'];
            chars.Add('N', new byte[] { 0, 0, 1, 1, 0 });
            charsDigit[','] = chars['N'];
            chars.Add('M', new byte[] { 0, 0, 1, 1, 1 });
            charsDigit['.'] = chars['M'];
            chars.Add((char)10, new byte[] { 0, 0, 0, 1, 0 });
            chars.Add((char)13, new byte[] { 0, 1, 0, 0, 0 });
        }

        public byte[] GetData()
        {
            if (overrideNext != null)
            {
                byte[] returnVal = overrideNext;
                overrideNext = null;
                return returnVal;
            }
            int readByte = inputData.ReadByte();
            if (readByte == -1)
            {
                return null;
            }
            char c = (char)readByte;
            if (char.IsLetter(c))
            {
                c = char.ToUpper(c);
            }

            if (chars.ContainsKey(c))
            {
                if (wasDigit)
                {
                    overrideNext = chars[c];
                    return switchToLetter;
                }
                return chars[c];
            }
            if (charsDigit.ContainsKey(c))
            {
                if (!wasDigit)
                {
                    overrideNext = charsDigit[c];
                    return switchToDigit;
                }
                return charsDigit[c];
            }
            return null;
        }
    }
}