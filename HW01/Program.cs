using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ConsoleApp01
{
    class Program
    {
        static void Main(string[] args)
        {
            var userInput = "";
            do
            {
                Console.WriteLine();
                Console.WriteLine("1) Caesar cipher");
                Console.WriteLine("2) Vigenere cipher");
                Console.WriteLine("X) Exit");
                Console.Write(">");

                userInput = Console.ReadLine()?.ToLower();

                switch (userInput)
                {
                    case "1":
                        Caesar();
                        break;
                    case "2":
                        Vigenere();
                        break;
                    case "x":
                        Console.WriteLine("closing down...");
                        break;
                    default:
                        Console.WriteLine($"Don't have this '{userInput}' as an option!");
                        break;
                }
            } while (userInput != "x");
        }
        static void Caesar()
        {
            Console.WriteLine("Caesar Cipher");
            // byte per character
            // 0-255
            // 0-127 - latin
            // 128-255 - change what you want
            // ABCD - A 189, B - 195, C 196, D 202
            // unicode 
            // AÄÖÜLA❌
            var key = 0;
            Tuple<int, string> userInput;
            userInput = UserInput();
            if (userInput == null)
            {
                return;
            }
            do
            {
                Console.Write("Please enter your shift amount:");
                var keyIn = Console.ReadLine()?.ToLower().Trim();
                if (int.TryParse(keyIn, out var keyValue))
                {
                    key = keyValue % 255;
                    if (key == 0)
                    {
                        Console.WriteLine("multiples of 255 is no cipher, this would not do anything!");
                    }
                    else
                    {
                        Console.WriteLine($"Cesar key is: {key}");
                    }
                }
            } while (key == 0);
            var result = CaesarEncryptString(userInput.Item2, (byte) key, userInput.Item1);
            
            PrintResults(Encoding.Default.GetString(result));
        }
        static byte[] CaesarEncryptString(string input, byte shiftAmount, int choice)
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

        static void Vigenere()
        {
            Tuple<int, string> userInput;
            userInput = UserInput();
            if (userInput == null)
            {
                return;
            }
            int choice = userInput.Item1;
            byte[] inputBytes = Encoding.Default.GetBytes(userInput.Item2);
            
            Console.WriteLine("Write key to use for Vigenere.");
            var key = Console.ReadLine();
            
            byte[] result = VigenereCipher(inputBytes, key, choice);

            PrintResults(Encoding.Default.GetString(result));
        }

        static byte[] VigenereCipher(byte[] input, string key, int choice)
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
        static Tuple<int, string> UserInput()
        {
            var choice = 0;
            var inputText = "";
            bool? isBase64 = null;
            bool base64Confirmed = false;
            do
            {
                Console.WriteLine("Do you want to (E)ncrypt or (D)ecrypt?");
                var userInput = Console.ReadLine()?.Trim().ToLower();
                switch (userInput)
                {
                    case "e":
                        choice = 1;
                        break;
                    case "d":
                        choice = 2;
                        break;
                    default:
                        Console.WriteLine("Input has to be \"e\" or \"d\"");
                        break;
                }
            } while (choice == 0);

            do
            {
                Console.WriteLine("Is input base64? y/n");
                var userInput = Console.ReadLine()?.Trim().ToLower();
                switch (userInput)
                {
                    case "y":
                        isBase64 = true;
                        break;
                    case "n":
                        isBase64 = false;
                        break;
                    default:
                        Console.WriteLine("Invalid answer, must be \"y\" or \"n\"");
                        break;
                }
            } while (isBase64 == null);

            do
            {
                Console.WriteLine("Write your input:");
                inputText = Console.ReadLine();
                if (inputText != null)
                {
                    if (isBase64 == true)
                    {
                        inputText = inputText.Trim();
                        if ((inputText.Length % 4 == 0) &&
                            Regex.IsMatch(inputText, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None))
                        {
                            inputText = Base64Decode(inputText);
                            base64Confirmed = true;
                        }
                        else
                        {
                            Console.WriteLine("Invalid base64, will return to main.");
                            return null;
                        }
                    }
                }
            } while (string.IsNullOrEmpty(inputText) && base64Confirmed != true);

            return new Tuple<int, string>(choice, inputText);
        }

        static void PrintResults(string stringResult)
        {
            Console.WriteLine("String result:");
            Console.WriteLine(stringResult);
            Console.WriteLine("Base64 result:");
            Console.WriteLine(Base64Encode(stringResult));
        }
        static string Base64Encode(string plainText) {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }
        
        static string Base64Decode(string base64EncodedData) {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}