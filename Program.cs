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

            long lineCount = 0;
            using (var reader = File.OpenText(args[0]))
            {
                while (reader.ReadLine() != null)
                {
                    lineCount++;
                }
            }

            long i = 0;
            decimal p;

            IEnumerable<string> lines = File.ReadLines(args[0]);

            foreach (string line in lines)
            {
                if (i % 1000 == 0)
                {
                    p = (decimal)i / (decimal)lineCount * (decimal)100;
                    Console.Error.Write(string.Format("\r{0:N0}/{1:N0} ({2:N2}%)", i, lineCount, p));
                }

                Key keyU = new Key(ComputeSHA256(line), fCompressedIn: false);
                Console.WriteLine(keyU.GetAddress(ScriptPubKeyType.Legacy, Network.Main) + " # " + keyU.GetWif(Network.Main) + " # " + line);

                i++;
            }

            p = (decimal)i / (decimal)lineCount * (decimal)100;
            Console.Error.WriteLine(string.Format("\r{0:N0}/{1:N0} ({2:N2}%)", i, lineCount, p));
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
