using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IRC
{
    class bottools
    {
        public enum msglevel
        {
            ok,
            warning,
            critcial,
            info,
            message,
            server,
            channel
        }
        public static void consolemsg(msglevel state, string message, string alertmsg)
        {
            //Console.WriteLine(alertmsg.Length);
            if (alertmsg.Length < 5)
            {
                for (int i = 1; i <= 5 - alertmsg.Length; i++ )
                {
                    alertmsg = alertmsg + "-";
                }
            }
            Console.Write("[");
            if (state == msglevel.ok)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(alertmsg);
                Console.ResetColor();
            }
            else if (state == msglevel.warning)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(alertmsg);
                Console.ResetColor();
            }
            else if (state == msglevel.critcial)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(alertmsg);
                Console.ResetColor();
            }
            else if (state == msglevel.info)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write(alertmsg);
                Console.ResetColor();
            }
            else if (state == msglevel.message)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write(alertmsg);
                Console.ResetColor();
            }
            else if (state == msglevel.server)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write(alertmsg);
                Console.ResetColor();
            }
            else if (state == msglevel.channel)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(alertmsg);
                Console.ResetColor();
            }
            Console.Write("]   ");
            Console.WriteLine(message);
        }

        //Config get method, supply a file name and options 
        public static Dictionary<string, string> configparser(string config_file, string config_opts) 
        {
            if (File.Exists(config_file))
            {
                try
                {
                    StreamReader configreader = new StreamReader(config_file); //Open file for reading

                    Dictionary<string, string> config_options = new Dictionary<string, string>(); //Dict for our options

                    //Declare relevant vars
                    string file_line;
                    string option_data;
                    bool read_data = false;
                    bool found;

                    foreach (string word in config_opts.Split())
                    {
                        //reset variables
                        option_data = "";
                        found = false;
                        //Return to begining of file
                        configreader.DiscardBufferedData();
                        configreader.BaseStream.Seek(0, SeekOrigin.Begin);
                        configreader.BaseStream.Position = 0;

                        while (configreader.EndOfStream != true)
                        {
                            file_line = configreader.ReadLine(); //Shoud be in "word={option}" format
                            if (file_line.Contains(word) && file_line.Contains("{") && file_line.Contains("}"))
                            {
                                foreach (char letter in file_line)
                                {
                                    //Start reading from { till }
                                    if (letter == '{')
                                    {
                                        read_data = true;
                                    }
                                    else if (letter == '}')
                                    {
                                        read_data = false;
                                    }
                                    if (read_data && letter != '{')
                                    {
                                        option_data = option_data + letter; //Make string from chars we want
                                    }
                                }
                                found = true;
                                config_options.Add(word, option_data);
                                //Console.WriteLine(option_data);
                            }
                        }
                        if (found == false)
                        {
                            Console.WriteLine("Option {0} was not found or was in the incorrect format, please enter it now and update the config file accordingly", word);
                            option_data = Console.ReadLine();
                            config_options.Add(word, option_data);
                        }
                    }

                    configreader.Close();
                    return config_options; //for when we have finished writing this thing
                }
                catch (Exception e)
                {
                   // Console.WriteLine(e.Message);
                    Dictionary<string, string> failed_config = new Dictionary<string, string>();

                    failed_config.Add("failed", e.Message);

                    return failed_config;
                }
            }
            else
            {
                Console.WriteLine("{0} does not seem to exit, do you want to create it? [Y/n]", config_file);
                string user_reps = Console.ReadLine();
                if (String.IsNullOrEmpty(user_reps) || user_reps.ToLower().StartsWith("y"))
                {
                    Console.WriteLine("Creating a new config file.");

                    StreamWriter new_conf_write = new StreamWriter(config_file);

                    string opts;

                    foreach (string word in config_opts.Split(' '))
                    {
                        Console.WriteLine("Option for {0}", word);
                        opts = Console.ReadLine();
                        new_conf_write.WriteLine(word + "={" + opts + "}");
                    }
                    new_conf_write.Close();

                    Console.WriteLine("Config file created");

                    Dictionary<string, string> new_config = new Dictionary<string, string>();

                    new_config.Add("created", "created"); //Main program should decide what to do next.

                    return new_config;
                }
                else if (user_reps.ToLower().StartsWith("n"))
                {
                    Dictionary<string, string> failed_config = new Dictionary<string, string>();

                    failed_config.Add("failed", "Requested not to create new config file");

                    return failed_config;
                }
                else
                {
                    Dictionary<string, string> failed_config = new Dictionary<string, string>();

                    failed_config.Add("failed", "Unknown");

                    return failed_config;
                }
            }
        }
    }
}
