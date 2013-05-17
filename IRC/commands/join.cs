using System;
using System.Collections.Generic;
using System.Text;
using Meebey.SmartIrc4net;
using System.IO;

namespace IRC
{
    class join
    {
        public static void channeladd(string channel, IrcClient irc)
        {
            StringBuilder sb = new StringBuilder();
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
            if (write == true)
            {
                StreamWriter writer = new StreamWriter("channel.list", true);
                Nimbot.msgcolours(IRC.Nimbot.msglevel.info, "INFO");
                Console.WriteLine("Adding {0} to channel list", channel);
                writer.WriteLine(channel);
                writer.Close();
            }
            else
            {
                Nimbot.msgcolours(IRC.Nimbot.msglevel.info, "INFO");
                Console.WriteLine("{0} is already in the channel list", channel);
            }
        }
    }
}
