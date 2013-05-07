using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Linq;
using System.Text;
using Meebey.SmartIrc4net;

namespace IRC
{
    class bcommands
    {

        public static void bc(string botop, string channel, string nick, string message, string server, int port, string version, ref string botname, double timestart, IrcClient irc)
        {

            string[] args = message.TrimEnd().Split(' ');
            Console.WriteLine("Command char detected!");
            int lnth = args.Length;
            string command_check = args[0];

            switch (command_check)
            {
                case "U":
                case "u":
                    u.umcf(args, lnth, channel, nick, irc);
                    break;
                
                case "Reddit":
                case "reddit":
                    redditclass.reddit(args, lnth, channel, nick, irc);
                    break;
                
                case "Add":
                case "add":
                    addition.add(args, lnth, channel, nick, irc);
                    break;
                
                case "Minus":
                case "minus":
                    subtraction.minus(args, lnth, channel, nick, irc);
                    break;
                
                case "Multiply":
                case "multiply":
                    multiplication.multi(args, lnth, channel, nick, irc);
                    break;
                
                case "Divide":
                case "divide":
                    division.divide(args, lnth, channel, nick, irc);
                    break;

                case "bunnywaffle":
                    bunny.waffle(channel, irc);
                    break;

                //All hardcoded, non class commands
                case "Ping":
                case "ping":
                    Console.WriteLine("ping command");
                    irc.SendMessage(SendType.Message, string.Format(channel, "#botspam"), "pong", Priority.High);
                    break;

                case "info":
                    irc.SendMessage(SendType.Message, channel, "I am a bot written by Nimphina, very messily written using the SmartIrc4net lib for irc stuff. Sauce code: http://github.com/Nimphina/Nimbot", Priority.High);
                    break;

                case "version":
                    irc.SendMessage(SendType.Message, channel, version, Priority.High);
                    Console.WriteLine(version);
                    break;

                case "gettime":
                    irc.SendMessage(SendType.Message, channel, string.Format("Bot server time is {0}", DateTime.Now.ToLongTimeString()), Priority.High);
                    break;

                case "uptime":
                    string timenow_st = DateTime.Now.ToShortTimeString();
                    bool pmtrue = timenow_st.Contains("PM");

                    timenow_st = timenow_st.Replace("AM", "");
                    timenow_st = timenow_st.Replace("PM", "");
                    timenow_st = timenow_st.Replace(":", "");
                    double timenow = double.Parse(timenow_st);

                    if (pmtrue == true)
                    {
                        timenow += 1200;
                    }

                    double timenow_mins = 0;
                    while (timenow >= 100)
                    {
                        timenow -= 100;
                        timenow_mins += 60;
                    }
                    timenow = timenow + timenow_mins;

                    double minutes = timenow - timestart;
                    double hour = 0;
                    while (minutes >= 60)
                    {
                        minutes -= 60;
                        hour++;
                    }

                    irc.SendMessage(SendType.Message, channel, string.Format("{0} Hours {1} Minutes",hour, minutes), Priority.High);
                    break;
		
				case "q":
                case "quit":
                    if (nick == botop)
                    {
                        Console.WriteLine("Quit command issed by {0}", nick);
                        irc.SendMessage(SendType.Message, channel, "Qwitting", Priority.High);
                        
                        Environment.Exit(0);
                    }
                    else
                    {
                        Console.WriteLine("Quit command issed by {0}", nick);
                        irc.SendMessage(SendType.Message, channel, "You are not allowed to issue that command :C", Priority.High);
                        Console.WriteLine("Unauthorized user using quit, suggesting immediate extermination");
                    }
                    break;

                case "Hello":
                case "hello":
                case "hi":
                    irc.SendMessage(SendType.Message, channel, "Hello, " + nick, Priority.High);
                    break;

                case "join":
                    if (args[1] != "#fofftopic")
                    {
                        irc.RfcJoin(args[1]);
                        StreamWriter writer = new StreamWriter("channel.list", true);
                        writer.WriteLine(args[1]);
                        writer.Close();
                    }
                    else
                    {
                        irc.SendMessage(SendType.Message, channel, "Joining #fofftopic is not allowed!", Priority.High);
                    }
                    break;

                case "part":
					if (lnth == 1)
					{
						irc.RfcPart(channel);
					}
                	else if (lnth == 2)
                	{
                   		 irc.RfcPart(args[1]);
                	}
                	else if (lnth >= 3)
                	{
                		irc.RfcPart(args[1] + args[2]);	
                	}
                    break;

                case "nick":
					if (nick == botop)
					{
	                    irc.RfcNick(args[1]);
						botname = args[1];
					}
					else
					{
						irc.SendMessage(SendType.Message, channel, "You are not allowed to perform that command!", Priority.High);
					}
                    break;

                //Bot op only commands
                case "op":
                    opitems.op(channel, botop, nick, args, lnth, irc);
                    break;

                case "deop":
                    opitems.deop(channel, botop, nick, args, lnth, irc);
                    break;

                case "voice":
                    opitems.voice(channel, botop, nick, args, lnth, irc);
                    break;
                case "devoice":
                    opitems.devoice(channel, botop, nick, args, lnth, irc);
                    break;

                case "kick":
                    opitems.kick(channel, botop, nick, args, lnth, irc);
                    break;

				case "restart": 
					if (nick == botop)
					{
						try{
						//System.Diagnostics.Process.Start("Nimbot.sh");
						System.Diagnostics.Process.Start("Nimbot.bat");
						Environment.Exit(0);
						}catch(Exception f){
						    Console.WriteLine(f.Message);
						}
					}
				break;
            }
        }
    }
}
