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

            var bwFile = args[0];

            StreamReader file = File.OpenText(bwFile);

            long i = 0;

            IEnumerable<string> lines = File.ReadLines(bwFile);
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
            Console.Error.WriteLine(string.Format("{0:N0}", i));
            Console.Beep();
            Console.Error.WriteLine("End of program");
            return 0;
        }

        private static byte[] ComputeSHA256(string plainText)
        {
            return ComputeSHA256(Encoding.UTF8.GetBytes(plainText));
        }

        private static byte[] ComputeSHA256(byte[] data)
        {
            return SHA256.Create().ComputeHash(data);
        }
    }
}
