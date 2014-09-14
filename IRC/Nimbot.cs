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
        //Set object variables
        private static IrcClient irc = new IrcClient();
        public static Dictionary<string, string> config_options = Bottools.ConfigParser("config", "server port rootchannel botoperator nick nspassword commandchar logging");
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

            //Start threads for commandline interface
            ThreadStart CliThread = new ThreadStart(Cmd);
            Thread Cli = new Thread(CliThread);
            Cli.Start();


            try
            {   //This is probably a shitty way to do things
                if (config_options.ContainsKey("failed"))
                {
                    Bottools.ConsoleMsg(Bottools.msglevel.critical, string.Format("There is an error with the configuration: {0}", config_options["failed"]), "ERROR");
                }
                else if (config_options.ContainsKey("created"))
                {
                    Bottools.ConsoleMsg(Bottools.msglevel.info, "Config created, please restart the program", "INFO");
                }
                else
                {
                    irc.Connect(config_options["server"], int.Parse(config_options["port"])); //Attempt connection to IRC server
                }
            }
            catch (ConnectionException)
            {
                //If there is a failure to connect, the client SHOULD kill the cli thread, print error and then wait for user input before quitting. 
                Cli.Abort();
                Bottools.ConsoleMsg(Bottools.msglevel.critical, "Failed to connect:", "ERROR");

                Bottools.ConsoleMsg(Bottools.msglevel.info, "Press any key to continue.", "AWINP");
                Console.ReadKey();
            }
            Cli.Abort();
        }
        //IRC event handlers

        private void OnConnecting(object sender, EventArgs e)
        {
            Console.Title = "Nimbot - Connecting";
            StartText(version);
            Bottools.ConsoleMsg(Bottools.msglevel.info, string.Format("Attempting to connect to {0}", config_options["server"]), "INFO");
        }

        private void OnConnected(object sender, EventArgs e)
        {
            Console.Title = "Nimbot - Connected";
            Bottools.ConsoleMsg(Bottools.msglevel.ok, string.Format("Connected to {0}", config_options["server"]), "OK");

            irc.Login(config_options["nick"], config_options["nick"], 0, config_options["nick"] + "-bot");
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
                Bottools.ConsoleMsg(Bottools.msglevel.critical, "Channel list not found.", "ERROR");
            }
            finally
            {
                //Identify
                BotSay(string.Format("identify {0} {1}", config_options["nick"], config_options["nspassword"]), "Nickserv");
            }
            irc.Listen(true);
        }

        public void OnChannelMessage(object sender, IrcEventArgs e)
        {
            Bottools.ConsoleMsg(Bottools.msglevel.channel, e.Data.Message, "MESSAGE");

            if (e.Data.Message.StartsWith(Nimbot.config_options["commandchar"]))
            {
                Console.WriteLine(Commands.CommandHandler(e.Data.Message, e.Data.Channel, e.Data.Nick, config_options, this));
            }
            else if (e.Data.Message.ToLower().Contains("beep boop"))
            {
                BotSay("imma robot", e.Data.Channel);
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
            Bottools.ConsoleMsg(Bottools.msglevel.server, e.Data.Message, "SERVER");  //temp
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

        public static void BotSay(string message, string channel)
        {
            Bottools.ConsoleMsg(Bottools.msglevel.message, message, "BOTMSG");
            irc.SendMessage(SendType.Message, channel, message);
        }

        //Handle joining channels and adding them to the channel file
        public static void ChanJoin(string channel)
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
                Bottools.ConsoleMsg(Bottools.msglevel.info, string.Format("Adding {0} to channel list", channel), "INFO");
                writer.WriteLine(channel);
                writer.Close();
            }
            else
            {
                Bottools.ConsoleMsg(Bottools.msglevel.info, string.Format("{0} is already in the channel list", channel), "INFO");
            }
            irc.RfcJoin(channel);
        }

        //Handle leaving channels and removing them from the list
        public static void ChanDel(string channel, string leaving_message, IrcClient irc)
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
                    Bottools.ConsoleMsg(Bottools.msglevel.info, string.Format("Removing {0} from channel list", channel), "INFO");
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

        private static void StartText(string version)
        {
            Random rand = new Random();
            int random = rand.Next(0, 7);
            switch (random)
            {
                case 1:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case 2:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case 3:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case 4:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case 5:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case 6:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                case 7:
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
            }

            Console.WriteLine("");
            Console.WriteLine("                      _   __ _             __            __");
            Console.WriteLine("                     / | / /(_)____ ___   / /_   ____   / /_");
            Console.WriteLine("                    /  |/ // // __ `__ \\ / __ \\ / __ \\ / __/");
            Console.WriteLine("                   / /|  // // / / / / // /_/ // /_/ // /_");
            Console.WriteLine("                  /_/ |_//_//_/ /_/ /_//_.___/ \\____/ \\__/");

            Console.WriteLine("                      Ver: {0} written by Nimphina", version);
            Console.WriteLine("                      Reality cuts like a knife");
            Console.WriteLine("");
            // Console.WriteLine("---------------------------------------------------------------------------------");
            Console.ResetColor();

        }

        private static void Cmd()
        {

        }

    }

    class Bottools
    {
        public enum msglevel
        {
            ok,
            warning,
            critical,
            info,
            message,
            server,
            channel
        }
        public static void ConsoleMsg(msglevel state, string message, string alertmsg)
        {
            //Console.WriteLine(alertmsg.Length);
            if (alertmsg.Length < 5)
            {
                for (int i = 1; i <= 5 - alertmsg.Length; i++)
                {
                    alertmsg = alertmsg + "-";
                }
            }
            Console.Write("[");
            if (state == msglevel.ok)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(alertmsg);
                Console.ResetColor();
            }
            else if (state == msglevel.warning)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(alertmsg);
                Console.ResetColor();
            }
            else if (state == msglevel.critical)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(alertmsg);
                Console.ResetColor();
            }
            else if (state == msglevel.info)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write(alertmsg);
                Console.ResetColor();
            }
            else if (state == msglevel.message)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write(alertmsg);
                Console.ResetColor();
            }
            else if (state == msglevel.server)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write(alertmsg);
                Console.ResetColor();
            }
            else if (state == msglevel.channel)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(alertmsg);
                Console.ResetColor();
            }
            Console.Write("]   ");
            Console.WriteLine(message);
        }

        //Config get method, supply a file name and options 
        public static Dictionary<string, string> ConfigParser(string config_file, string config_opts)
        {
            if (File.Exists(config_file))
            {
                try
                {
                    StreamReader configreader = new StreamReader(config_file); //Open file for reading

                    Dictionary<string, string> config_options = new Dictionary<string, string>(); //Dict for our options

                    //Declare relevant vars
                    string file_line;
                    string option_data;
                    bool read_data = false;
                    bool found;

                    foreach (string word in config_opts.Split())
                    {
                        //reset variables
                        option_data = "";
                        found = false;
                        //Return to begining of file
                        configreader.DiscardBufferedData();
                        configreader.BaseStream.Seek(0, SeekOrigin.Begin);
                        configreader.BaseStream.Position = 0;

                        while (configreader.EndOfStream != true)
                        {
                            file_line = configreader.ReadLine(); //Shoud be in "word={option}" format
                            if (file_line.Contains(word) && file_line.Contains("{") && file_line.Contains("}"))
                            {
                                foreach (char letter in file_line)
                                {
                                    //Start reading from { till }
                                    if (letter == '{')
                                    {
                                        read_data = true;
                                    }
                                    else if (letter == '}')
                                    {
                                        read_data = false;
                                    }
                                    if (read_data && letter != '{')
                                    {
                                        option_data = option_data + letter; //Make string from chars we want
                                    }
                                }
                                found = true;
                                config_options.Add(word, option_data);
                                //Console.WriteLine(option_data);
                            }
                        }
                        if (found == false)
                        {
                            Console.WriteLine("Option {0} was not found or was in the incorrect format, please enter it now and update the config file accordingly", word);
                            option_data = Console.ReadLine();
                            config_options.Add(word, option_data);
                        }
                    }

                    configreader.Close();
                    return config_options; //for when we have finished writing this thing
                }
                catch (Exception e)
                {
                    // Console.WriteLine(e.Message);
                    Dictionary<string, string> failed_config = new Dictionary<string, string>();

                    failed_config.Add("failed", e.Message);

                    return failed_config;
                }
            }
            else
            {
                Console.WriteLine("{0} does not seem to exit, do you want to create it? [Y/n]", config_file);
                string user_reps = Console.ReadLine();
                if (String.IsNullOrEmpty(user_reps) || user_reps.ToLower().StartsWith("y"))
                {
                    Console.WriteLine("Creating a new config file.");

                    StreamWriter new_conf_write = new StreamWriter(config_file);

                    string opts;

                    foreach (string word in config_opts.Split(' '))
                    {
                        Console.WriteLine("Option for {0}", word);
                        opts = Console.ReadLine();
                        new_conf_write.WriteLine(word + "={" + opts + "}");
                    }
                    new_conf_write.Close();

                    Console.WriteLine("Config file created");

                    Dictionary<string, string> new_config = new Dictionary<string, string>();

                    new_config.Add("created", "created"); //Main program should decide what to do next.

                    return new_config;
                }
                else if (user_reps.ToLower().StartsWith("n"))
                {
                    Dictionary<string, string> failed_config = new Dictionary<string, string>();

                    failed_config.Add("failed", "Requested not to create new config file");

                    return failed_config;
                }
                else
                {
                    Dictionary<string, string> failed_config = new Dictionary<string, string>();

                    failed_config.Add("failed", "Unknown");

                    return failed_config;
                }
            }
        }
    }
}
