
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
                    int keystreamLength = int.Parse(args[3]);

                    Console.WriteLine($"{keystreamSeed} – seed");

                    Cipher keystreamCipher = new Cipher(keystreamSeed, keystreamTap);

                    string keystream = string.Empty;

                    for (int i = 0; i < keystreamLength; i++)
                    {
                        char bit = keystreamCipher.Step();
                        keystream += bit;
                        Console.WriteLine($"{keystreamCipher.Seed} {bit}");
                    }
                    
                    Console.WriteLine("The Keystream: " + keystream);

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
    }
}