using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Text;
using Meebey.SmartIrc4net;

namespace IRC
{
    class bcommands
    {

        public static void bc(string botop, string channel, string nick, string message, string server, int port, string version, ref string botname, ref string opsymbol, int timestart, IrcClient irc)
        {

            string[] args = message.TrimEnd().Split(' ');
            int lnth = args.Length;
            string command_check = args[0];

            switch (command_check)
            {
				case "test":
					if(nick == botop)
					{
						irc.SendMessage(SendType.Message, channel, "\"dadasdasd\"" , Priority.High);
					}
					break;
                
                case "g":
                case "google":
                    message = message.Replace(args[0], "");
					message = message.TrimStart(new char[]{' '});
                    google.search(message, channel, nick, irc);
                    break;

                case "help":
                    irc.SendMessage(SendType.Message, channel, "Avalible commands are:", Priority.High);
                    irc.SendMessage(SendType.Message, channel, "u, reddit, add, subtract, minus, multiply, divide, wafflebunny, ping, info, version, join, part, uptime", Priority.High);
                    break;

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
                
                case "wafflebunny":
                case "bunnywaffle":
                    bunny.waffle(channel, irc);
                    break;

                case "fortune":
                    quotes.quotegetter(channel, irc);
                    break;

                //All hardcoded, non class commands
                case "Ping":
                case "ping":
					Nimbot.msgcolours(IRC.Nimbot.msglevel.info, "INFO");
                    Console.WriteLine("ping command");
                    irc.SendMessage(SendType.Message, string.Format(channel, "#botspam"), "pong", Priority.High);
                    break;

                case "info":
                    irc.SendMessage(SendType.Message, channel, "I am a bot written by Nimphina, very messily written using the SmartIrc4net lib for irc stuff. Sauce code: http://github.com/Nimphina/Nimbot", Priority.High);
                    break;

                case "version":
                    irc.SendMessage(SendType.Message, channel, "Nimbot " + version, Priority.High);
                    break;

                case "gettime":
                    irc.SendMessage(SendType.Message, channel, string.Format("Bot server time is {0}", DateTime.Now.ToLongTimeString()), Priority.High);
                    break;

				case "say":
					message = message.Replace("say", "");
					message = message.TrimStart(new char[]{' '});
					irc.SendMessage(SendType.Message, channel, message, Priority.High);
					break;

                case "uptime":

                    int timenow = Nimbot.getmins();

                    int minutes = timenow - timestart;
                    int hour = 0;
					string hrst = "Hours"; string minst = "Minutes";
                    while (minutes >= 60)
                    {
                        minutes -= 60;
                        hour++;
                    }
					if (hour <= 1)
					{
						hrst = "Hour";
					}
					if (minutes <= 1)
					{
						minst = "Minute";
					}
					irc.SendMessage(SendType.Message, channel, string.Format("{0} {1} {2} {3}",hour, hrst, minutes, minst), Priority.High);
                    break;
		
				case "q":
                case "quit":
                    if (nick == botop)
                    {
						Nimbot.msgcolours(IRC.Nimbot.msglevel.critcial, "CRITICAL");
                        Console.WriteLine("Quit command issed by {0}", nick);
                        irc.SendMessage(SendType.Message, channel, "Qwitting", Priority.High);
                        
                        Environment.Exit(0);
                    }
                    else
                    {
						Nimbot.msgcolours(IRC.Nimbot.msglevel.critcial, "CRITICAL");
                        Console.WriteLine("Quit command issed by {0}", nick);
                        irc.SendMessage(SendType.Message, channel, "You are not allowed to issue that command", Priority.High);
						Nimbot.msgcolours(IRC.Nimbot.msglevel.critcial, "CRITICAL");
                        Console.WriteLine("Unauthorized user using quit, suggesting immediate extermination");
                    }
                    break;

                case "Hello":
                case "hello":
                case "hi":
                    irc.SendMessage(SendType.Message, channel, "Hello, " + nick, Priority.High);
                    break;

                case "join":
                    if (args[1].Contains("#"))
                    {
                        if (args[1] != "#fofftopic")
                        {
                            irc.RfcJoin(args[1]);
                            join.channeladd(args[1], irc);
                        }
                        else
                        {
                            irc.SendMessage(SendType.Message, channel, "Joining #fofftopic is not allowed!", Priority.High);
                        }
                    }
                    else
                    {
                        irc.SendMessage(SendType.Message, channel, "Not a valid channel.", Priority.High);
                    }
                    break;

				case "botop":
				case "operator":
				case "owner":
					irc.SendMessage(SendType.Message, channel, botop, Priority.High);
					break;

				case "leave":
                case "part":
					if (lnth == 1)
					{
						part.channelremove(channel,"leaving",  irc);
					}
                	else if (lnth == 2)
                	{
						part.channelremove(args[1], "leaving", irc);
                	}
                	else if (lnth >= 3)
                	{	
						part.channelremove(args[1],args[2], irc);
                	}
                    break;

                case "nick":
					if (nick == botop)
					{
	                    irc.RfcNick(args[1]);
						botname = args[1];
					}
					else
					{
						irc.SendMessage(SendType.Message, channel, "You are not allowed to perform that command!", Priority.High);
					}
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

				case "opchar":
				case "opsymbol":
					if (nick == botop)
					{
						if (lnth == 1)
						{
							irc.SendMessage(SendType.Message, channel, "I need a new symbol!", Priority.High);
						}
						else if (lnth > 1)
						{
							if (args[1].Length == 1)
							{
								opsymbol = args[1];
								irc.SendMessage(SendType.Message, channel, string.Format("The operator symbol has been changed to {0}", args[1]), Priority.High);
                                Nimbot.msgcolours(Nimbot.msglevel.info, "INFO");
								Console.WriteLine("Operator symbol has been changed to {0}", args[1] );
							}
							else
							{
								irc.SendMessage(SendType.Message, channel, "The symbol needs to be a single character.", Priority.High);
							}
						}
					}
					else
					{
						irc.SendMessage(SendType.Message, channel, "You are not allowed to perform that command!", Priority.High);
					}
					break;

				case "restart": 
					if (nick == botop)
					{
						try
						{
						System.Diagnostics.Process.Start("Nimbot.bat");
						Environment.Exit(0);
						}
						catch(Exception f)
						{
						    Console.WriteLine(f.Message);
						}
					}
				break;
            }
        }
    }
}
