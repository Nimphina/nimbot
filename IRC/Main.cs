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
        public static string version = "dev-1.1.5";
        public static string opsymbol;
        public static int timestart;
        public static string logging;

        public static void Main()
        {
            Console.Title = "Nimbot " + version;
            Nimbot bot = new Nimbot();
        }

        public Nimbot()
        {
            // Get start time and covert it into minutes so it can be used to calculate uptime

            timestart = getmins();
            startup.stage1(version);
            startup.stage2(out server, out port, out rootchannel, out botname, out botop, out opsymbol, out logging);

            //Setting some eventhandlers
            irc.OnConnected += new EventHandler(OnConnected);
            irc.OnConnecting += new EventHandler(OnConnecting);
            irc.OnPing += new PingEventHandler(OnPing);
            irc.OnDisconnected += new EventHandler(OnDisconnected);
            irc.OnChannelMessage += new IrcEventHandler(OnChannelMessage);
            irc.OnOp += new OpEventHandler(OnOp);
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
                console_messages("fail", "ERROR");
                Console.Write("Failed to connect: " + e.Message);

                Console.ReadKey();
            }
			t1.Abort();
        }

        void OnConnecting(object sender, EventArgs e)
        {
            console_messages("info", "INFO");
            Console.WriteLine("Attempting to connect to {0} on {1}.", server, port);
        }

        void OnConnected (object sender, EventArgs e)
		{
            console_messages("ok", "OK");
			Console.WriteLine ("    Successfully connected to {0} on port {1}.", server, port);
            
			irc.Login (botname, botname, 0, string.Format ("{0}-bot", botname));

            try
            {
                StreamReader reader = new StreamReader("channel.list");
                string[] channel_list = new string[10];
                int i = 0;


                console_messages("info", "INFO");
                Console.WriteLine("Joining rootchannel {0}.", rootchannel);
                irc.RfcJoin(rootchannel);

                //Reading the list of channels and joining them
                while (reader.EndOfStream == false)
                {
                    channel_list[i] = reader.ReadLine();
                    i++;
                }

				i = 0;

                foreach (string value in channel_list)
                {
					if (string.IsNullOrEmpty(channel_list[i]))
					{
						//Stops console spam
					}
					else
					{
                        console_messages("ongoing", "ONGOING");
                        Console.WriteLine("Joining {0}.", channel_list[i]);
                        irc.RfcJoin(channel_list[i]);
						i++;
					}
                }
                reader.Close();
            }
            catch (FileNotFoundException)
            {
                StreamWriter writer = new StreamWriter("channel.list");
                writer.Close();
            }
			finally
			{

                console_messages("ok", "OK");
				Console.Write("All channels joined successfully.");

                if (logging == "enabled")
                {
                    Console.WriteLine("");
                    console_messages("warning", "WARNING");
                    Console.Write("Channel logging is enabled.");
                    Console.ResetColor();
                }

                Console.WriteLine("");
                Console.WriteLine("----------------------------------Server messages--------------------------------");
                Console.WriteLine("");

			}
            irc.Listen(true);
        }

        public void OnChannelMessage (object sender, IrcEventArgs e)
		{
            string channel = e.Data.Channel;
            string message = e.Data.Message;
            string nick = e.Data.Nick;
            string bn = botname.ToLower(); // So that the how are you thing would work

            if (logging == "enabled")
            {
                StreamWriter writer = new StreamWriter(e.Data.Channel + ".log", true);
                writer.WriteLine("[{0}] ({1}) <{2}> {3}", DateTime.Now.ToShortTimeString(), channel, nick, message);
                writer.Close();
            }

			message = message.Trim(new Char[] { '!', '?','.', '\'', });

			if (message.StartsWith (opsymbol)) {
				char opsymbolchar = Convert.ToChar (opsymbol);
				message = message.Trim (new Char[] { opsymbolchar }); 
				bcommands.bc (botop, channel, nick, message, server, port, version, ref botname, timestart, irc);
			}

			if (nick == "Ralph") {
				irc.SendMessage (SendType.Message, channel, "I hate Ralph and he hates me", Priority.High);
				Console.WriteLine ("RALPH SAID SHIT");
			}

			else if (message == "What is love") 
			{
				irc.SendMessage (SendType.Message, channel, "Baby don't hurt me", Priority.High);
			}

			else if (message.ToLower() == "beep boop") 
			{
				irc.SendMessage (SendType.Message, channel, "imma robot", Priority.High);
			}

			else if (message.ToLower() == "hello" || message.ToLower() == "hi") 
			{
				irc.SendMessage (SendType.Message, channel, string.Format ("Hello {0}", e.Data.Nick), Priority.High);
			}

			else if (message.ToLower() == string.Format ("hello {0}, how are you", bn) || message.ToLower() == string.Format ("how are you doing {0}", bn)) 
			{
				irc.SendMessage (SendType.Message, channel, string.Format ("Hello {0}, I'm doing fine today, thanks", e.Data.Nick), Priority.High);
			}
        }

        void OnDisconnected(object sender, EventArgs e)
        {
            Console.WriteLine("Disconnected from server.");
        }

        void OnPing(object sender, PingEventArgs e)
        {
            //Console.WriteLine("Responded to ping at {0}.", DateTime.Now.ToShortTimeString());
        }
        
        void OnOp(object sender, OpEventArgs e)
        {
            irc.SendMessage(SendType.Message, e.Channel, string.Format("Whoop!"), Priority.High);
        }

        void OnQueryMessage(object sender, IrcEventArgs e)
        {
            Console.WriteLine("[{0}] >{1}<: {2}", DateTime.Now.ToShortTimeString(), e.Data.Nick, e.Data.Message );
            string message = e.Data.Message;
            string nick = e.Data.Nick;

            if (e.Data.Message.StartsWith(opsymbol))
            {
                char opsymbolchar = Convert.ToChar(opsymbol);
                message = message.Trim(new Char[] { opsymbolchar });
                bcommands.bc(botop, nick, nick, message, server, port, version, ref botname, timestart, irc);
            }
        }

        public static void OnMessage (object sender, IrcEventArgs e)
		{
            string channel = e.Data.Channel;
            string nick = e.Data.Nick;
            string message = e.Data.Message;

            if (nick == "PING" || string.IsNullOrEmpty(nick) || nick == botname) 
			{
			} 
			else 
			{
                if (string.IsNullOrEmpty(channel))
                {
                    channel = "server";
                }
                try
                {
                    if (message.Contains(botname))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                }
                catch (Exception f)
                {
                    Console.WriteLine(f.Message);
                }
                Console.WriteLine("[{0}] ({1}) <{2}> {3}", DateTime.Now.ToShortTimeString(), channel, nick, message);

                Console.ResetColor();
			}
		}

        void OnBan(object sender, BanEventArgs e)
        {
			Console.WriteLine("{0} was banned from {1} by {2}.", botname, e.Data.Channel, e.Data.Nick);
            irc.RfcPart(e.Data.Channel);
        }

        void OnTopicChange(object sender, TopicChangeEventArgs e)
        {
            Console.WriteLine("{0} changed {1}, topic to {2}",e.Who, e.Channel, e.NewTopic);
        }

		public static void cmd ()
		{
			string channel = rootchannel;

			while (true) {
				string consolemessage = Console.ReadLine ();
				if (consolemessage.StartsWith ("/")) 
				{
					string[] args = consolemessage.TrimEnd ().Split (' ');
					int lnth = args.Length;

					string command_check = args[0].Trim (new Char[] {'/', });

					switch (command_check) {

					case "stop":
					case "quit":
						Environment.Exit (0);
						break;

					case "join":
						if (lnth == 1)
						{
							Console.WriteLine("This command requires an argument!");
						}
						else
						{
						irc.RfcJoin (args [1]);
						}
						break;
					
					case "part":
						if (lnth == 1)
						{
							Console.WriteLine("This command requires an argument!");
						}

						if (lnth == 2)
						{
						irc.RfcPart(args [1]);
						}

						else if (lnth >= 3)
                		{
                		irc.RfcPart(args[1] + args[2]);	
                		}

						break;

					case "version":
						Console.WriteLine(version);
						break;
					
					case "target":
						if (lnth == 1)
						{
							Console.WriteLine("This command requires an argument!");
							Console.Beep(40,10);
						}
						else
						{
						channel = args[1];
						}
						break;

					case "ident":
						Console.WriteLine ("Enter your ident pass");
						string pass = Console.ReadLine ();
						irc.SendMessage(SendType.Message, "NickServ", string.Format("identify {0} {1}", botname, pass), Priority.High);
						break;

					default :
						Console.WriteLine("Commands are, quit/stop, join, part, version and ident");
						break;
					}
				} 
				else 
				{
					if (string.IsNullOrEmpty(consolemessage))
					{
						Console.WriteLine("Message cannot be nothing!");
					}
					else
					{
					irc.SendMessage (SendType.Message, channel, string.Format ("{0}", consolemessage), Priority.High);
					Console.WriteLine ("[" + DateTime.Now.ToShortTimeString () + "]" + "(" + channel + ") <" + botname + "> " + consolemessage);
					}
				}
			}
        }

        public static int getmins()
        {
            string rawtime = DateTime.Now.ToShortTimeString();
            bool pmtrue = rawtime.Contains("PM");

            rawtime = rawtime.Replace("AM", "");
            rawtime = rawtime.Replace("PM", "");
            rawtime = rawtime.Replace(":", "");
            int time = int.Parse(rawtime);

            if (pmtrue == true)
            {
                time += 1200;
            }
            int time_mins = 0;
            while (time >= 100)
            {
                time -= 100;
                time_mins += 60;
            }
            time = time + time_mins;
            return time;
        }
        public static void console_messages(string state, string message)
        {
            if (state.ToLower() == "ok")
            {
                Console.Write("[ ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(message);
                Console.ResetColor();
                Console.Write(" ]   ");
            }
            else if (state.ToLower() == "warning")
            {
                Console.Write("[ ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(message);
                Console.ResetColor();
                Console.Write(" ]   ");
            }
            else if (state.ToLower() == "fail")
            {
                Console.Write("[ ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(message);
                Console.ResetColor();
                Console.Write(" ]   ");
            }
            else if (state.ToLower() == "info")
            {
                Console.Write("[ ");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write(message);
                Console.ResetColor();
                Console.Write(" ]   ");
            }
        }
    }
}
