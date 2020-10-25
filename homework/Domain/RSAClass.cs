namespace Domain
{
    public class RSAClass
    {
        public int Id { get; set; } // PK
        
        public ulong PrimeP { get; set; }
        public ulong PrimeQ { get; set; }
        
        public string BaseText { get; set; }
        
        public byte[] EncryptedText { get; set; }
        
    }
}