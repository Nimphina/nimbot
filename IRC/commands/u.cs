using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Meebey.SmartIrc4net;

namespace IRC
{
	public class u
	{
		public static void umcf (string [] args, int lnth, string channel, string nick, IrcClient irc)
		{
			Console.WriteLine("U command");

            if (lnth == 2)
            {
                irc.SendMessage(SendType.Message, channel, string.Format("{0}: http://u.mcf.li/{1}", nick, args[1]));
            }
            else if (lnth == 3)
            {
                irc.SendMessage(SendType.Message, channel, string.Format("{0}: http://u.mcf.li/{1}/{2}", nick, args[1], args[2]));
            }
            else 
            {
                irc.SendMessage(SendType.Message, channel, string.Format("{0}, List of options: [profile, posts, topics, warnings, videos, friends, pm, names, admin, edit, modcp, validate, warn, suspend, iphistory]", nick));
            }
		}
	}
}

