using System;
using System.Collections.Generic;
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

				case "kick":
					if(nick == botop)
					{
						if(lnth == 2)
						{
							irc.RfcKick(channel, args[1]);
						}
						else if (lnth == 3)
						{
							irc.RfcKick(channel, args[1], args[2]);
						}	
						else
						{
							irc.SendMessage(SendType.Message, channel, "Who do you want to kick?", Priority.High);
						}
					}
					else
					{
						irc.SendMessage(SendType.Message, channel, "You are not authorised to perform that command", Priority.High);
					}
					break;

				case "op":
					if(nick == botop)
					{
						if(lnth == 2)
						{
							irc.Op(channel, args[1]);
						}
						else
						{
							irc.SendMessage(SendType.Message, channel, "Who do you want to op?", Priority.High);
						}
					}
					else
					{
						irc.SendMessage(SendType.Message, channel, "You are not authorised to perform that command", Priority.High);
					}
					break;

				case "deop":
					if(nick == botop)
					{
						if(lnth == 2)
						{
							irc.Deop(channel, args[1]);
						}
						else
						{
							irc.SendMessage(SendType.Message, channel, "Who do you want to deop?", Priority.High);
						}
					}
					else
					{
						irc.SendMessage(SendType.Message, channel, "You are not authorised to perform that command", Priority.High);
					}
					break;

				case "devoice":
					if(nick == botop)
					{
						if(lnth == 2)
						{
							irc.Devoice(channel, args[1]);
						}
						else
						{
							irc.SendMessage(SendType.Message, channel, "Who do you want to devoice?", Priority.High);
						}
					}
					else
					{
						irc.SendMessage(SendType.Message, channel, "You are not authorised to perform that command", Priority.High);
					}
					break;

				case "voice":
					if(nick == botop)
					{
						if(lnth == 2)
						{
							irc.Voice(channel, args[1]);
						}
						else
						{
							irc.SendMessage(SendType.Message, channel, "Who do you want to voice?", Priority.High);
						}
					}
					else
					{
						irc.SendMessage(SendType.Message, channel, "You are not authorised to perform that command", Priority.High);
					}
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
