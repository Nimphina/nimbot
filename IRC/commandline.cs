using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Meebey.SmartIrc4net;

namespace IRC
{
    class commandline
    {
        public static void cmd(IrcClient irc)
        {
			string channel = "#botspam";
            Console.WriteLine("Enter a message: ");
            string consolemessage = Console.ReadLine();
            irc.SendMessage(SendType.Message, channel, string.Format("{0}", consolemessage), Priority.High);
        }
    }
}
