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
        public static string checker(string clientversion, bool infos)
        {
            try
            {
                TcpClient tcpclnt = new TcpClient();
				if(infos == true){
                	Nimbot.msgcolours(IRC.Nimbot.msglevel.info, "INFO");
                	Console.WriteLine("Connecting to version server");
				}

                tcpclnt.Connect("192.210.212.82", 2051);
                // use the ipaddress as in the server program
				if(infos == true){
                	Nimbot.msgcolours(IRC.Nimbot.msglevel.ok, "OK");
                	Console.WriteLine("Connected to version server");
				}
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

                //Console.WriteLine(versionint);
                if (clientint < versionint)
                {
					return "A new version is avalible!";
                }
                else if (clientint > versionint)
                {
					return "I am ahead of the server";
                }
                else
                {
					return "I am up to date!";
                }
            }

            catch (Exception e)
            {
                Nimbot.msgcolours(IRC.Nimbot.msglevel.critcial, "ERROR");
                Console.WriteLine("Error in connecting" + e.StackTrace);
				return "Error in connection to server";
            }
        }
    }
}
