using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Meebey.SmartIrc4net;

namespace IRC
{
    class bcommands
    {

        public static void bc(string botop, string channel, string nick, string message, string server, int port, string version, IrcClient irc)
        {

            string[] args = message.TrimEnd().Split(' ');
            Console.WriteLine("Command char detected!");
            int lnth = args.Length;
            string command_check = args[0];

            switch (command_check)
            {
                case "U":
                case "u":
                    u.umcf(args, lnth, channel, nick, irc);
                    break;
                
                case "Reddit":
                case "reddit":
                    redditclass.reddit(args, lnth, channel, nick, irc);
                    break;
                
                case "Add":
                case "add":
                    addition.add(args, lnth, channel, nick, irc);
                    break;
                
                case "Minus":
                case "minus":
                    subtraction.minus(args, lnth, channel, nick, irc);
                    break;
                
                case "Multiply":
                case "multiply":
                    multiplication.multi(args, lnth, channel, nick, irc);
                    break;
                
                case "Divide":
                case "divide":
                    division.divide(args, lnth, channel, nick, irc);
                    break;

                case "bunnywaffle":
                    bunny.waffle(channel, irc);
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

                case "gettime":
                    irc.SendMessage(SendType.Message, channel, string.Format("Bot server time is {0}", DateTime.Now.ToShortTimeString()), Priority.High);
                    break;

                case "quit":
                    if (nick == botop)
                    {
                        Console.WriteLine("Quit command issed by {0}", nick);
                        irc.SendMessage(SendType.Message, channel, "Qwitting", Priority.High);
                        irc.Disconnect();
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
                        StreamWriter writer = new StreamWriter(args[1]);
                        writer.WriteLine(args[1]);
                        writer.Close();
                    }
                    else
                    {
                        irc.SendMessage(SendType.Message, channel, "Joining #fofftopic is not allowed!", Priority.High);
                    }
                    break;

                case "part":
                    irc.RfcPart(args[1]);
                    break;

                case "nick":
                    irc.RfcNick(args[1]);
                    break;

                //Bot op only commands
                case "op":
                    opitems.op(channel, botop, nick, args, lnth, irc);
                    break;

                case "deop":
                    opitems.deop(channel, botop, nick, args, lnth, irc);
                    break;

                case "voice":
                    opitems.voice(channel, botop, nick, args, lnth, irc);
                    break;
                case "devoice":
                    opitems.devoice(channel, botop, nick, args, lnth, irc);
                    break;

                case "kick":
                    opitems.kick(channel, botop, nick, args, lnth, irc);
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
                            irc.SendMessage(SendType.Message, channel, string.Format("{0}", consolemessage), Priority.High);
                        }
                    }
                    else
                    {
                        irc.SendMessage(SendType.Message, channel, "You are not allowed to perform that command!", Priority.High);
                    }
                    break;
            }
        }
    }
}
