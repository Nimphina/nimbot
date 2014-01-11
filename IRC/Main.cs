using System;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Meebey.SmartIrc4net;

/*Changelog (starting ver dev 1.1.21)
 * dev-1.1.21:
 * Commented the code a lot more, added changelog.
 * 
 * Todo:
 * Improve configuration method
 * 
 */

namespace IRC
{
    class Nimbot
    {
        //Set object variables
        private static IrcClient irc = new IrcClient();
        private static string server;
        public static int port;
        public static string rootchannel;
        public static string botop;
        public static string botname;
        public static string pass;
        public static string version = "dev-2.0.0";
        public static string opsymbol;
        public static string logging;
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
                irc.Connect(server, port); //Attempt connection to IRC server
            }
            catch (ConnectionException)
            {
                //If there is a failure to connect, the client SHOULD kill the cli thread, print error and then wait for user input before quitting. 
                t_cli.Abort();
                consolemsg(msglevel.critcial, "ERROR", "Failed to connect:");

                consolemsg(msglevel.info, "AWINPUT", "Press any key to continue.");
                Console.ReadKey();
            }
            t_cli.Abort();
        }
        //IRC event handlers

        private void OnConnecting(object sender, EventArgs e)
        {

        }

        private void OnConnected(object sender, EventArgs e)
        {

        }

        private void OnChannelMessage(object sender, IrcEventArgs e)
        {

        }

        private void OnDisconnected(object sender, EventArgs e)
        {

        }

        private void OnPing(object sender, PingEventArgs e)
        {

        }

        private void OnQueryMessage(object sender, IrcEventArgs e)
        {

        }

        private void OnMessage(object sender, IrcEventArgs e)
        {

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

        private static void cmd()
        {

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
        public static void consolemsg(msglevel state, string message, string alertmsg)
        {
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
            else if (state == msglevel.critcial)
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
    }

    class main
    {
        public static void Main()
        {
            Console.Title = "Nimbot";
            
            Nimbot bot = new Nimbot();
        }
    }
}
