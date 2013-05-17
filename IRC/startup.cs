using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace IRC
{
    class startup
    {
        public static void stage1(string version)
        {
            Random rand = new Random();
            int random = rand.Next(0, 7);
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
                case 7:
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
            }

            Console.WriteLine("");
            Console.WriteLine("                      _   __ _             __            __");
            Console.WriteLine("                     / | / /(_)____ ___   / /_   ____   / /_");
            Console.WriteLine("                    /  |/ // // __ `__ \\ / __ \\ / __ \\ / __/");
            Console.WriteLine("                   / /|  // // / / / / // /_/ // /_/ // /_");
            Console.WriteLine("                  /_/ |_//_//_/ /_/ /_//_.___/ \\____/ \\__/");

            Console.WriteLine("                      Ver: {0} written by Nimphina", version);
            Console.WriteLine("                 This bot may or may not have super cow powers");
            Console.WriteLine("");
            Console.WriteLine("---------------------------------------------------------------------------------");
            Console.ResetColor();

        }
        public static void stage2(out string server, out int port, out string rootchannel, out string botname, out string botop, out string opsymbol, out string logging)
        {
            try
            {
                TextReader reader = new StreamReader("config.conf");
                reader.Close();
            }
            catch (FileNotFoundException)
            {
                stage3();
            }
            finally
            {
                TextReader reader = new StreamReader("config.conf");
                reader.ReadLine(); //skip server title, get server
                server = reader.ReadLine();

                reader.ReadLine(); //get port
                string portstring = reader.ReadLine();
                port = int.Parse(portstring);

                reader.ReadLine(); //get root channel
                rootchannel = reader.ReadLine();

                reader.ReadLine(); //get botname
                botname = reader.ReadLine();

                reader.ReadLine(); //get botop
                botop = reader.ReadLine();

                reader.ReadLine(); //get opsymbol
                opsymbol = reader.ReadLine();

                reader.ReadLine(); //get opsymbol
                logging = reader.ReadLine();
                reader.Close();
            }
        }
        public static void stage3()
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

            writer.WriteLine("Logging:");
            writer.WriteLine("disabled");
            writer.Close();
        }
    }
}
