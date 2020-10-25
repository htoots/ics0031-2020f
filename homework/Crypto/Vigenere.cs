namespace Crypto
{
    public class Vigenere
    {
        public static byte[] VigenereCipher(byte[] input, string key, int choice)
        {
            byte[] result = new byte[input.Length];
            key = key.Trim().ToUpper();

            int keyIndex = 0;
            int keyLength = key.Length;

            for (var i = 0; i < input.Length; i++)
            {
                keyIndex %= keyLength;
                int shift = key[keyIndex] - 65;
                if (choice == 1)
                {
                    result[i] = (byte) ((input[i] + shift) % 256);
                }
                if (choice == 2)
                {
                    result[i] = (byte) ((input[i] + 256 - shift) % 256);
                }
                keyIndex++;
            }
            return result;
        }
    }
}