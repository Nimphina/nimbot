using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Meebey.SmartIrc4net;

namespace IRC
{
    class bcommands
    {

        public static void bc (string botop, string channel, string nick, string message, IrcClient irc){
            
     
            string[] args = message.TrimEnd().Split(' ');
            Console.WriteLine("A command was used!");
            int lnth = args.Length;
            string switchmess = args[0];

            switch (switchmess)
            {
                case "u":
                    Console.WriteLine("U command");
               
                    if (lnth == 2)
                    {
                        irc.SendMessage(SendType.Message, channel, string.Format("{0}: http://u.mcf.li/{1}", nick, args[1]));
                    }
                    if (lnth == 3)
                    {
                        irc.SendMessage(SendType.Message, channel, string.Format("{0}: http://u.mcf.li/{1}/{2}", nick, args[1], args[2]));
                    }
                    else
                    {
                        irc.SendMessage(SendType.Message, channel, string.Format("{0}, List of options: [profile, posts, topics, warnings, videos, friends, pm, names, admin, edit, modcp, validate, warn, suspend, iphistory]", nick));
                    }
                    break; 


                case "Ping":
                case "ping":
                    Console.WriteLine("ping command");
                    irc.SendMessage(SendType.Message, channel, "pong", Priority.High);
                    break;

                case "info":
                    irc.SendMessage(SendType.Message, channel, "I am a shitty little bot created by Nimphina using the smartirc4net lib", Priority.High);
                    break;

                case "quit":
                    if (nick == botop)
                    {
                        Console.WriteLine("Quit command issed by {0}", nick);
                        irc.SendMessage(SendType.Message, channel, "Qwitting", Priority.High);
                        Environment.Exit(0);
                    }
                    else
                    {
                        irc.SendMessage(SendType.Message, channel, "You are not allowed to issue that command :C", Priority.High);
                        Console.WriteLine("Unauthorized user using quit, suggesting immediate extermination");
                    }
                    break;

                case "Hello":
                case "hello":
                case "hi":
                    irc.SendMessage(SendType.Message, channel, "Hello, " + nick, Priority.High);
                    break;
                
            }
        }
    }
}
