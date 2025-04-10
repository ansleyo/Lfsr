
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

                    Console.WriteLine($"The Keystream: {generatedKeystream}");

                    File.WriteAllText("keystream", generatedKeystream);

                    break; 

                case "encrypt":

                    Crypt encryptCrypt = new Crypt();

                    string plainText = args[1];

                    string keystreamFromFile = File.ReadAllText("keystream");

                    string ciphertext = encryptCrypt.Encrypt(plainText, keystreamFromFile);

                    Console.WriteLine($"The ciphertext is: {ciphertext}");

                    break;

                case "decrypt":

                    Crypt decryptCrypt = new Crypt();

                    string ciphertextToDecrypt = args[1];

                    string keystreamToDecrypt = File.ReadAllText("keystream");

                    string decryptedText = decryptCrypt.Decrypt(ciphertextToDecrypt, keystreamToDecrypt);

                    Console.WriteLine($"The plaintext is: {decryptedText}");

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

        public class Crypt{

            public string Encrypt(string plaintext, string keystreamFromFile)
            {
                int maxLength = Math.Max(plaintext.Length, keystreamFromFile.Length);
                plaintext= plaintext.PadLeft(maxLength, '0');
                keystreamFromFile = keystreamFromFile.PadLeft(maxLength, '0');

                string ciphertext = string.Empty;

                for (int i = 0; i < plaintext.Length; i++)
                {
                    char p = plaintext[i];
                    char k = keystreamFromFile[i];
                    ciphertext += (p == k) ? '0' : '1';
                }

                return ciphertext;
            }

            public string Decrypt(string ciphertext, string keystream)
            {
                int maxLengthDecrypt = Math.Max(ciphertext.Length, keystream.Length);
                ciphertext = ciphertext.PadLeft(maxLengthDecrypt, '0');
                keystream = keystream.PadLeft(maxLengthDecrypt, '0');

                string decryptedText = string.Empty;

                for (int i = 0; i < ciphertext.Length; i++)
                {
                    char c = ciphertext[i];
                    char k = keystream[i];
                    decryptedText += (c == k) ? '0' : '1';
                }

                return decryptedText;

        }
    }
    }
}