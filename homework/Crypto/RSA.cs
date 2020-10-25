using System;
using System.Text;

namespace Crypto
{
    
    public static class RSA
    {
        public static byte[] RsaEncryptString(string input, ulong PrimeP, ulong PrimeQ)
        {
            var inputBytes = Helpers.GetBytes(input);
            ulong n = PrimeP * PrimeQ;
            ulong m = (PrimeP - 1) * (PrimeQ - 1);
            Tuple<ulong, ulong> rsaValues = RsaCalculations(m);
            ulong e = rsaValues.Item1;
            return RsaCalculator(inputBytes, e, n);
        }
        // Dirty debug testing
        // public static byte[] RsaDecrypt(byte [] inputBytes, ulong PrimeP, ulong PrimeQ)
        // {
        //     ulong n = PrimeP * PrimeQ;
        //     ulong m = (PrimeP - 1) * (PrimeQ - 1);
        //     Tuple<ulong, ulong> RsaValues = RsaCalculations(m);
        //     ulong d = RsaValues.Item2;
        //     return RsaCalculator(inputBytes, d, n);
        // }

        private static byte[] RsaCalculator(byte[] inputBytes, ulong expo, ulong mod)
        {
            var result = new byte[inputBytes.Length];
            for (var i = 0; i < inputBytes.Length; i++)
            {
                var newValue = (UlongPow(inputBytes[i], expo, mod));
                    if (newValue > byte.MaxValue) newValue -= byte.MaxValue;
                    result[i] = (byte) newValue;
            }

            return result;
        }
        public static Tuple<ulong, ulong> RsaCalculations(ulong m)
        {
            ulong e;
            for (e = 2; e < ulong.MaxValue; e++)
            {
                if (Helpers.GCD(m, e) == 1) break;
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
        
        public static ulong UlongPow(ulong baseNum, ulong exponent, ulong modulus)
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

        public static ulong GetP(ulong p, ulong n)
        {
            while (n % p != 0)
            {
                p--;
                // Failsafe, should never happen
                if (p < 1)
                {
                    Console.WriteLine("Could not find p, exiting");
                    throw new ArithmeticException("Unable to calculate prime p");
                }
            }

            return p;
        }
    }
}