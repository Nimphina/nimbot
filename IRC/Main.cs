using System;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Meebey.SmartIrc4net;

/*Changelog (starting ver dev 1.1.21)
 * dev-2.0.0:
 * Complete re-write of main functions.
 * Improved configuration
 * Implemented some basic functionality
 * Created a new class with some common methods
 * Added join/part and message commands to bot object
 * 
 * dev-1.1.21:
 * Commented the code a lot more, added changelog.
 * 
 * Todo:
 * Fix some stablity issues regarding the configuration and add proper exception handling 
 */

namespace IRC
{
    class Nimbot
    {
        //Set object variables
        private static IrcClient irc = new IrcClient();
        public static Dictionary<string, string> config_options = bottools.configparser("config", "server port rootchannel botoperator nick nspassword commandchar logging");
        public static string version = "dev-2.0.0";
        public static bool debug = false;
        public static string server_name = "";
        public static DateTime StartTime = DateTime.Now;

        public Nimbot()
        {

            //Set irc library related options
            irc.Encoding = System.Text.Encoding.UTF8;
            irc.ActiveChannelSyncing = true;
            irc.AutoReconnect = true;
            irc.AutoRetry = true;
            irc.AutoRetryDelay = 10;
            irc.AutoRelogin = true;
            irc.AutoJoinOnInvite = true;
            irc.CtcpVersion = "Nimbot";
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

            //Star threads for commandline interface
            ThreadStart commandlinethread = new ThreadStart(cmd);
            Thread t_cli = new Thread(commandlinethread);
            t_cli.Start();


            try
            {
                if (config_options.ContainsKey("failed")|| config_options.ContainsKey("created"))
                {
                    Console.WriteLine("Panic");
                }
                else
                {
                    irc.Connect(config_options["server"], int.Parse(config_options["port"])); //Attempt connection to IRC server
                }
            }
            catch (ConnectionException)
            {
                //If there is a failure to connect, the client SHOULD kill the cli thread, print error and then wait for user input before quitting. 
                t_cli.Abort();
                bottools.consolemsg(bottools.msglevel.critcial, "Failed to connect:", "ERROR");

                bottools.consolemsg(bottools.msglevel.info, "Press any key to continue.", "AWINP");
                Console.ReadKey();
            }
            t_cli.Abort();
        }
        //IRC event handlers

        private void OnConnecting(object sender, EventArgs e)
        {
            Console.Title = "Nimbot - Connecting";
            startup.stage1(version);
            bottools.consolemsg(bottools.msglevel.info, string.Format("Attempting to connect to {0}", config_options["server"]), "INFO");
        }

        private void OnConnected(object sender, EventArgs e)
        {
            Console.Title = "Nimbot - Connected";
            bottools.consolemsg(bottools.msglevel.ok, string.Format("Connected to {0}", config_options["server"]), "OK");

            irc.Login(config_options["nick"],config_options["nick"], 0, config_options["nick"] + "-bot");
            irc.RfcJoin(config_options["rootchannel"]);

            try //Load the channel list
            {
                StreamReader channelloader = new StreamReader("channel.list");

                while (channelloader.EndOfStream == false)
                {
                    irc.RfcJoin(channelloader.ReadLine());
                }
                channelloader.Close();
            }
            catch (FileNotFoundException)
            {
                bottools.consolemsg(bottools.msglevel.critcial, "Channel list not found.", "ERROR");
            }
            finally
            {
                //Identify
                botmsg(string.Format("identify {0} {1}", config_options["nick"], config_options["nspassword"]), "Nickserv");
            }
            irc.Listen(true);
        }

        private void OnChannelMessage(object sender, IrcEventArgs e)
        {
            bottools.consolemsg(bottools.msglevel.channel, e.Data.Message, "MESSAGE"); 
            if (e.Data.Message == "hello")
            {
                Nimbot.botmsg("Hi", e.Data.Channel);
            }
        }

        private void OnDisconnected(object sender, EventArgs e)
        {

        }

        private void OnPing(object sender, PingEventArgs e)
        {
            //Stay with us
        }

        private void OnQueryMessage(object sender, IrcEventArgs e)
        {

        }

        private void OnMessage(object sender, IrcEventArgs e)
        {
            bottools.consolemsg(bottools.msglevel.server, e.Data.Message, "SERVER");  //temp
        }

        private void OnBan(object sender, BanEventArgs e)
        {

        }
        private void OnTopicChange(object sender, TopicChangeEventArgs e)
        {

        }

        private void OnOp(object sender, OpEventArgs e)
        {

        }

        private void OnDeop(object sender, DeopEventArgs e)
        {

        }
        //Bot specific methods

        public static void botmsg(string message, string channel)
        {
            bottools.consolemsg(bottools.msglevel.message, message, "BOTMSG");
            irc.SendMessage(SendType.Message, channel, message);
        }

        //Handle joining channels and adding them to the channel file
        public static void channeljoin(string channel)
        {

            string line;
            bool write = true;

            using (System.IO.StreamReader file = new System.IO.StreamReader("channel.list"))
            {
                while ((line = file.ReadLine()) != null)
                {
                    if (line.Contains(channel))
                    {
                        write = false;
                    }
                }
            }
            if (write)
            {
                StreamWriter writer = new StreamWriter("channel.list", true);
                bottools.consolemsg(bottools.msglevel.info, string.Format("Adding {0} to channel list", channel), "INFO");
                writer.WriteLine(channel);
                writer.Close();
            }
            else
            {
                bottools.consolemsg(bottools.msglevel.info, string.Format("{0} is already in the channel list", channel), "INFO");
            }
            irc.RfcJoin(channel);
        }

        //Handle leaving channels and removing them from the list
        public static void channelremove(string channel, string leaving_message, IrcClient irc)
        {
           
            System.IO.File.Copy("channel.list", "channel.tmp", true);
            StreamWriter writer = new StreamWriter("channel.list");
            StreamReader reader = new StreamReader("channel.tmp");
            string readchan;
            while (reader.EndOfStream == false)
            {
                readchan = reader.ReadLine();
                readchan = readchan.TrimEnd(' ');
                if (readchan == channel)
                {
                    //write nothing \o/
                    bottools.consolemsg(bottools.msglevel.info, string.Format("Removing {0} from channel list", channel), "INFO");
                }
                else
                {
                    writer.WriteLine(readchan);
                }
            }
            writer.Close();
            reader.Close();
            System.IO.File.Delete("channel.tmp");

            irc.RfcPart(channel, leaving_message);
        }

        private static void cmd()
        {

        }

    }

    class main
    {
        public static void Main()
        {
            Console.Title = "Nimbot";
            
            Nimbot bot = new Nimbot();
            Console.ReadLine();
        }
    }
}
