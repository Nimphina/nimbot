using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Meebey.SmartIrc4net;

namespace IRC
{
    class bcommands
    {

        public static void bc(string botop, string channel, string nick, string message, string version, IrcClient irc)
        {

            string[] args = message.TrimEnd().Split(' ');
            Console.WriteLine("A command was used!");
            int lnth = args.Length;
            string switchmess = args[0];

            switch (switchmess)
            {
                case "u":
                    u.umcf(args, lnth, channel, nick, irc);
                    break;

                case "reddit":
                    redditclass.reddit(args, lnth, channel, nick, irc);
                    break;

                case "add":
                    addition.add(args, lnth, channel, nick, irc);
                    break;

                case "minus":
                    subtraction.minus(args, lnth, channel, nick, irc);
                    break;

                case "multiply":
                    multiplication.multi(args, lnth, channel, nick, irc);
                    break;

                case "divide":
                    division.divide(args, lnth, channel, nick, irc);
                    break;

                //All hardcoded, non class commands
                case "Ping":
                case "ping":
                    Console.WriteLine("ping command");
                    irc.SendMessage(SendType.Message, string.Format(channel, "#botspam"), "pong", Priority.High);
                    break;

                case "info":
                    irc.SendMessage(SendType.Message, channel, "I am a bot written by Nimphina, very messily written using the SmartIrc4net lib for irc stuff. Sauce code: http://github.com/Nimphina/Nimbot", Priority.High);
                    break;

                case "version":
                    irc.SendMessage(SendType.Message, channel, version, Priority.High);
                    break;

                case "quit":
                    if (nick == botop)
                    {
                        Console.WriteLine("Quit command issed by {0}", nick);
                        irc.SendMessage(SendType.Message, channel, "Qwitting", Priority.High);
                        Environment.Exit(0);
                    }
                    else
                    {
                        Console.WriteLine("Quit command issed by {0}", nick);
                        irc.SendMessage(SendType.Message, channel, "You are not allowed to issue that command :C", Priority.High);
                        Console.WriteLine("Unauthorized user using quit, suggesting immediate extermination");
                    }
                    break;

                case "Hello":
                case "hello":
                case "hi":
                    irc.SendMessage(SendType.Message, channel, "Hello, " + nick, Priority.High);
                    break;

                case "join":
                    if (args[1] != "#fofftopic")
                    {
                        irc.RfcJoin(args[1]);
                    }
                    else
                    {
                        irc.SendMessage(SendType.Message, channel, "Joining #fofftopic is not allowed!", Priority.High);
                    }
                    break;

                case "part":
                    irc.RfcPart(args[1]);
                    break;

                case "condebug":
                    if (nick == botop)
                    {
                        irc.SendMessage(SendType.Message, channel, string.Format("{0}, Messages from console enabled", botop), Priority.High);
                        while (true)
                        {
                            Console.WriteLine("Enter a message: ");
                            string consolemessage = Console.ReadLine();

                            if (consolemessage == "/stop")
                            {
                                irc.SendMessage(SendType.Message, channel, string.Format("{0}, Messages from console disabled", botop), Priority.High);
                                break;
                            }
                            irc.SendMessage(SendType.Message, channel, string.Format("[{0}]", consolemessage), Priority.High);
                        }
                    }
                    else
                    {
                        irc.SendMessage(SendType.Message, channel, "You are not allowed to issue that command :C", Priority.High);
                    }
                    break;
            }
        }
    }
}
