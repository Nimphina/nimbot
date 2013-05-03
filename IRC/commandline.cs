using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRC
{
    class commandline
    {
        public static void cmd()
        {
            Console.WriteLine("Enter a message: ");
            string consolemessage = Console.ReadLine();

            if (consolemessage == "/stop")
            {
                irc.SendMessage(SendType.Message, channel, string.Format("{0}, Messages from console disabled", botop), Priority.High);
                break;
            }
            irc.SendMessage(SendType.Message, channel, string.Format("{0}", consolemessage), Priority.High);
        }
    }
}
