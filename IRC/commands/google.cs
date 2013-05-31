using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Meebey.SmartIrc4net;
using Google.API.Search;

namespace IRC
{
    class google
    {
        public static void search(string search_terms, string channel, string nick, IrcClient irc)
        {
            GwebSearchClient client = new GwebSearchClient("");
            IList<IWebResult> results = client.Search(search_terms, 2);
            foreach (IWebResult result in results)
            {
                irc.SendMessage(SendType.Message, channel, string.Format("{0}: {1} => {2}", nick, result.Title, result.Url), Priority.High);
            }  
        }
    }
}
