using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRC
{
    class Commands
    {
        public static int CommandHandler(string messageData, string channel, string nick, Dictionary<string, string> config_options, Nimbot nimbot)
        {

            messageData = messageData.TrimStart(new Char[] { Convert.ToChar(config_options["commandchar"]) });

            string[] messageArgs = messageData.TrimEnd().Split(' ');
            int messageLength = messageArgs.Length;


            switch (messageArgs[0].ToLower())
            {

                case "g":
                case "google":
                    if (messageLength > 1)
                    {
                        messageData = messageData.Replace(messageArgs[0], "");
                        messageData = messageData.TrimStart(new char[] { ' ' });
                        google.search(messageData, channel, nick);
                    }
                    else
                    {
                      Nimbot.BotSay("Google: Takes your query and searches Google with it, displays two results.", channel);
                    }
                    break;

                case "ping":

                    Nimbot.BotSay("pong", channel);

                    break;

                case "info":
                    Nimbot.BotSay("I am a bot written by Nimphina, very messily written using the SmartIrc4net lib for irc stuff. Sauce code: http://github.com/Nimphina/Nimbot", channel);
                    break;

                case "gettime":
                    Nimbot.BotSay(string.Format("Bot server time is {0}", DateTime.Now.ToLongTimeString()), channel);
                    break;

                case "say":
                    messageData = "~" + messageData; //It's a shitty way to just remove the command from the string but if anyone has a better way...
                    messageData = messageData.Replace("~say", "");
                    messageData = messageData.TrimStart(new char[] { ' ' });
                    Nimbot.BotSay(messageData, channel);
                    break;
            }


            return 1;
        }
    }
}
