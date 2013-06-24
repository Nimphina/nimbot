using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Meebey.SmartIrc4net;
using System.Net;
using System.Net.Sockets;

namespace IRC
{
    class versionchk
    {
        public static void checker(string clientversion, string channel,  IrcClient irc)
        {
            try
            {
                TcpClient tcpclnt = new TcpClient();
                Nimbot.msgcolours(IRC.Nimbot.msglevel.info, "INFO");
                Console.WriteLine("Connecting to version server");

                tcpclnt.Connect("192.210.212.82", 2051);
                // use the ipaddress as in the server program

                Nimbot.msgcolours(IRC.Nimbot.msglevel.ok, "OK");
                Console.WriteLine("Connected to version server");
                Stream stm = tcpclnt.GetStream();

                byte[] bb = new byte[100];
                int k = stm.Read(bb, 0, 100);
                string version = "";
                for (int i = 0; i < k; i++)
                {
                    version = version + Convert.ToChar(bb[i]);
                }
                tcpclnt.Close();

                clientversion = clientversion.Replace(".", "");
                clientversion = clientversion.Replace("dev-", "");
                int clientint = int.Parse(clientversion);

                version = version.Replace(".", "");
                int versionint = int.Parse(version);

                Console.WriteLine(versionint);
                if (clientint < versionint)
                {
                    irc.SendMessage(SendType.Message, channel, "A new version of Nimbot is avalible!", Priority.High);
                }
                else if (clientint > versionint)
                {
                    irc.SendMessage(SendType.Message, channel, "This seems to be a newer version that what the server says the new version is.", Priority.High);
                }
                else
                {
                    irc.SendMessage(SendType.Message, channel, "You are up to date!", Priority.High);
                }
            }

            catch (Exception e)
            {
                Nimbot.msgcolours(IRC.Nimbot.msglevel.critcial, "ERROR");
                Console.WriteLine("Error in connecting" + e.StackTrace);
            }
        }
    }
}
