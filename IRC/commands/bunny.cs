using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Meebey.SmartIrc4net;

namespace IRC
{
    public class bunny
    {
        public static void waffle(string channel, IrcClient irc)
        {
            irc.SendMessage(SendType.Message, channel, " |\\_/|", Priority.High);
            irc.SendMessage(SendType.Message, channel, "(^-^)", Priority.High);
            irc.SendMessage(SendType.Message, channel, "(>#<)", Priority.High);
            irc.SendMessage(SendType.Message, channel, " U U", Priority.High);
            irc.SendMessage(SendType.Message, channel, "Yay waffle bunny!", Priority.High);
        }
    }
}
