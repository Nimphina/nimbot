using System;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Meebey.SmartIrc4net;

namespace IRC
{
    class Nimbot
    {
        public static IrcClient irc = new IrcClient();
        public string server;
        public int port;
        public static string rootchannel;
        public static string botop;
        public static string botname;
        public static string version = "dev-1.1.15";
        public static string opsymbol;
        public static string logging;
        public static DateTime StartTime = DateTime.Now;

        public static void Main()
        {
            Console.Title = "Nimbot " + version;
			//Console.SetWindowSize(100,100);
            Nimbot bot = new Nimbot();
        }

        public Nimbot()
        {

            startup.stage1(version);
            startup.stage2(out server, out port, out rootchannel, out botname, out botop, out opsymbol, out logging);
           
            irc.Encoding = System.Text.Encoding.UTF8;
            irc.ActiveChannelSyncing = true;
            irc.AutoReconnect = true;
            irc.AutoRetry = true;
            irc.AutoRetryDelay = 10;
            irc.AutoRelogin = true;
            irc.AutoJoinOnInvite = true;
            irc.CtcpVersion = "Nimbot " + version;
            irc.SendDelay = 300;

            //Setting some eventhandlers
            irc.OnConnected += new EventHandler(OnConnected);
            irc.OnConnecting += new EventHandler(OnConnecting);
            irc.OnPing += new PingEventHandler(OnPing);
            irc.OnDisconnected += new EventHandler(OnDisconnected);
            irc.OnChannelMessage += new IrcEventHandler(OnChannelMessage);
            irc.OnOp += new OpEventHandler(OnOp);
			irc.OnDeop += new DeopEventHandler(OnDeop);
            irc.OnQueryMessage += new IrcEventHandler(OnQueryMessage);
            irc.OnRawMessage += new IrcEventHandler(OnMessage);
            irc.OnBan += new BanEventHandler(OnBan);
            irc.OnTopicChange += new TopicChangeEventHandler(OnTopicChange);

            ThreadStart commandlinethread = new ThreadStart(cmd);
            Thread t1 = new Thread(commandlinethread);
            t1.Start();

            try
            {
                irc.Connect(server, port);
            }
            catch (Exception e)
            {
                t1.Abort();
                msgcolours(msglevel.critcial, "ERROR");
                Console.Write("Failed to connect: " + e.Message);

                Console.ReadKey();
            }
            t1.Abort();
        }

        void OnConnecting(object sender, EventArgs e)
        {
            msgcolours(msglevel.info, "INFO");
            Console.WriteLine("Attempting to connect to {0} on {1}.", server, port);
        }

        void OnConnected(object sender, EventArgs e)
        {
            msgcolours(msglevel.ok, "OK");
            Console.WriteLine("Successfully connected to {0} on port {1}.", server, port);

            irc.Login(botname, botname, 0, string.Format("{0}-bot", botname));

			msgcolours(msglevel.info, "INFO");
            Console.WriteLine("Joining rootchannel {0}.", rootchannel);
            irc.RfcJoin(rootchannel);

            try
            {
                StreamReader chanlistcheck = new StreamReader("channel.list");

                int chanarraylnth = 0;
                while (chanlistcheck.EndOfStream == false)
                {
                    chanlistcheck.ReadLine();
                    chanarraylnth++;
                }
                chanlistcheck.Close();
                string[] channel_list = new string[chanarraylnth];

                StreamReader chanreader = new StreamReader("channel.list");

                int i = 0;

                //Reading the list of channels and joining them
                while (chanreader.EndOfStream == false)
                {
                    channel_list[i] = chanreader.ReadLine();
                    i++;
                }

                i = 0;

                foreach (string value in channel_list)
                {
                    msgcolours(msglevel.info, "INFO");
                    Console.WriteLine("Joining {0}.", channel_list[i]);
                    irc.RfcJoin(channel_list[i]);
                    i++;
                }
                chanreader.Close();
            }
            catch (FileNotFoundException)
            {
                StreamWriter writer = new StreamWriter("channel.list");
                writer.Close();
            }
            finally
            {
                msgcolours(msglevel.ok, "OK");
                Console.WriteLine("All channels joined successfully.");

                if (logging == "enabled")
                {
                    Console.WriteLine("");
                    msgcolours(msglevel.warning, "WARNING");
                    Console.WriteLine("Channel logging is enabled.");
                    Console.ResetColor();
                }

                Console.WriteLine("");

            }
            irc.Listen(true);
        }

