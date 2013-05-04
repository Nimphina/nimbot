using Meebey.SmartIrc4net;
using System;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace IRC
{
    class ClientDemo
    {
        public static IrcClient irc = new IrcClient();
        public string server;
        public int port;
        public static string rootchannel;
        public static string botop;
        public static string botname;
        public static string version = "dev-1.0.25";
        public static string opsymbol;


        public static void Main()
        {
            Console.Title = "Nimbot terminal";
            ClientDemo demo = new ClientDemo();
        }

        public ClientDemo()
        {
            try
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
                reader.Close();
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Creating a blank config file");

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

                writer.Close();

                Console.WriteLine("Now exiting, please restart nimbot");
                Console.ReadLine();
                Environment.Exit(0);
            }
            catch (FormatException)
            {
                Console.WriteLine("Some settings were incorrect in the config file, make sure the port is just numerals not letters i.e. 6667");
                Console.ReadLine();
                Environment.Exit(0);
            }
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
                Console.Write("Failed to connect: " + e.Message);
                Console.ReadKey();
				t1.Abort();
            }
        }

        void OnConnecting(object sender, EventArgs e)
        {
            Console.WriteLine("Nimbot version: {0} ", version);
            Console.WriteLine("Botop: {0}, Command char: {1}", botop, opsymbol);
            Console.WriteLine(" ");
        }

        void OnConnected (object sender, EventArgs e)
		{
			Console.WriteLine ("Connected to {0} on port {1}.", server, port);

			irc.Login (botname, botname, 0, string.Format ("{0}-bot", botname));

            try
            {
                StreamReader reader = new StreamReader("channel.list");
                string[] channel_list = new string[10];
                int i = 0;

                while (reader.EndOfStream == false)
                {
                    channel_list[i] = reader.ReadLine();
                    i++;
                }

				Console.WriteLine("Joining {0}.", rootchannel);
                foreach (string value in channel_list)
                {
                    irc.RfcJoin(channel_list[i]);
					Console.WriteLine("Joining {0}", channel_list[i]);
                }
                reader.Close();
            }
            catch (FileNotFoundException)
            {
                StreamWriter writer = new StreamWriter("channel.list");
                writer.Close();
				Console.WriteLine("Joining {0}.", rootchannel);
            }
			finally
			{
				irc.RfcJoin(rootchannel);
				Console.WriteLine("All channels joined successfully");
			}
            irc.Listen(true);
        }

        public void OnChannelMessage (object sender, IrcEventArgs e)
		{
			Console.WriteLine ("[" + DateTime.Now.ToShortTimeString () + "]" + "(" + e.Data.Channel + ") <" + e.Data.Nick + "> " + e.Data.Message);

			StreamWriter writer = new StreamWriter (e.Data.Channel + ".log", true);
			writer.WriteLine ("[" + DateTime.Now.ToShortTimeString () + "]" + "(" + e.Data.Channel + ") <" + e.Data.Nick + "> " + e.Data.Message);
			writer.Close ();

			string channel = e.Data.Channel;
			string message = e.Data.Message;
			string nick = e.Data.Nick;

			message = message.Trim(new Char[] { '!', '?','.', '\'', });

			if (message.StartsWith (opsymbol)) {
				char opsymbolchar = Convert.ToChar (opsymbol);
				message = message.Trim (new Char[] { opsymbolchar }); 
				bcommands.bc (botop, channel, nick, message, server, port, version, ref botname, irc);
			}

			if (nick == "Ralph") {
				irc.SendMessage (SendType.Message, channel, "I hate Ralph and he hates me", Priority.High);
				Console.WriteLine ("RALPH SAID SHIT");
			}

			if (message == "What is love") 
			{
				irc.SendMessage (SendType.Message, channel, "Baby don't hurt me", Priority.High);
			}

			if (message == "Hodor") 
			{
				irc.SendMessage (SendType.Message, channel, "Oh shut up", Priority.High);
			}

			if (message.ToLower() == "hello" || message.ToLower() == "hi") 
			{
				irc.SendMessage (SendType.Message, channel, string.Format ("Hello {0}", e.Data.Nick), Priority.High);
			}

			if (message.ToLower() == string.Format ("hello {0}, how are you", botname) || message.ToLower() == string.Format ("how are you doing {0}", botname)) 
			{
				irc.SendMessage (SendType.Message, channel, string.Format ("Hello {0}, I'm doing fine today, thanks", e.Data.Nick), Priority.High);
			}
        }

        void OnDisconnected(object sender, EventArgs e)
        {
            Console.WriteLine("Disconnected.");
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
            Console.WriteLine("Query: >" + e.Data.Nick +"<: " + e.Data.Message);
            string message = e.Data.Message;
            string nick = e.Data.Nick;

            if (e.Data.Message.StartsWith(opsymbol))
            {
                char opsymbolchar = Convert.ToChar(opsymbol);
                message = message.Trim(new Char[] { opsymbolchar });
                bcommands.bc(botop, nick, nick, message, server, port, version, ref botname, irc);
            }
        }

        void OnMessage (object sender, IrcEventArgs e)
		{
            if (e.Data.Nick == "PING" || string.IsNullOrEmpty(e.Data.Nick)) 
			{
			} 
			else 
			{
				Console.WriteLine ("Message: -" + e.Data.Nick + "-: " + e.Data.Message);
			}
		}

        void OnBan(object sender, BanEventArgs e)
        {
            Console.WriteLine("Bot was banned from: {0}", e.Data.Channel);
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
						irc.RfcJoin (args [1]);
						break;

					case "version":
						Console.WriteLine(version);
						break;
					
					case "target":
						channel = args[1];
						break;

					case "ident":
						Console.WriteLine ("Enter your ident pass");
						string pass = Console.ReadLine ();
						irc.SendMessage(SendType.Message, "NickServ", string.Format("identify {0} {1}", botname, pass), Priority.High);
						break;
					}
				} 
				else 
				{
					irc.SendMessage (SendType.Message, channel, string.Format ("{0}", consolemessage), Priority.High);
					Console.WriteLine ("[" + DateTime.Now.ToShortTimeString () + "]" + "(" + channel + ") <" + botname + "> " + consolemessage);
				}
			}
        }
    }
}
