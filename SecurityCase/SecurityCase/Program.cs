using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace SecurityCase
{
    public class RsaEncryption
    {
        private static RSACryptoServiceProvider csp = RSACryptoServiceProvider(2048);

        private static RSACryptoServiceProvider RSACryptoServiceProvider(int v)
        {
            throw new NotImplementedException();
        }

        private RSAParameters privateKey;
        private RSAParameters publicKey;

        public RsaEncryption()
        {
            privateKey = csp.ExportParameters(true);
            publicKey = csp.ExportParameters(false);
        }

        public string GetPublicKey()
        {
            var sw = new StringWriter();
            var xs = new XmlSerializer(typeof(RSAParameters));
            xs.Serialize(sw, publicKey);
            return sw.ToString();
        }

        public string Encrypt(string plainText)
        {
            csp = new RSACryptoServiceProvider();
            csp.ImportParameters(publicKey);
            var data = Encoding.Unicode.GetBytes(plainText);
            var chyper = csp.Encrypt(data, false);
            return Convert.ToBase64String(chyper);
        }

        public string Decrypt(string cypherText)
        {
            var dataBytes = Convert.FromBase64String(cypherText);
            csp.ImportParameters(privateKey);
            var plainText = csp.Decrypt(dataBytes, false);
            return Encoding.Unicode.GetString(plainText);
        }
    }

        class Program
    {
        static void Main(string[] args)
        {
            RsaEncryption rsa = new RsaEncryption();
            string cypher = string.Empty;

            Console.WriteLine($"Public Key : {rsa.GetPublicKey()} \n");

            Console.WriteLine("Enter Your Text To Encrypt");
            var text = Console.ReadLine();
            if(!string.IsNullOrEmpty(text))
            {
                cypher = rsa.Encrypt(text);
                Console.WriteLine($"Encrypted Text : {cypher}");
            }

            Console.WriteLine("Press Any Key To Decrypt Text");
            Console.ReadLine();
            var plainText = rsa.Decrypt(cypher);

            Console.WriteLine($"Decrypted Message : {plainText}");
            Console.ReadLine();
        }
    }
}
