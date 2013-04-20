using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Meebey.SmartIrc4net;

namespace IRC
{
    class bcommands
    {

        public static void bc(string botop, string channel, string nick, string message, IrcClient irc)
        {

            string[] args = message.TrimEnd().Split(' ');
            Console.WriteLine("A command was used!");
            int lnth = args.Length;
            string switchmess = args[0];

            switch (switchmess)
            {
                case "u":
                    Console.WriteLine("U command");

                    if (lnth == 2)
                    {
                        irc.SendMessage(SendType.Message, channel, string.Format("{0}: http://u.mcf.li/{1}", nick, args[1]));
                    }
                    if (lnth == 3)
                    {
                        irc.SendMessage(SendType.Message, channel, string.Format("{0}: http://u.mcf.li/{1}/{2}", nick, args[1], args[2]));
                    }
                    else
                    {
                        irc.SendMessage(SendType.Message, channel, string.Format("{0}, List of options: [profile, posts, topics, warnings, videos, friends, pm, names, admin, edit, modcp, validate, warn, suspend, iphistory]", nick));
                    }
                    break;

                case "reddit":
                    if (lnth == 2)
                    {
                        irc.SendMessage(SendType.Message, channel, string.Format("{0}: http://reddit.com/r/{1}", nick, args[1]));
                    }
                    else
                    {
                        irc.SendMessage(SendType.Message, channel, string.Format("{0}: http://reddit.com/r/{1}", nick, "{1}"));
                    }
                    break;

                case "Ping":
                case "ping":
                    Console.WriteLine("ping command");
                    irc.SendMessage(SendType.Message, string.Format(channel, "#botspam"), "pong", Priority.High);
                    break;

                case "info":
                    irc.SendMessage(SendType.Message, channel, "I am a bot written by Nimphina, very messily written using the SmartIrc4net lib for irc stuff. Sauce code: http://github.com/Nimphina/Nimbot", Priority.High);
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

                case "add":
                    try
                    {
                        double[] operands = new double[lnth];
                        double noresult = 0;
                        if (lnth >= 3)
                        {
                            for (int i = 1; i < lnth; i++)
                            {
                                double workingnum = double.Parse(args[i]);
                                operands[i] = workingnum;
                                Console.WriteLine(operands[i]);

                                noresult = noresult + operands[i];
                            }

                            string result = Convert.ToString(noresult);
                            irc.SendMessage(SendType.Message, channel, result, Priority.High);


                            if (result == "69")
                            {
                                irc.SendMessage(SendType.Message, channel, "Heh heh, 69, hehe", Priority.High);
                            }
                        }
                        else
                        {
                            irc.SendMessage(SendType.Message, channel, "placholder message", Priority.High);
                        }
                    }
                    catch (Exception e)
                    {
                        irc.SendMessage(SendType.Message, channel, e.Message, Priority.High);
                    }
                    break;

                case "minus":
                    //Note, fix 1 - 1 producing -1 when you can be bothered
                    try
                    {
                        double[] operands = new double[lnth];
                        double noresult = 1;
                        if (lnth >= 3)
                        {
                            for (int i = 1; i < lnth; i++)
                            {
                                double workingnum = double.Parse(args[i]);
                                operands[i] = workingnum;
                                Console.WriteLine(operands[i]);

                                noresult = noresult - operands[i];
                            }

                            string result = Convert.ToString(noresult);
                            irc.SendMessage(SendType.Message, channel, result, Priority.High);


                            if (result == "69")
                            {
                                irc.SendMessage(SendType.Message, channel, "Heh heh, 69, hehe", Priority.High);
                            }
                        }
                        else
                        {
                            irc.SendMessage(SendType.Message, channel, "placholder message", Priority.High);
                        }
                    }
                    catch (Exception e)
                    {
                        irc.SendMessage(SendType.Message, channel, e.Message, Priority.High);
                    }
                    break;

                case "multiply":
                    try
                    {
                        double[] operands = new double[lnth];
                        double noresult = 1;
                        if (lnth >= 3)
                        {
                            for (int i = 1; i < lnth; i++)
                            {
                                double workingnum = double.Parse(args[i]);
                                operands[i] = workingnum;
                                Console.WriteLine(operands[i]);

                                noresult = noresult * operands[i];
                            }

                            string result = Convert.ToString(noresult);
                            irc.SendMessage(SendType.Message, channel, result, Priority.High);


                            if (result == "69")
                            {
                                irc.SendMessage(SendType.Message, channel, "Heh heh, 69, hehe", Priority.High);
                            }
                        }
                        else
                        {
                            irc.SendMessage(SendType.Message, channel, "placholder message", Priority.High);
                        }
                    }
                    catch (Exception e)
                    {
                        irc.SendMessage(SendType.Message, channel, e.Message, Priority.High);
                    }
                    break;

                case "divide":
                    try
                    {
                        double[] operands = new double[lnth];
                        double noresult = 1;
                        if (lnth >= 3)
                        {
                            for (int i = 1; i < lnth; i++)
                            {
                                double workingnum = double.Parse(args[i]);
                                operands[i] = workingnum;
                                Console.WriteLine(operands[i]);

                                noresult = noresult / operands[i];
                            }

                            string result = Convert.ToString(noresult);
                            irc.SendMessage(SendType.Message, channel, result, Priority.High);


                            if (result == "69")
                            {
                                irc.SendMessage(SendType.Message, channel, "Heh heh, 69, hehe", Priority.High);
                            }
                        }
                        else
                        {
                            irc.SendMessage(SendType.Message, channel, "placholder message", Priority.High);
                        }
                    }
                    catch (Exception e)
                    {
                        irc.SendMessage(SendType.Message, channel, e.Message, Priority.High);
                    }
                    break;

                case "condebug":
                    if (nick == botop)
                    {
                        irc.SendMessage(SendType.Message, channel, string.Format("{0}, Messges from console enabled", botop), Priority.High);
                        while (true)
                        {
                            Console.WriteLine("Enter a message: ");
                            string consolemessage = Console.ReadLine();

                            if (consolemessage == "/stop")
                            {
                                irc.SendMessage(SendType.Message, channel, string.Format("{0}, Messges from console disabled", botop), Priority.High);
                                break;
                            }
                            irc.SendMessage(SendType.Message, channel, string.Format("[{0}]", consolemessage), Priority.High);
                        }
                    }
                    break;
            }
        }
    }
}