        public void OnChannelMessage(object sender, IrcEventArgs e)
        {
            string channel = e.Data.Channel;
            string message = e.Data.Message;
            string nick = e.Data.Nick;
            string bn = botname.ToLower(); // So that the how are you thing would work
            Random rand = new Random();
            int random = rand.Next(1,70);

            if (logging == "enabled")
            {
                StreamWriter writer = new StreamWriter(e.Data.Channel + ".log", true);
                writer.WriteLine("[{0}] ({1}) <{2}> {3}", DateTime.Now.ToShortTimeString(), channel, nick, message);
                writer.Close();
            }

            //message = message.Trim(new Char[] { '!', '?', '.', '\'', });

            if (message.StartsWith(opsymbol))
            {
                char opsymbolchar = Convert.ToChar(opsymbol);
                message = message.TrimStart(new Char[] { opsymbolchar });
                bcommands.bc(botop, channel, nick, message, server, port, version, ref botname, ref opsymbol, irc);
            }
            else if (random == 2 && message.Length > 15)
            {
                StreamWriter quoter = new StreamWriter("quotes", true);
                quoter.WriteLine("<{0}>: {1}", nick, message);
                quoter.Close();
                irc.SendMessage(SendType.Message, channel, "Quoted!", Priority.High);
            }

            else if (nick == "Ralph")
            {
                irc.SendMessage(SendType.Message, channel, "I hate Ralph and he hates me", Priority.High);
                Console.WriteLine("RALPH SAID SHIT");
            }

            else if (message == "What is love?")
            {
                irc.SendMessage(SendType.Message, channel, "Baby don't hurt me", Priority.High);
            }

            else if (message.ToLower() == "beep boop")
            {
                irc.SendMessage(SendType.Message, channel, "imma robot", Priority.High);
            }

            else if (message.ToLower() == "hello" || message.ToLower() == "hi")
            {
                irc.SendMessage(SendType.Message, channel, string.Format("Hello {0}", e.Data.Nick), Priority.High);
            }

            else if (message.ToLower() == string.Format("hello {0}, how are you?", bn) || message.ToLower() == string.Format("how are you doing {0}?", bn))
            {
                irc.SendMessage(SendType.Message, channel, string.Format("Hello {0}, I'm doing fine today, thanks", e.Data.Nick), Priority.High);
            }
        }

        void OnDisconnected(object sender, EventArgs e)
        {
            msgcolours(msglevel.critcial, "ERROR");
            Console.WriteLine("Disconnected from server.");
        }

        void OnPing(object sender, PingEventArgs e)
        {
            //Console.WriteLine("Responded to ping at {0}.", DateTime.Now.ToShortTimeString());
        }

        void OnOp(object sender, OpEventArgs e)
        {
			msgcolours(msglevel.channel, "CHANNEL");
			Console.WriteLine("{0} has given op to {1} in channel {2}.", e.Data.Nick, e.Data.RawMessageArray[4], e.Data.Channel);
        }

		void OnDeop(object sender, DeopEventArgs e)
        {
			msgcolours(msglevel.channel, "CHANNEL");
			Console.WriteLine("{0} has removed op from {1} in channel {2}.", e.Data.Nick, e.Data.RawMessageArray[4], e.Data.Channel);
        }

        void OnQueryMessage(object sender, IrcEventArgs e)
        {
            Console.WriteLine("[{0}] >{1}<: {2}", DateTime.Now.ToShortTimeString(), e.Data.Nick, e.Data.Message);
            string message = e.Data.Message;
            string nick = e.Data.Nick;

            if (e.Data.Message.StartsWith(opsymbol))
            {
                char opsymbolchar = Convert.ToChar(opsymbol);
                message = message.Trim(new Char[] { opsymbolchar });
                bcommands.bc(botop, nick, nick, message, server, port, version, ref botname, ref opsymbol, irc);
            }
        }

        public static void OnMessage(object sender, IrcEventArgs e)
        {
            string channel = e.Data.Channel;
            string nick = e.Data.Nick;
            string message = e.Data.Message;
			//Console.WriteLine(e.Data.Nick + e.Data.RawMessage);

            if (nick == "PING" || string.IsNullOrEmpty(nick) || nick == botname || string.IsNullOrEmpty(message) || message == ":")
            {
            }
            else
            {
                if (string.IsNullOrEmpty(channel))
                {
                    channel = "server";
					msgcolours(msglevel.server, "SERVER");
					Console.WriteLine("<{0}>: {1}", nick, message);
                }
				else
				{

					if(e.Data.RawMessageArray[1] == "PART")
					{
						msgcolours(msglevel.channel, "CHANNEL");
						Console.WriteLine("{0} has parted {1}", nick, e.Data.RawMessageArray[2]);
					}
					else if(e.Data.RawMessageArray[1] == "JOIN")
					{
						msgcolours(msglevel.channel, "CHANNEL");
						Console.WriteLine("{0} has joined {1}", nick, e.Data.RawMessageArray[2]);
					}
					else{
					msgcolours(msglevel.message, "MESSAGE");
					Console.WriteLine("({0}) <{1}>: {2}", channel, nick, message);
					}
				}
            }
        }

