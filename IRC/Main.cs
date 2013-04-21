using Meebey.SmartIrc4net;
using System;
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
        public string rootchannel;
        public string botop;
        public string botname;
        public string version = "dev-1.0.14";
        public string opsymbol = "#";


        public static void Main()
        {
            Console.Title = "Nimbot terminal";
            ClientDemo demo = new ClientDemo();
        }

        public ClientDemo()
        {

            try
            {
                TextReader reader = new StreamReader("config");
                reader.ReadLine();
                server = reader.ReadLine();
                reader.ReadLine();
                string portstring = reader.ReadLine();
                port = int.Parse(portstring);
                reader.ReadLine();
                rootchannel = reader.ReadLine();
                reader.ReadLine();
                botname = reader.ReadLine();
                reader.ReadLine();
                botop = reader.ReadLine();
                reader.Close();
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Creating a blank config file");
                StreamWriter writer = new StreamWriter("config");
                writer.WriteLine("Server:");
                writer.WriteLine(" ");
                writer.WriteLine("Port:");
                writer.WriteLine(" ");
                writer.WriteLine("Root channel:");
                writer.WriteLine(" ");
                writer.WriteLine("Bot nick:");
                writer.WriteLine(" ");
                writer.WriteLine("Botop:");
                writer.WriteLine(" ");
                writer.WriteLine("Command char:");
                writer.WriteLine(" ");
                writer.Close();
                Console.WriteLine("Now exiting");
                Console.ReadLine();
                Environment.Exit(0);
            }
            catch (FormatException)
            {
                Console.WriteLine("Some settings were incorrect in the config file, make sure the port is just numerals not letters i.e. 6667");
                Console.ReadLine();
                Environment.Exit(2);
            }

            irc.OnConnected += new EventHandler(OnConnected);
            irc.OnConnecting += new EventHandler(OnConnecting);
            irc.OnPing += new PingEventHandler(OnPing);
            irc.OnDisconnected += new EventHandler(OnDisconnected);
            irc.OnChannelMessage += new IrcEventHandler(OnChannelMessage);

            try
            {
                irc.Connect(server, port);
            }
            catch (Exception e)
            {
                Console.Write("Failed to connect: " + e.Message);
                Console.ReadKey();
            }
        }

        void OnConnecting(object sender, EventArgs e)
        {
            Console.WriteLine("Starting Nimbot version: {0} ", version);
            Console.WriteLine("Botop: {0}, Command char: {1}", botop, "#");
            Console.WriteLine(" ");
            Console.WriteLine("Connecting to server {0} on port {1}.", server, port);
        }

        void OnConnected(object sender, EventArgs e)
        {
            if (rootchannel == "fofftopic")
            {
                Environment.Exit(0);
            }
            Console.WriteLine("Connected to {0}.", server);
            //irc.SendMessage(SendType.Message, channel, "Joined: " + channel + " Bot op is: " + botop, Priority.BelowMedium);

            irc.Login(botname, botname, 0, botname);
            irc.RfcJoin(rootchannel);
            Console.WriteLine("Joining {0}.", rootchannel);
            irc.Listen(true);
        }

        public void OnChannelMessage(object sender, IrcEventArgs e)
        {
            Console.WriteLine("(" + e.Data.Channel + ") <" + e.Data.Nick + "> " + e.Data.Message);

            string channel = e.Data.Channel;
            string message = e.Data.Message;
            string nick = e.Data.Nick;

            if (e.Data.Message.StartsWith(opsymbol))
            {
                message = message.Trim(new Char[] { '#' }); //will have to find a way for this to work with the varibles
                bcommands.bc(botop, channel, nick, message, version, irc);
            }

            if (e.Data.Nick == "Ralph")
            {
                irc.SendMessage(SendType.Message, channel, "I hate Ralph and he hates me", Priority.High);
                Console.WriteLine("RALPH SAID SHIT");
            }

            if (e.Data.Message == "What is love?")
            {
                irc.SendMessage(SendType.Message, channel, "Baby don't hurt me", Priority.High);
            }

            if (e.Data.Message == "Hodor!")
            {
                irc.SendMessage(SendType.Message, channel, "Oh shut up", Priority.High);
            }

            if (e.Data.Message == "The war z")
            {
                irc.SendMessage(SendType.Message, channel, "http://www.youtube.com/watch?v=RtKAm3nzg6I", Priority.High);
            }

            if (e.Data.Message == "Hello" || e.Data.Message == "hello" || e.Data.Message == "Hi" || e.Data.Message == "hi")
            {

                irc.SendMessage(SendType.Message, channel, string.Format("Hello {0}", e.Data.Nick), Priority.High);
            }
        }

        void OnDisconnected(object sender, EventArgs e)
        {
            Console.WriteLine("Disconnected.");
        }

        void OnPing(object sender, PingEventArgs e)
        {
            Console.WriteLine("Responded to ping at {0}.", DateTime.Now.ToShortTimeString());
        }
    }
}