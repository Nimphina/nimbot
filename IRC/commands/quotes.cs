using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Meebey.SmartIrc4net;

namespace IRC
{
    class quotes
    {
        public static void quotegetter(string channel, IrcClient irc)
        {
            try
            {
                Random rand = new Random();
                
                StreamReader quoter = new StreamReader("quotes");

                int quote_ammount = 1;
                
                while (quoter.EndOfStream == false)
                {
                    quoter.ReadLine();
                    quote_ammount++;
                }
                int random = rand.Next(1, quote_ammount + 1); 
                quoter.Close();
                StreamReader quotereader = new StreamReader("quotes");
                for (int i = 0; i < random; i++)
                {
                    quotereader.ReadLine();
                }
                string quote = quotereader.ReadLine();
                irc.SendMessage(SendType.Message, channel, quote, Priority.High);
                quotereader.Close();
            }
            catch (FileNotFoundException)
            {
                Nimbot.msgcolours(Nimbot.msglevel.warning, "WARNING");
                Console.WriteLine("Quote file not found");
            }
        }
    }
}
