using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime;
using System.Security.Cryptography;
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
                Console.WriteLine("3) Diffie Hellman key exchange");
                Console.WriteLine("4) RSA");
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
                    case "3":
                        DiffieHellman();
                        break;
                    case "4":
                        CustomRSA();
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

        static bool PrimalityTest(ulong input, int k = 5)
        {
            // Miller-Rabin primality test implementation
            // int k = depth of likeliness
            if ((input < 2) || (input % 2 == 0)) return (input == 2);

            ulong s = input - 1;
            while (s % 2 == 0) s >>= 1;

            Random r = new Random();

            for (int i = 0; i < k; i++)
            {
                int a = r.Next((int) (input - 1)) + 1;
                ulong temp = s;
                long mod = 1;
                for (ulong j = 0; j < temp; ++j) mod = (mod * a) % (long) input;
                while (temp != input - 1 && mod != 1 && mod != (long) (input - 1))
                {
                    mod = (mod * mod) % (long) input;
                    temp *= 2;
                }

                if (mod != (long) (input - 1) && temp % 2 == 0) return false;
            }

            return true;
        }

        static void DiffieHellman()
        {
            bool[] check = new bool[2];
            ulong secretA;
            ulong secretB;
            ulong modp;
            ulong baseg;
            do
            {
                secretA = GetUserNumber("secret A");
                secretB = GetUserNumber("secret B");
                modp = GetUserNumber("modulus p (has to be prime)");
                baseg = GetUserNumber("public base g (has to be prime)");

                check[0] = PrimalityTest(modp);
                check[1] = PrimalityTest(baseg);
                if (check[0] == false || check[1] == false)
                {
                    Console.WriteLine();
                    Console.WriteLine("Primality tests:");
                    Console.WriteLine("Modulus p: " + (check[0] ? "True" : "False"));
                    Console.WriteLine("Public base g: " + (check[1] ? "True" : "False"));
                    Console.WriteLine("At least one of these failed, please redo the inputs.");
                }
            } while (check[0] == false || check[1] == false);

            List<ulong> results = DiffieHellmanCalc(secretA, secretB, modp, baseg);
            Console.WriteLine();
            Console.WriteLine("Keys for both parties (debug and confirmation):");
            foreach (var key in results)
            {
                Console.WriteLine("Key: " + key);
            }
        }

        static ulong GetUserNumber(string type)
        {
            do
            {
                Console.WriteLine("Input " + type + ": ");
                var userInput = Console.ReadLine();
                if (ulong.TryParse(userInput, out ulong nr))
                {
                    return nr;
                }
                Console.WriteLine("Unable to parse input as ulong. Try again.");
            } while (true);
        }

        static ulong DiffiePow(ulong a, ulong b, ulong c)
        {
            checked
            {
                if (a == 0) return 0;
                if (b == 0) return 1;

                ulong y;
                if (b % 2 == 0)
                {
                    y = DiffiePow(a, b / 2, c);
                    y = (y * y) % c;
                }
                else
                {
                    y = a % c;
                    y = (y * DiffiePow(a, b - 1, c) % c) % c;
                }
                return (y + c) % c;
            }
        }
        static List<ulong> DiffieHellmanCalc(ulong secret1, ulong secret2, ulong modulusp, ulong baseg)
        {
            ulong s1 = DiffiePow(baseg, secret1, modulusp);
            ulong s2 = DiffiePow(baseg, secret2, modulusp);
            
            ulong k1 = DiffiePow(s2, secret1, modulusp);
            ulong k2 = DiffiePow(s1, secret2, modulusp);
            
            List<ulong> result = new List<ulong>();
            result.Add(k1);
            result.Add(k2);
            return result;
        }

        static void CustomRSA()
        {
            Console.WriteLine("RSA");
            // try parse
            ulong p;
            ulong q;
            bool[] check = new bool[2];
            do
            {
                p = GetUserNumber("p (has to be prime)");
                q = GetUserNumber("q (has to be prime)");

                check[0] = PrimalityTest(p);
                check[1] = PrimalityTest(q);
                if (check[0] == false || check[1] == false)
                {
                    Console.WriteLine();
                    Console.WriteLine("Primality tests:");
                    Console.WriteLine("p: " + (check[0] ? "True" : "False"));
                    Console.WriteLine("q: " + (check[1] ? "True" : "False"));
                    Console.WriteLine("At least one of these failed, please redo the inputs.");
                }
            } while (check[0] == false || check[1] == false);

            Console.WriteLine($"p: {p} q: {q}");

            var n = p * q;
            var m = (p - 1) * (q - 1);
            
            Console.WriteLine($"n = p * q : {n}");
            Console.WriteLine($"m = (p - 1) * (q - 1) : {m}");

            Tuple<ulong, ulong> RSAValues = RSACalculations(m);
            ulong e = RSAValues.Item1;
            ulong d = RSAValues.Item2;

            Console.WriteLine($"Public key ({n}, {e})");
            Console.WriteLine($"Private key ({n}, {d})");
            
            ulong message = GetUserNumber("Message (number to encrypt)");

            var cipher = UlongPow(message, e, n);
            Console.WriteLine($"Cipher: {cipher}");

            var plainMsg = UlongPow(cipher, d, n);
            Console.WriteLine($"Plain msg: {plainMsg}");

            BruteForceRSA(n, cipher);
        }

        // Brute force break RSA with public key and cipher message
        static void BruteForceRSA(ulong n, ulong cipher)
        {
            // Tested with n = 133, message 6 (like on slides)
            Console.WriteLine($"\n\nBrute force RSA with public {n} and ciphered text {cipher} from RSA function");
            ulong p = Convert.ToUInt64(Math.Floor(Math.Sqrt(n)));
            if (p % 2 == 0) p -= 1;
            Console.WriteLine($"Testing possible value of p from {p} down to 0");
            do
            {
                p--;
                // Failsafe
                if (p < 1)
                {
                    Console.WriteLine("Could not find p, exiting");
                    return;
                }
            } while (n % p != 0);

            Console.WriteLine($"Found possible p: {p}");
            ulong q = n / p;
            Console.WriteLine($"Found possible q : {q}");

            ulong check = p * q;
            Console.WriteLine($"!!! p * q should be '{n}', is '{check}'");

            if (check != n)
            {
                Console.WriteLine("Cracking failed, p * q does not equal n, exiting...");
                return;
            }
            Console.WriteLine($"p: {p}, and q: {q}. Calculating (p-1)*(q-1)");
            ulong m = (p - 1) * (q - 1);
            Console.WriteLine($"m = {m}");

            Tuple<ulong, ulong> RSAValues = RSACalculations(m);
            ulong e = RSAValues.Item1;
            ulong d = RSAValues.Item2;

            Console.WriteLine($"e = {e}, d = {d}");
            Console.WriteLine($"Cracking ciphered message: {cipher}");
            var plainMsg = UlongPow(cipher, d, n);
            Console.WriteLine($"Possible plain message: {plainMsg}");

        }

        public static Tuple<ulong, ulong> RSACalculations(ulong m)
        {
            ulong e;
            for (e = 2; e < ulong.MaxValue; e++)
            {
                if (GCD(m, e) == 1) break;
            }

            ulong d = 0;
            for (ulong k = 2; k < ulong.MaxValue; k++)
            {
                if ((1 + k * m) % e == 0)
                {
                    d = (1 + k * m) / e;
                    break;
                }
            }
            return new Tuple<ulong, ulong>(e, d);
        }

        private static ulong UlongPow(ulong baseNum, ulong exponent, ulong modulus)
        {
            if (modulus == 1) return 0;
            var curPow = baseNum % modulus;
            ulong res = 1;
            while (exponent > 0)
            {
                if (exponent % 2 == 1) res = (res * curPow) % modulus;
                exponent = exponent / 2;
                curPow = (curPow * curPow) % modulus;
            }
            return res;
        }

        static ulong GCD(ulong a, ulong b)
        {
            if (a == 0) return b;
            return GCD(b % a, a);
        }
    }
}