        void OnBan(object sender, BanEventArgs e)
        {
			if (e.Data.RawMessageArray[4].Contains(string.Format (botname + "-bot")))
			{
				msgcolours(msglevel.warning, "WARNING");
            	Console.WriteLine("{0} was banned from {1} by {2}.", botname, e.Data.Channel, e.Data.Nick);
				part.channelremove(e.Data.Channel, "BANNED", irc);
			}
			else
			{
				msgcolours(msglevel.channel, "CHANNEL");
				Console.WriteLine("{0} was banned from {1} by {2}.", e.Data.RawMessageArray[4], e.Data.Channel, e.Data.Nick);
			}
        }

        void OnTopicChange(object sender, TopicChangeEventArgs e)
        {
            msgcolours(msglevel.info, "INFO");
            Console.WriteLine("{0} changed {1}'s topic to {2}", e.Who, e.Channel, e.NewTopic);
        }

        public static void cmd()
        {
            string channel = rootchannel;

            while (true)
            {
                string consolemessage = Console.ReadLine();
				consolemessage = consolemessage.TrimStart(' ');

                if (consolemessage.StartsWith("/"))
                {
					consolemessage = consolemessage.TrimStart(new Char[] { '/', });
                    string[] args = consolemessage.TrimEnd().Split(' ');
                    int lnth = args.Length;

                    string command_check = args[0];

                    switch (command_check)
                    {
                        case "stop":
                        case "quit":
                            Environment.Exit(0);
                            break;

                        case "join":
                            if (lnth == 1)
                            {
                                msgcolours(msglevel.critcial, "ERROR");
                                Console.WriteLine("This command requires an argument!");
                            }
                            else
                            {
                                irc.RfcJoin(args[1]);
                                join.channeladd(args[1], irc);
                            }
                            break;

                        case "part":
                            if (lnth == 1)
                            {
                                msgcolours(msglevel.critcial, "ERROR");
                                Console.WriteLine("This command requires an argument!");
                            }

                            if (lnth == 2)
                            {
                                part.channelremove(args[1], "leaving", irc);
                            }

                            else if (lnth >= 3)
                            {
                                part.channelremove(args[1], args[2], irc);
                            }

                            break;

                        case "version":
                            Console.WriteLine(version);
                            break;

                        case "target":
                            if (lnth == 1)
                            {
                                msgcolours(msglevel.critcial, "ERROR");
                                Console.WriteLine("This command requires an argument!"); ;
                            }
                            else
                            {
                                channel = args[1];
                            }
                            break;

						case "ns":
						case "nickserv":
							consolemessage = consolemessage.Replace("ns", "");
							consolemessage = consolemessage.Replace("nickserv", "");
							consolemessage = consolemessage.TrimStart(' ');
							consolemessage = consolemessage.TrimEnd(' ');
							irc.SendMessage(SendType.Message, "NickServ", consolemessage, Priority.High);
							break;

                        case "ident":
                            msgcolours(msglevel.info, "INFO");
                            Console.Write("Enter your ident pass: ");
                            string pass = Console.ReadLine();
                            irc.SendMessage(SendType.Message, "NickServ", string.Format("identify {0} {1}", botname, pass), Priority.High);
                            break;

                        default:
                            msgcolours(msglevel.info, "INFO");
                            Console.WriteLine("Commands are, quit/stop, join, part, version and ident");
                            break;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(consolemessage))
                    {
                        msgcolours(msglevel.critcial, "ERROR");
                        Console.WriteLine("Message cannot be nothing!");
                    }
                    else
                    {
                        irc.SendMessage(SendType.Message, channel, string.Format("{0}", consolemessage), Priority.High);
                        msgcolours(msglevel.message, "MESSAGE");
                        Console.WriteLine("[" + DateTime.Now.ToShortTimeString() + "]" + "(" + channel + ") <" + botname + "> " + consolemessage);
                    }
                }
            }
        }

      
        public enum msglevel
        {
            ok,
            warning,
            critcial,
            info,
            message,
			server,
			channel
        }
        public static void msgcolours(msglevel state, string message)
        {
			Console.Write("[");
            if (state == msglevel.ok)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(message);
                Console.ResetColor();               
            }
            else if (state == msglevel.warning)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(message);
                Console.ResetColor();
            }
            else if (state == msglevel.critcial)
			{
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(message);
                Console.ResetColor();
            }
            else if (state == msglevel.info)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write(message);
                Console.ResetColor();
            }
            else if (state == msglevel.message)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write(message);
                Console.ResetColor();
            }
			else if (state == msglevel.server)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write(message);
                Console.ResetColor();
            }
			else if (state == msglevel.channel)
			{
				Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(message);
                Console.ResetColor();
			}
			Console.Write("]   ");
        }
    }
}
