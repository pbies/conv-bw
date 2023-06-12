using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using NBitcoin;

namespace conv_bw
{
    internal class Program
    {
        static int Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Specify input file!");
                return 1;
            }

            var PasswordsFile = args[0];

            StreamReader file = File.OpenText(PasswordsFile);

            int i = 0;

            IEnumerable<string> lines = File.ReadLines(PasswordsFile);
            foreach (string line in lines)
            {
                if (i % 1000 == 0)
                {
                    Console.Error.Write($"\r");
                    Console.Error.Write(string.Format("{0:N0}", i));
                }

                Key keyU = new Key(ComputeSHA256(line), fCompressedIn: false);
                Console.WriteLine(keyU.GetAddress(ScriptPubKeyType.Legacy, Network.Main) + " # " + keyU.GetWif(Network.Main) + " # " + line);

                i++;
            }
            Console.Error.Write($"\r");
            Console.Error.Write(string.Format("{0:N0}", i));

            Console.Error.WriteLine("End of program");
            return 0;
        }

        private static byte[] ComputeSHA256(string plainText)
        {
            byte[] textAsBytes = Encoding.UTF8.GetBytes(plainText);

            return ComputeSHA256(textAsBytes);
        }

        private static byte[] ComputeSHA256(byte[] data)
        {
            using (SHA256 sha = SHA256.Create())
            {
                return sha.ComputeHash(data);
            }
        }
    }
}
