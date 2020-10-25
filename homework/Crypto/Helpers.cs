using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Crypto
{
    public static class Helpers
    {
        public static ulong GCD(ulong a, ulong b)
        {
            if (a == 0) return b;
            return GCD(b % a, a);
        }

        public static void PrintResults(string stringResult)
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

        public static Tuple<int, string> UserInput()
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

        public static bool PrimalityTest(ulong input, int k = 5)
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

        public static ulong GetUserNumber(string type)
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
        
        public static ulong CheckEven(ulong input)
        {
            if (input == 2) return input;
            return input % 2 == 0 ? input - 1 : input;
        }
    }
}