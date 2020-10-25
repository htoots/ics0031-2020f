using System;

namespace Crypto
{
    
    public static class RSA
    {
        public static Tuple<ulong, ulong> RSACalculations(ulong m)
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