using Meebey.SmartIrc4net;
using System;
using System.Collections.Generic;
using System.Text;

class  ClientDemo
{
    public static IrcClient irc = new  IrcClient();
    public string server = "irc.esper.net";
    private int port = 6667;
	private string channel = "#nimphina";
	public const string botop = "Nimphina";
	public const string leavecommand = "LEAVEUS";
	public const string opsymbol = "*";
	public const string version = "dev-1.0.2";

    public static  void Main()
    {
        ClientDemo demo = new  ClientDemo();
    }

    public ClientDemo()
    {
        irc.OnConnected += new EventHandler(OnConnected);
		irc.OnConnecting += new EventHandler(OnConnecting);
		irc.OnPing += new  PingEventHandler(OnPing);
		irc.OnDisconnected += new  EventHandler(OnDisconnected);
		irc.OnChannelMessage += new  IrcEventHandler(OnChannelMessage);

        try
        {
            irc.Connect(server, port);
        }
        catch (Exception e)
        {
            Console.Write("Failed to connect:n"+ e.Message);
            Console.ReadKey();
        }
    }

	void OnConnecting (object sender, EventArgs e)
	{
		Console.WriteLine("Starting Nimbot version: {0} " , version);
		Console.WriteLine("Botop: {0}, Command char: {1}," , botop, opsymbol);
		Console.WriteLine(" ");
		Console.WriteLine("Connecting to server {0} on port {1}." , server, port );
	}

    void OnConnected (object sender, EventArgs e)
	{
		Console.WriteLine ("Connected to {0}.", server);
		irc.SendMessage (SendType.Message, channel, "Connected to: " + server + " Bot op is: " + botop , Priority.BelowMedium);

		irc.Login ("Nimbot", "Nim-bot", 0, "Nimbot");
		irc.RfcJoin (channel);
		Console.WriteLine ("Joining {0}.", channel);
		irc.Listen (true);
    }

	void OnChannelMessage (object sender, IrcEventArgs e)
	{
		Console.WriteLine (e.Data.Type + ":");
		Console.WriteLine ("(" + e.Data.Channel + ") <" + e.Data.Nick + "> " + e.Data.Message);

		if (e.Data.Message.StartsWith (opsymbol)) {

			Console.WriteLine("Got command!");
			switch(e.Data.Message){
			
			case opsymbol:
				irc.SendMessage (SendType.Message, channel, "Need to actually enter a command you know.", Priority.High);
				break;

			case opsymbol + "info":
			irc.SendMessage (SendType.Message, channel, "I am a shitty little bot created by Nimphina using the smartirc4net lib", Priority.High);
				break;

			case opsymbol + "botquit":
				if (e.Data.Nick == botop) {
					irc.SendMessage (SendType.Message, channel, "Qwitting", Priority.High);
					Environment.Exit (0);
				}else{
					irc.SendMessage (SendType.Message, channel, "You are not allowed to issue that command :C", Priority.High);
					Console.WriteLine ("Unauthorized user using quit, suggesting immediate extermination");
				}
				break;

			case opsymbol + "Hello":
			case opsymbol + "hello":
			case opsymbol + "hi":
				irc.SendMessage (SendType.Message, channel, "Hello, " + e.Data.Nick, Priority.High);
				break;

			}

		}

		if (e.Data.Nick == "Ralph") {
			irc.SendMessage (SendType.Message, channel, "I hate Ralph and he hates me", Priority.High);
			Console.WriteLine ("RALPH SAID SHIT");
		}

		if (e.Data.Message == "What is love?") {

			irc.SendMessage (SendType.Message, channel, "Baby don't hurt me", Priority.High);
		}
    }
	 
	void OnDisconnected(object sender, EventArgs e)
    {
        Console.WriteLine("Disconnected.");
    }

	void OnPing(object sender, PingEventArgs e)
    {
        Console.WriteLine("Responded to ping at {0}.",DateTime.Now.ToShortTimeString());
    }
}