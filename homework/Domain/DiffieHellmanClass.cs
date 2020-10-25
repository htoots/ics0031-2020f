using System.Collections.Generic;

namespace Domain
{
    public class DiffieHellmanClass
    {
        public int Id { get; set; } // PK
        
        public ulong SecretA { get; set; }
        public ulong SecretB { get; set; }
        public ulong ModulusP { get; set; }
        public ulong BaseG { get; set; }
        
        public List<ulong> Keys { get; set; }

    }
}