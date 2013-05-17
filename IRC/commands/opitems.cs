using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Meebey.SmartIrc4net;

namespace IRC
{
    class opitems
    {
        public static void op(string channel, string botop, string nick, string[] args, int lnth, IrcClient irc)
        {
            if (nick == botop)
            {
                if (lnth == 2)
                {
                    irc.Op(channel, args[1]);
                }
                else
                {
                    irc.SendMessage(SendType.Message, channel, "Who do you want to op?", Priority.High);
                }
            }
            else
            {
                irc.SendMessage(SendType.Message, channel, "You are not authorised to perform that command", Priority.High);
				Nimbot.msgcolours(IRC.Nimbot.msglevel.warning, "WARNING");
				Console.WriteLine("An unauthorised user attempted to use a op command.");
            }

        }
        public static void deop(string channel, string botop, string nick, string[] args, int lnth, IrcClient irc)
        {
            if (nick == botop)
            {
                if (lnth == 2)
                {
                    irc.Deop(channel, args[1]);
                }
                else
                {
                    irc.SendMessage(SendType.Message, channel, "Who do you want to deop?", Priority.High);
                }
            }
            else
            {
                irc.SendMessage(SendType.Message, channel, "You are not authorised to perform that command", Priority.High);
				Nimbot.msgcolours(IRC.Nimbot.msglevel.warning, "WARNING");
				Console.WriteLine("An unauthorised user attempted to use a op command.");

            }
        }
        public static void voice(string channel, string botop, string nick, string[] args, int lnth, IrcClient irc)
        {
            if (nick == botop)
            {
                if (lnth == 2)
                {
                    irc.Voice(channel, args[1]);
                }
                else
                {
                    irc.SendMessage(SendType.Message, channel, "Who do you want to voice?", Priority.High);
                }
            }
            else
            {
                irc.SendMessage(SendType.Message, channel, "You are not authorised to perform that command", Priority.High);
				Nimbot.msgcolours(IRC.Nimbot.msglevel.warning, "WARNING");
				Console.WriteLine("An unauthorised user attempted to use a op command.");
            }
        }
        public static void devoice(string channel, string botop, string nick, string[] args, int lnth, IrcClient irc)
        {
            if (nick == botop)
            {
                if (lnth == 2)
                {
                    irc.Devoice(channel, args[1]);
                }
                else
                {
                    irc.SendMessage(SendType.Message, channel, "Who do you want to devoice?", Priority.High);
                }
            }
            else
            {
                irc.SendMessage(SendType.Message, channel, "You are not authorised to perform that command", Priority.High);
				Nimbot.msgcolours(IRC.Nimbot.msglevel.warning, "WARNING");
				Console.WriteLine("An unauthorised user attempted to use a op command.");
            }
        }
        public static void kick(string channel, string botop, string nick, string[] args, int lnth, IrcClient irc)
        {
            if (nick == botop)
            {
                if (lnth == 2)
                {
                    irc.RfcKick(channel, args[1]);
                }
                else if (lnth == 3)
                {
                    irc.RfcKick(channel, args[1], args[2]);
                }
                else
                {
                    irc.SendMessage(SendType.Message, channel, "Who do you want to kick?", Priority.High);
                }
            }
            else
            {
                irc.SendMessage(SendType.Message, channel, "You are not authorised to perform that command", Priority.High);
				Nimbot.msgcolours(IRC.Nimbot.msglevel.warning, "WARNING");
				Console.WriteLine("An unauthorised user attempted to use a op command.");
            }
        }
    }
}
