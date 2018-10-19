using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Polibius
{
    class Program
    {
        class Indexes
        {
            public Indexes(int i, int j)
            {
                I = i;
                J = j;
            }
            int I { get; set; }
            int J { get; set; }
        }
        static void Main(string[] args)
        {
            char[,] alphabet = {
                { 'A', 'B','C','D', 'E' },
                { 'F', 'G',   'H',   'I', 'K' },
                { 'L', 'M',   'N',   'O',   'P'},
                {'Q',  'R',   'S',   'T',   'U'},
                {'V',  'W',   'X',   'Y',   'Z'},
            };


            Console.WriteLine("The alphabet is ...");
            PrintArray(alphabet);
            Console.WriteLine();
            Console.WriteLine("Write some key with lenght of {0}!", alphabet.GetLength(0));
            var key = Console.ReadLine();
            Console.WriteLine("Write some message ...");
            var msg = Console.ReadLine();

            Console.WriteLine();
            Console.WriteLine("Your key is ...");
            Console.WriteLine(key);

            var result = Encrypt(key,msg,alphabet);
            Console.WriteLine("Encrypted message ...");
            Console.WriteLine(result);
            Console.WriteLine();
            Console.WriteLine("================================================");
            Console.WriteLine();
            result = Decrypt(key, result, alphabet);
            Console.WriteLine("Decrypted message ...");
            Console.WriteLine(result);
            Console.WriteLine();
            PrintArray(alphabet);
            Console.ReadKey();

        }


        public static string Encrypt(string key, string msg, char[,] alp)
        {
            string encrypted = "";

            key = Regex.Replace(key, @"\s+", string.Empty);

            if (RemoveDuplicates(key).Length != alp.GetLength(0))
            {
                Console.WriteLine("Wrong Key!");
                Console.ReadKey();
                System.Environment.Exit(0);
                return encrypted;
            }

            msg = Regex.Replace(msg, @"\s+", string.Empty);
            Console.WriteLine();
            Console.WriteLine("Your message ...");
            Console.WriteLine(msg);
            Console.WriteLine();
            setKey(alp, key);

            var txt_array = msg.ToUpper().ToCharArray();

            int[] horizontal = new int[msg.Length];
            int[] vertical = new int[msg.Length];

            for (int x = 0; x < txt_array.Length; x++)
            {
                for (int i = 0; i < alp.GetLength(0); i++)
                {
                    for (int j = 0; j < alp.GetLength(1); j++)
                    {
                        if (txt_array[x] == alp[i, j])
                        {
                            horizontal[x] = j;
                            vertical[x] = i;
                        }
                    }
                }
            }



            var indexes = new int[horizontal.Length + vertical.Length];
            horizontal.CopyTo(indexes, 0);
            vertical.CopyTo(indexes, horizontal.Length);

            for (int x = 0; x < indexes.Length-1; x++)
            {
                if (x % 2 == 0)
                {
                    encrypted += string.Format("{0}", alp[indexes[x+1], indexes[x]]);
                }
            }
            return encrypted;
        }


        public static string Decrypt(string key, string msg, char[,] alp)
        {
            string decrypted = "";
            Regex.Replace(key, @"\s+", string.Empty);

            if (RemoveDuplicates(key).Length != alp.GetLength(0))
            {
                Console.WriteLine("Wrong Key!");
                Console.ReadKey();
                System.Environment.Exit(0);
                return decrypted;
            }

            msg = Regex.Replace(msg, @"\s+", string.Empty);
            Console.WriteLine();
            Console.WriteLine("Your message ...");
            Console.WriteLine(msg);
            Console.WriteLine();
            setKey(alp, key);

            var txt_array = msg.ToUpper().ToCharArray();

            int[] horizontal = new int[msg.Length];
            int[] vertical = new int[msg.Length];

            for (int x = 0; x < txt_array.Length; x++)
            {
                for (int i = 0; i < alp.GetLength(0); i++)
                {
                    for (int j = 0; j < alp.GetLength(1); j++)
                    {
                        if (txt_array[x] == alp[i, j])
                        {
                            horizontal[x] = j;
                            vertical[x] = i;
                        }
                    }
                }
            }

            var indexes = new int[horizontal.Length + vertical.Length];
            horizontal.CopyTo(indexes, 0);
            vertical.CopyTo(indexes, horizontal.Length);

            var z = 0;
            for (int i = 0; i < vertical.Length; i++)
            {
                indexes[z++] = horizontal[i];
                indexes[z++] = vertical[i];
            }

            var half = indexes.Length / 2;

            Array.Copy(indexes, 0, horizontal, 0,half);
            Array.Copy(indexes, half, vertical, 0,half);

            for (int i = 0; i < horizontal.Length; i++)
            {
                decrypted += alp[vertical[i], horizontal[i]];
            }
            return decrypted;
        }


        public static char[,] setKey(char[,] alph, string key)
        {
            List<char> letters = AlphabetToList(alph);
            var key_chars = key.ToUpper().ToCharArray();

            for (int i = 0; i < key_chars.Length; i++)
            {
                alph[0, i] = key_chars[i];
                letters.Remove(key_chars[i]);
            }

            var r = 1;
            var j = 0;
            for (int x = 0; x < letters.Count; x++)
            {
                if (j > alph.GetLength(1)-1) { j = 0; r += 1; }
                alph[r, j] = letters[x];
                j++;

            }

            return alph;
        }

        public static string RemoveDuplicates(string input)
        {
            return new string(input.ToCharArray().Distinct().ToArray());
        }

        public  static List<char> AlphabetToList(char[,] array)
        {
            List<char> list = new List<char>();

            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    list.Add(array[i, j]);
                }
            }

            return list;
        }



        public static void PrintArray(char[,] array)
        {
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    Console.Write("{0} ", array[i, j]);
                }
                Console.WriteLine();
            }
        }
    }
}
