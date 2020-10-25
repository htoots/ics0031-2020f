using System;
using System.Collections.Generic;
using System.Text;
using Crypto;

namespace consoleApp
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
                        CaesarVoid();
                        break;
                    case "2":
                        VigenereVoid();
                        break;
                    case "3":
                        DiffieHellmanVoid();
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
        static void CaesarVoid()
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
            userInput = Helpers.UserInput();
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
            var result = Caesar.CaesarEncryptString(userInput.Item2, (byte) key, userInput.Item1);
            
            Helpers.PrintResults(Encoding.Default.GetString(result));
        }
        
        static void VigenereVoid()
        {
            Tuple<int, string> userInput;
            userInput = Helpers.UserInput();
            if (userInput == null)
            {
                return;
            }
            int choice = userInput.Item1;
            byte[] inputBytes = Encoding.Default.GetBytes(userInput.Item2);
            
            Console.WriteLine("Write key to use for Vigenere.");
            var key = Console.ReadLine();
            
            byte[] result = Vigenere.VigenereCipher(inputBytes, key, choice);

            Helpers.PrintResults(Encoding.Default.GetString(result));
        }
        
        static void DiffieHellmanVoid()
        {
            bool[] check = new bool[2];
            ulong secretA;
            ulong secretB;
            ulong modp;
            ulong baseg;
            do
            {
                secretA = Helpers.GetUserNumber("secret A");
                secretB = Helpers.GetUserNumber("secret B");
                modp = Helpers.GetUserNumber("modulus p (has to be prime)");
                baseg = Helpers.GetUserNumber("public base g (has to be prime)");

                check[0] = Helpers.PrimalityTest(modp);
                check[1] = Helpers.PrimalityTest(baseg);
                if (check[0] == false || check[1] == false)
                {
                    Console.WriteLine();
                    Console.WriteLine("Primality tests:");
                    Console.WriteLine("Modulus p: " + (check[0] ? "True" : "False"));
                    Console.WriteLine("Public base g: " + (check[1] ? "True" : "False"));
                    Console.WriteLine("At least one of these failed, please redo the inputs.");
                }
            } while (check[0] == false || check[1] == false);

            List<ulong> results = DiffieHellman.DiffieHellmanCalc(secretA, secretB, modp, baseg);
            Console.WriteLine();
            Console.WriteLine("Keys for both parties (debug and confirmation):");
            foreach (var key in results)
            {
                Console.WriteLine("Key: " + key);
            }
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
                p = Helpers.GetUserNumber("p (has to be prime)");
                q = Helpers.GetUserNumber("q (has to be prime)");

                check[0] = Helpers.PrimalityTest(p);
                check[1] = Helpers.PrimalityTest(q);
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

            Tuple<ulong, ulong> RSAValues = RSA.RsaCalculations(m);
            ulong e = RSAValues.Item1;
            ulong d = RSAValues.Item2;

            Console.WriteLine($"Public key ({n}, {e})");
            Console.WriteLine($"Private key ({n}, {d})");
            
            ulong message = Helpers.GetUserNumber("Message (number to encrypt)");

            var cipher = RSA.UlongPow(message, e, n);
            Console.WriteLine($"Cipher: {cipher}");

            var plainMsg = RSA.UlongPow(cipher, d, n);
            Console.WriteLine($"Plain msg: {plainMsg}");

            BruteForceRSA(n, cipher);
        }
        
        static void BruteForceRSA(ulong n, ulong cipher)
        {
            // Tested with n = 133, message 6 (like on slides)
            Console.WriteLine($"\n\nBrute force RSA with public '{n}' and ciphered text '{cipher}' from RSA function");
            ulong p = Convert.ToUInt64(Math.Floor(Math.Sqrt(n)));
            p = Helpers.CheckEven(p);
            Console.WriteLine($"Testing possible value of p from {p} down to 0");
            p = RSA.GetP(p, n);

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

            Tuple<ulong, ulong> RSAValues = RSA.RsaCalculations(m);
            ulong e = RSAValues.Item1;
            ulong d = RSAValues.Item2;

            Console.WriteLine($"e = {e}, d = {d}");
            Console.WriteLine($"Cracking ciphered message: {cipher}");
            var plainMsg = RSA.UlongPow(cipher, d, n);
            Console.WriteLine($"Possible plain message: {plainMsg}");

        }
    }
    
}