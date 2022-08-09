using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class DiffieHellmanClass
    {
        public int Id { get; set; } // PK
        
        public ulong SecretA { get; set; }
        public ulong SecretB { get; set; }
        public ulong ModulusP { get; set; }
        public ulong BaseG { get; set; }
        
        public ulong Key1 { get; set; }
        
        public ulong Key2 { get; set; }
        
        public string UserId { get; set; }
        public IdentityUser User { get; set; }

    }
}