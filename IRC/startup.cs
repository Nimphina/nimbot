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
            // Console.WriteLine("---------------------------------------------------------------------------------");
            Console.ResetColor();

        }
        public static void stage2(ref string server, ref int port, ref string rootchannel, ref string botname, ref string pass, ref string botop, ref string opsymbol, ref string logging)
        {
            try
            {
                TextReader reader = new StreamReader("config");
                reader.Close();
            }
            catch (FileNotFoundException)
            {
                stage3();
            }
            finally
            {
                StreamReader reader = new StreamReader("config");
                int counter = 0;
                while (reader.EndOfStream == false)
                {
                    string[] work_string = reader.ReadLine().TrimEnd().Split(' ');;
                    //Console.WriteLine(work_string[0].ToLower().TrimEnd(new Char[] { ':' }) + work_string[1]);

                    switch(work_string[0].ToLower().TrimEnd(new Char[]{':'})){

                        case "server":
                            server = work_string[1];
                            break;

                        case "port":
                            try
                            {
                                port = int.Parse(work_string[1]);
                                counter++;
                            }
                            catch (FormatException)
                            {
                                Console.WriteLine("Port is not a integer!");
                                Console.ReadLine();
                                Environment.Exit(0);
                            }
                            break;

                        case "rootchannel":
                            rootchannel = work_string[1];
                            counter++;
                            break;

                        case "botname":
                        case "botnick":
                            botname = work_string[1];
                            counter++;
                            break;

                        case "password":
                        case "pass":
                            pass = work_string[1];
                            counter++;
                            break;

                        case "botop":
                        case "operator":
                            botop = work_string[1];
                            counter++;
                            break;

                        case "opsymbol":
                        case "commandchar":
                            opsymbol = work_string[1];
                            counter++;
                            break;

                        case "logging":
                            logging = work_string[1];
                            counter++;
                            break;

                        default:
                            Console.WriteLine("Switch has defaulted");
                            break;
                    }
                }
               
                reader.Close();
            }
        }
        private static void stage3()
        {
            Console.WriteLine("Configuration file not found: Creating a blank config file.");

            StreamWriter writer = new StreamWriter("config");
            
            Console.WriteLine("What server are you connecting to?");
            string writestring = Console.ReadLine();
            writer.WriteLine("Server: " + writestring);

            Console.WriteLine("What port are you connecting to?");
            writestring = Console.ReadLine();
            writer.WriteLine("Port: " + writestring);

            Console.WriteLine("What is the root channel?");
            writestring = Console.ReadLine();
            writer.WriteLine("Rootchannel: " + writestring);

            Console.WriteLine("What is the bot's nick?");
            writestring = Console.ReadLine();
            writer.WriteLine("Botname: " + writestring);

            Console.WriteLine("What is the bot's nickserv password?");
            writestring = Console.ReadLine();
            writer.WriteLine("Password: " + writestring);

            Console.WriteLine("Who is the bot operator?");
            writestring = Console.ReadLine();
            writer.WriteLine("Botop: " + writestring);

            Console.WriteLine("What is the command char?");
            writestring = Console.ReadLine();
            writer.WriteLine("Opsymbol: " + writestring);

            writer.WriteLine("Logging: Disabled");

            writer.Close();
        }
    }
}
