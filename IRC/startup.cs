﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace IRC
{
    class startup
    {
        public static void stage1(string version)
        {
            Random rand = new Random();
            int random = rand.Next(0, 7);
            switch (random)
            {
                case 1:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case 2:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case 3:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case 4:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case 5:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case 6:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                case 7:
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
            }

            Console.WriteLine("");
            Console.WriteLine("                      _   __ _             __            __");
            Console.WriteLine("                     / | / /(_)____ ___   / /_   ____   / /_");
            Console.WriteLine("                    /  |/ // // __ `__ \\ / __ \\ / __ \\ / __/");
            Console.WriteLine("                   / /|  // // / / / / // /_/ // /_/ // /_");
            Console.WriteLine("                  /_/ |_//_//_/ /_/ /_//_.___/ \\____/ \\__/");

            Console.WriteLine("                      Ver: {0} written by Nimphina", version);
            Console.WriteLine("                 Reality cuts like a knife");
            Console.WriteLine("");
            // Console.WriteLine("---------------------------------------------------------------------------------");
            Console.ResetColor();

        }
    }
}
