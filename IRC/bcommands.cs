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

        public static void bc(string botop, string channel, string nick, string message, string server, int port, string version, ref string botname, ref string opsymbol, IrcClient irc)
        {

            string[] args = message.TrimEnd().Split(' ');
            int lnth = args.Length;
            string command_check = args[0].ToLower();

            switch (command_check)
            {
                case "test":
                    if (nick == botop)
                    {
                        irc.SendMessage(SendType.Message, channel, string.Format("{0} {1} {2} {3}", args[0], args[1], args[2], args[3]), Priority.High);
                    }
                    break;

                case "g":
                case "google":
                    if (lnth > 1)
                    {
                        message = message.Replace(args[0], "");
                        message = message.TrimStart(new char[] { ' ' });
                        google.search(message, channel, nick, irc);
                    }
                    else
                    {
                        irc.SendMessage(SendType.Message, channel, "Google: Takes your query and searches Google with it, displays two results.", Priority.High);
                    }
                    break;

                case "help":
                    irc.SendMessage(SendType.Message, channel, "Avalible commands are:", Priority.High);
                    irc.SendMessage(SendType.Message, channel, "u, reddit, add, subtract, minus, multiply, divide, wafflebunny, ping, info, version, join, part, uptime, say", Priority.High);
                    break;

                case "u":
                    if (lnth > 1)
                    {
                        u.umcf(args, lnth, channel, nick, irc);
                    }
                    else
                    {
                        irc.SendMessage(SendType.Message, channel, "u.mcf.li for lazy people. List of options: [profile, posts, topics, warnings, videos, friends, pm, names, admin, edit, modcp, validate, warn, suspend, iphistory]", Priority.High);
                    }
                    break;

                case "reddit":
                    if (lnth > 1){
                    redditclass.reddit(args, lnth, channel, nick, irc);
                    }
                    else
                    {
                        irc.SendMessage(SendType.Message, channel, "reddit for lazy people. Format: u <user> or r <subreddit>", Priority.High);
                    }
                    break;
                
                case "calc":
                    if (lnth > 1)
                    {
                        maths.calc(args, lnth, channel, nick, irc);
                    }
                    else
                    {
                        irc.SendMessage(SendType.Message, channel, "Options are: [+ - * / % **/^ ]", Priority.High);
                    }
                    break;

                case "wafflebunny":
                case "bunnywaffle":
                    bunny.waffle(channel, irc);
                    break;

                case "fortune":
                    quotes.quotegetter(channel, irc);
                    break;

                //All hardcoded, non class commands
                case "ping":
                    irc.SendMessage(SendType.Message, channel, "pong", Priority.High);
                    break;

                case "info":
                    irc.SendMessage(SendType.Message, channel, string.Format("I am a bot written by Nimphina, very messily written using the SmartIrc4net {0} lib for irc stuff. Sauce code: http://github.com/Nimphina/Nimbot", irc.VersionNumber), Priority.High);
                    break;

                case "version":
                    irc.SendMessage(SendType.Message, channel, "Nimbot " + version, Priority.High);
                    break;

                case "gettime":
                    irc.SendMessage(SendType.Message, channel, string.Format("Bot server time is {0}", DateTime.Now.ToLongTimeString()), Priority.High);
                    break;

                case "say":
                    message = "~" + message; //It's a shitty way to just remove the command from the string but if anyone has a better way...
                    message = message.Replace("~say", "");
                    message = message.TrimStart(new char[] { ' ' });
                    irc.SendMessage(SendType.Message, channel, message, Priority.High);
                    break;

                case "uptime":
                    DateTime NowTime = DateTime.Now;
                    TimeSpan diff = NowTime.Subtract(Nimbot.StartTime);

                    irc.SendMessage(SendType.Message, channel, string.Format("Uptime: {0} days {1} hours {2} minutes {3} seconds", diff.Days, diff.Hours, diff.Minutes, diff.Seconds));
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
                        part.channelremove(channel, "leaving", irc);
                    }
                    else if (lnth == 2)
                    {
                        part.channelremove(args[1], "leaving", irc);
                    }
                    else if (lnth >= 3)
                    {
                        part.channelremove(args[1], args[2], irc);
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

				case "serverinfo":
                    //super long if statment!
				string os_version = Environment.OSVersion.ToString();

                if (Environment.OSVersion.ToString().Contains("Unix") && Environment.OSVersion.ToString().Contains("3.") || Environment.OSVersion.ToString().Contains("Unix") && Environment.OSVersion.ToString().Contains("2."))
				{
					os_version = Environment.OSVersion.ToString ().Replace("Unix", "Linux");
				}
                else if (Environment.OSVersion.ToString().Contains(" Windows NT") && Environment.OSVersion.ToString().Contains("6.3"))
                {
                    os_version = Environment.OSVersion.ToString().Replace("NT", "8");
                }
                else if (Environment.OSVersion.ToString().Contains(" Windows NT") && Environment.OSVersion.ToString().Contains("6.1"))
                {
                    os_version = Environment.OSVersion.ToString().Replace("NT", "7");
                }
                else if (Environment.OSVersion.ToString().Contains(" Windows NT") && Environment.OSVersion.ToString().Contains("5.1"))
                {
                    os_version = Environment.OSVersion.ToString().Replace("NT", "XP");
                }
				
				irc.SendMessage(SendType.Message, channel, string.Format("Hostname: {0} OS: {1}, LocalTime: {2}", Environment.MachineName, os_version, DateTime.Now));
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
                                Console.WriteLine("Operator symbol has been changed to {0}", args[1]);
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
            }
        }
    }
}
