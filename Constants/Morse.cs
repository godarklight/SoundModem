using System.Collections.Generic;

namespace SoundModem
{
    public class Morse
    {
        public Dictionary<char, byte[]> chars = new Dictionary<char, byte[]>();
        public Morse()
        {
            chars.Add('A', new byte[] { 0, 1 });
            chars.Add('B', new byte[] { 1, 0, 0, 0 });
            chars.Add('C', new byte[] { 1, 0, 1, 0 });
            chars.Add('D', new byte[] { 1, 0, 0 });
            chars.Add('E', new byte[] { 0 });
            chars.Add('F', new byte[] { 0, 0, 1, 0 });
            chars.Add('G', new byte[] { 1, 1, 0 });
            chars.Add('H', new byte[] { 0, 0, 0, 0 });
            chars.Add('I', new byte[] { 0, 0 });
            chars.Add('J', new byte[] { 0, 1, 1, 1 });
            chars.Add('K', new byte[] { 1, 0, 1 });
            chars.Add('L', new byte[] { 0, 1, 0, 0 });
            chars.Add('M', new byte[] { 1, 1 });
            chars.Add('N', new byte[] { 1, 0 });
            chars.Add('O', new byte[] { 1, 1, 1 });
            chars.Add('P', new byte[] { 0, 1, 1, 0 });
            chars.Add('Q', new byte[] { 1, 1, 0, 1 });
            chars.Add('R', new byte[] { 0, 1, 0 });
            chars.Add('S', new byte[] { 0, 0, 0 });
            chars.Add('T', new byte[] { 1 });
            chars.Add('U', new byte[] { 0, 0, 1 });
            chars.Add('V', new byte[] { 0, 0, 0, 1 });
            chars.Add('W', new byte[] { 0, 1, 1 });
            chars.Add('X', new byte[] { 1, 0, 0, 1 });
            chars.Add('Y', new byte[] { 1, 0, 1, 1 });
            chars.Add('Z', new byte[] { 1, 1, 0, 0 });
            chars.Add('1', new byte[] { 0, 1, 1, 1, 1 });
            chars.Add('2', new byte[] { 0, 0, 1, 1, 1 });
            chars.Add('3', new byte[] { 0, 0, 0, 1, 1 });
            chars.Add('4', new byte[] { 0, 0, 0, 0, 1 });
            chars.Add('5', new byte[] { 0, 0, 0, 0, 0 });
            chars.Add('6', new byte[] { 1, 0, 0, 0, 0 });
            chars.Add('7', new byte[] { 1, 1, 0, 0, 0 });
            chars.Add('8', new byte[] { 1, 1, 1, 0, 0 });
            chars.Add('9', new byte[] { 1, 1, 1, 1, 0 });
            chars.Add('0', new byte[] { 1, 1, 1, 1, 1 });
        }
    }
}