
using System;

namespace Lfsr
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: dotnet run <option> <other arguments>");
                return;
            }

            string option = args[0].ToLower();

            switch (option)
            {
                case "cipher":
                    ///add error checking !!!!!
                    string seed = args[1];
                    int tap = int.Parse(args[2]);

                    Console.WriteLine($"{seed} – seed");

                    Cipher cipher = new Cipher(seed, tap);
                    char outputBit = cipher.Step();

                    Console.WriteLine($"{cipher.Seed} {outputBit}");

                    break;

                case "generatekeystream":
                    /// another add error checking reminder
                    string keystreamSeed = args[1];
                    int keystreamTap = int.Parse(args[2]);
                    int length = int.Parse(args[3]);

                    Console.WriteLine($"{keystreamSeed} – seed");

                    Keystream keystream = new Keystream(keystreamSeed, keystreamTap, length);
                    string generatedKeystream = keystream.Generate();

                    Console.WriteLine($"{generatedKeystream} – keystream");

                    break;

                case "encrypt":
                    break;
                case "decrypt":
                    break;
                case "triplebits":
                    break;
                case "encryptimage":
                    break;
                case "decryptimage":
                    break;
                default:
                    Console.WriteLine($"Unknown option: {option}");
                    Console.WriteLine("Valid options are: Cipher, GenerateKeystream, Encrypt, Decrypt, TripleBits, EncryptImage, DecryptImage");
                    break;
            }
        }

        public class Cipher
        {
            public string Seed { get; set; }
            public int Tap { get; set; }

            public Cipher(string seed, int tap)
            {
                Seed = seed;
                Tap = tap;
            }

            public char Step()
            {
                char leftMostBit = Seed[0];
                char tapBit = Seed[Tap];
                char newBit = (leftMostBit == tapBit) ? '0' : '1'; 

                Seed = Seed.Substring(1) + newBit;

                return newBit;
            }
        }

        public class Keystream{
            public string Seed { get; set; }
            public int Tap { get; set; }
            public int Length { get; set; }

            public Keystream(string seed, int tap, int length)
            {
                Seed = seed;
                Tap = tap;
                Length = length;
            }

            public string Generate()
            {
                string keystream = string.Empty;
                Cipher cipher = new Cipher(Seed, Tap);

                for (int i = 0; i < Length; i++)
                {
                    char bit = cipher.Step();
                    keystream += bit;
                    Console.WriteLine($"{cipher.Seed} {bit}");
                }

                return keystream;
            }
        }
    }
}