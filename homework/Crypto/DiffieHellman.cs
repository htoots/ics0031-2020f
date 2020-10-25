using System;
using System.Collections.Generic;

namespace Crypto
{
    public static class DiffieHellman
    {
        public static ulong DiffiePow(ulong a, ulong b, ulong c)
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
        public static List<ulong> DiffieHellmanCalc(ulong secret1, ulong secret2, ulong modulusp, ulong baseg)
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
    }
}