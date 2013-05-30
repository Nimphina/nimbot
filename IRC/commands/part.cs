using System;
using System.Collections.Generic;
using System.Text;
using Meebey.SmartIrc4net;
using System.IO;

namespace IRC
{
    public class part
    {
        public static void channelremove(string channel, string leaving_message, IrcClient irc)
        {
			irc.RfcPart(channel, leaving_message);
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
                    IRC.Nimbot.msgcolours(Nimbot.msglevel.info, "INFO");
                    Console.WriteLine("Removing {0} from channel list", channel);
                }
                else
                {
                    writer.WriteLine(readchan);
                }
            }
            writer.Close();
            reader.Close();
            System.IO.File.Delete("channel.tmp");
        }
    }
}

