using System;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Meebey.SmartIrc4net;

/*Changelog (starting ver dev 1.1.21)
 * dev-2.0.0:
 * Complete re-write of main functions.
 * Improved configuration
 * Implemented some basic functionality
 * Created a new class with some common methods
 * Added join/part and message commands to bot object
 * 
 * dev-1.1.21:
 * Commented the code a lot more, added changelog.
 * 
 * Todo:
 * Fix some stablity issues regarding the configuration and add proper exception handling 
 */

namespace IRC
{
   
    class main
    {
        public static void Main()
        {
            Console.Title = "Nimbot";
            
            Nimbot bot = new Nimbot();
            Console.ReadLine();
        }
    }
}
