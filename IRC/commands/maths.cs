using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Meebey.SmartIrc4net;

namespace IRC
{
    class maths
    {
        public static void calc(string[] args, int lnth, string channel, string nick, IrcClient irc)
        {
            /*
             * For future reference, this is how this method works.
             * 1. Checks that the arguments have at least 2 operands and a operator.
             * 2. Finds out the first operator (args[2]).
             * 3. Takes the first and second operator and does that operation and puts that in the varible result, this is on the first calc only.
             * 4. The for loops if there is more than 1 calc to do, it increments until it is more than lnth - 1 because of arrays beggining from 0.
             * 5. The second calc just does that operation to result and the second number.
             */
            if (lnth >= 4)
            {
                double result = 0;
                double workingnum;
                double workingnum2;
                bool first_calc = true;

                for (int increment = 2; increment <= lnth - 1; increment = increment + 2)
                {
                    try
                    {
                        /*
                         * Debug lines
                        
                         * Console.WriteLine(increment);
                         * Console.WriteLine(increment - 1);
                         * Console.WriteLine(increment + 1);
                         */
                        switch (args[increment])
                        {
                            case "+":
                                workingnum = double.Parse(args[increment - 1]);
                                workingnum2 = double.Parse(args[increment + 1]);
                                if (first_calc == true)
                                {
                                    result = workingnum + workingnum2;
                                    first_calc = false;
                                }
                                else
                                {
                                    result = result + workingnum2;
                                }
                                break;

                            case "-":
                                workingnum = double.Parse(args[increment - 1]);
                                workingnum2 = double.Parse(args[increment + 1]);
                                if (first_calc == true)
                                {
                                    result = workingnum - workingnum2;
                                    first_calc = false;
                                }
                                else
                                {
                                    result = result - workingnum2;
                                }
                                break;

                            case "/":
                                workingnum = double.Parse(args[increment - 1]);
                                workingnum2 = double.Parse(args[increment + 1]);
                                if (first_calc == true)
                                {
                                    result = workingnum / workingnum2;
                                    first_calc = false;
                                }
                                else
                                {
                                    result = result / workingnum2;
                                }
                                break;

                            case "*":
                                workingnum = double.Parse(args[increment - 1]);
                                workingnum2 = double.Parse(args[increment + 1]);
                                if (first_calc == true)
                                {
                                    result = workingnum * workingnum2;
                                    first_calc = false;
                                }
                                else
                                {
                                    result = result * workingnum2;
                                }
                                break;

                            case "**":
                                if (first_calc == true)
                                {
                                    workingnum = double.Parse(args[increment - 1]);
                                    workingnum2 = double.Parse(args[increment + 1]);
                                    result = Math.Pow(workingnum, workingnum2);
                                }
                                else
                                {
                                    irc.SendMessage(SendType.Message, channel, "Only the first calculation can be powered, for now", Priority.High);
                                    increment = lnth - 1;
                                }
                                break;

                            case "%":
                                if (first_calc == true)
                                {
                                    workingnum = double.Parse(args[increment - 1]);
                                    workingnum2 = double.Parse(args[increment + 1]);
                                    result = workingnum % workingnum2;
                                }
                                else
                                {
                                    irc.SendMessage(SendType.Message, channel, "Only the first calculation can be modulus, for now", Priority.High);
                                    increment = lnth - 1;
                                }
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        Nimbot.msgcolours(Nimbot.msglevel.critcial, "ERROR");
                        Console.WriteLine(e.Message);
                    }
                }
                string str_result = Convert.ToString(result);
                irc.SendMessage(SendType.Message, channel, str_result, Priority.High);
            }
            else
            {
                irc.SendMessage(SendType.Message, channel, "Need at least 2 operands and 1 operator", Priority.High);
            }
        }
    }
}
