using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace IRC
{
    class startup
    {
        public static void stage1 (string version)
        {
            Random rand = new Random();
            int random = rand.Next(0, 6);
            switch (random)
            {
                case 1:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case 2:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case 3:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case 4:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case 5:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case 6:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
            }
            
            Console.WriteLine("");
            Console.WriteLine("    _   __ _             __            __");
            Console.WriteLine("   / | / /(_)____ ___   / /_   ____   / /_");
            Console.WriteLine("  /  |/ // // __ `__ \\ / __ \\ / __ \\ / __/");
            Console.WriteLine(" / /|  // // / / / / // /_/ // /_/ // /_");
            Console.WriteLine("/_/ |_//_//_/ /_/ /_//_.___/ \\____/ \\__/");
            Console.ResetColor();
            Console.WriteLine("   Ver: {0} written by Nimphina", version);
            Console.WriteLine("");

        }
        public static void stage2()
        {
            Console.WriteLine("Configuration file not found: Creating a blank config file.");

            StreamWriter writer = new StreamWriter("config.conf");
            writer.WriteLine("Server:");
            Console.WriteLine("What server are you connecting to?");
            string writestring = Console.ReadLine();
            writer.WriteLine(writestring);

            Console.WriteLine("What port are you connecting to?");
            writestring = Console.ReadLine();
            writer.WriteLine("Port:");
            writer.WriteLine(writestring);

            Console.WriteLine("What is the root channel?");
            writestring = Console.ReadLine();
            writer.WriteLine("Root channel:");
            writer.WriteLine(writestring);

            Console.WriteLine("What is the bot's nick?");
            writestring = Console.ReadLine();
            writer.WriteLine("Bot nick:");
            writer.WriteLine(writestring);

            Console.WriteLine("Who is the bot operator?");
            writestring = Console.ReadLine();
            writer.WriteLine("Botop:");
            writer.WriteLine(writestring);

            Console.WriteLine("What is the command char?");
            writestring = Console.ReadLine();
            writer.WriteLine("Command char:");
            writer.WriteLine(writestring);

            writer.Close();
        }
    }
}
