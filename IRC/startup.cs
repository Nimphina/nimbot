using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRC
{
    class startup
    {
        public static void start (string version)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("");
            Console.WriteLine("    _   __ _             __            __");
            Console.WriteLine("   / | / /(_)____ ___   / /_   ____   / /_");
            Console.WriteLine("  /  |/ // // __ `__ \\ / __ \\ / __ \\ / __/");
            Console.WriteLine(" / /|  // // / / / / // /_/ // /_/ // /_");
            Console.WriteLine("/_/ |_//_//_/ /_/ /_//_.___/ \\____/ \\__/");
            Console.ResetColor();
            Console.WriteLine("      Ver: {0} written by Nimphina", version);
            Console.WriteLine("");

        }
    }
}
