using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Meebey.SmartIrc4net;

namespace IRC
{
    public class addition
    {
        public static void add(string[] args, int lnth, string channel, string nick, IrcClient irc)
        {
            try
            {
                double[] operands = new double[lnth];
                double noresult = 0;
                if (lnth >= 3)
                {
                    for (int i = 1; i < lnth; i++)
                    {
                        double workingnum = double.Parse(args[i]);
                        operands[i] = workingnum;
                        Console.WriteLine(operands[i]);

                        noresult = noresult + operands[i];
                    }

                    string result = Convert.ToString(noresult);
                    irc.SendMessage(SendType.Message, channel, result, Priority.High);

                    if (result == "69")
                    {
                        irc.SendMessage(SendType.Message, channel, "Heh heh, 69, hehe", Priority.High);
                    }
                }
                else
                {
                    irc.SendMessage(SendType.Message, channel, "placholder message", Priority.High);
                }
            }
            catch (Exception e)
            {
                irc.SendMessage(SendType.Message, channel, e.Message, Priority.High);
            }
        }
    }
}

