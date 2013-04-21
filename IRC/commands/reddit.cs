using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Meebey.SmartIrc4net;

namespace IRC
{
	public class redditclass
	{
		public static void reddit (string[] args, int lnth, string channel, string nick, IrcClient irc)
		{
            Console.WriteLine("{0}, called reddit command", nick);
			if (lnth == 3)
            {
				if (args[1] == "user")
				{
					irc.SendMessage(SendType.Message, channel, string.Format("{0}: http://reddit.com/u/{1}", nick, args[2]));
				}
				else if (args[1] == "sub")
				{
                	irc.SendMessage(SendType.Message, channel, string.Format("{0}: http://reddit.com/r/{1}", nick, args[2]));
				}
				else
            	{
                	irc.SendMessage(SendType.Message, channel, string.Format("{0}, Command choices are user or sub", nick, "{1}"));
            	}
            }
            else
            {
                irc.SendMessage(SendType.Message, channel, string.Format("{0}, Ccommand choices are user or sub + user or subreddit", nick, "{1}"));
            }
		}
	}
}

