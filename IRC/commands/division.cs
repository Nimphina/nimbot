using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Meebey.SmartIrc4net;

namespace IRC
{
	public class division
	{
		public static void divide (string[] args, int lnth, string channel, string nick, IrcClient irc)
		{
            Console.WriteLine("{0}, called division command", nick);
			try 
			{
				double[] operands = new double[lnth];
				double noresult = 1;
				if (lnth >= 3) 
				{
					for (int i = 1; i < lnth; i++) 
					{
						double workingnum = double.Parse (args [i]);
						operands [i] = workingnum;
						Console.WriteLine (operands [i]);

						if (i == 1)
						{
							noresult = operands[1];
						}
						else
						{
						noresult = noresult / operands [i];
						}
					}

					string result = Convert.ToString (noresult);
					irc.SendMessage (SendType.Message, channel, result, Priority.High);

					if (result == "69") 
					{
						irc.SendMessage (SendType.Message, channel, "Heh heh, 69, hehe", Priority.High);
					}
				} 
				else 
				{
					irc.SendMessage (SendType.Message, channel, "placholder message", Priority.High);
				}
			} 
			catch (Exception e) 
			{
				irc.SendMessage (SendType.Message, channel, e.Message, Priority.High);
			}
		}
	}
}

