using System.Text;

namespace Crypto
{
    public class Caesar
    {
        public static byte[] CaesarEncryptString(string input, byte shiftAmount, int choice)
        {
            var inputBytes = Encoding.Default.GetBytes(input);
            return CaesarEncrypt(inputBytes, shiftAmount, choice);
        }
        static byte[] CaesarEncrypt(byte[] input, byte shiftAmount, int choice)
        {
            var result = new byte[input.Length];
            if (shiftAmount == 0)
            {
                // no shifting needed, just create deep copy
                for (var i = 0; i < input.Length; i++)
                {
                    result[i] = input[i];
                }
            }
            else
            {
                for (var i = 0; i < input.Length; i++)
                {
                    if (choice == 1)
                    {
                        var newCharValue = (input[i] + shiftAmount);
                        if (newCharValue > byte.MaxValue)
                        {
                            newCharValue -= byte.MaxValue;
                        }
                        result[i] = (byte) newCharValue;
                    }
                    if (choice == 2)
                    {
                        var newCharValue = (input[i] - shiftAmount);
                        if (newCharValue < 0)
                        {
                            newCharValue += byte.MaxValue;
                        }
                        result[i] = (byte) newCharValue;
                    }
                }
            }
            return result;
        }
    }
}