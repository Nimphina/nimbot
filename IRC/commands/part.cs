using System;
using System.Collections.Generic;
using System.Text;
using Meebey.SmartIrc4net;
using System.IO;

namespace IRC
{
	public class part
	{
		public static void channelremove (string channel)
		{
			StringBuilder sb = new StringBuilder();
			StreamWriter writer = new StreamWriter("channel.list", true);

            string line;
            bool remove = true;
              
            using (System.IO.StreamReader file = new System.IO.StreamReader("channel.list"))
            {
                while ((line = file.ReadLine()) != null)
                {
                    if (line.Contains(channel))
                    {
						Nimbot.console_messages("info", "ATTENTION");
						Console.WriteLine("Attempting to remove channel.");
						writer.WriteLine("");
                    }
                }
            }
		}
	}
}

