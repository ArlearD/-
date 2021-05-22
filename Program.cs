using System;
using System.Collections.Generic;
using System.IO;

namespace Информационная_безопасность
{
    class Program
    {
        static string symbols = @"0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ!""#$%&\'()*+,-./:;<=>?@[]^_`{|}~";

        static async System.Threading.Tasks.Task Main()
        {
            string password = "666";

            int passwordHash = password.GetHashCode();

            string passwordBankPath = @"C:\Users\Arleard\Desktop\john.txt";

            var dict = new Dictionary<int, string>();
            string line;

            using (StreamReader sr = new StreamReader(passwordBankPath))
            {
                while (!dict.ContainsKey(passwordHash) || sr.EndOfStream)
                {
                    if ((line = await sr.ReadLineAsync()) == null)
                        break;
                    dict.Add(line.GetHashCode(), line);
                }
            }

            if (dict.ContainsKey(passwordHash))
                Console.WriteLine($"Пароль найден в словаре: {dict[passwordHash]}");
            else
            {
                Console.WriteLine("Пароля нет в словаре, продолжить поиск с помощью подбора?");
                Console.WriteLine("Y/N");
                if (Console.ReadKey().Key == ConsoleKey.Y)
                {
                    Console.WriteLine();
                    var result = BruteForce(password.Length, passwordHash);
                    if (result == null)
                        Console.WriteLine("Не удалось подобрать пароль");
                    else
                        Console.WriteLine($"Искомый пароль: {result}");
                }
            }

            string BruteForce(int depth, int passwordHash, string currentString = null)
            {
                string result = null;
                for (int i = 0; result == null && i < symbols.Length; i++)
                {
                    string password = currentString + symbols[i];
                    if (password.GetHashCode() == passwordHash)
                        return password;
                    else if (password.Length < depth)
                        result = BruteForce(depth, passwordHash, password);
                }
                return result;
            }
        }
    }
}
