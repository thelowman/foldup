using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace foldup
{
    class Program
    {
        /// <summary>
        /// Loads the configuration.json file and will either show the available options
        /// or run the backups specified by the command line options.  If the configuration
        /// file is not present this will display some information on where and how to 
        /// create one.
        /// </summary>
        /// <param name="args">
        /// Can be any number of the configured backups.  The option "--all" will attempt to
        /// run all backups from the configuration file.  No arguments will display help information.
        /// </param>
        static void Main(string[] args)
        {
            if (args.Length == 0) Help.General();

            Configuration configuration = null;
            try { configuration = new Configuration(); }
            catch (Exception ex)
            {
                Log.Write("Error:", ConsoleColor.White, ConsoleColor.Red);
                Log.WriteLine(" " + ex.Message);
                Exit(1);
            }

            if (args.Length == 0)
            {
                Log.WriteLine();
                if (configuration.Backups.Count() == 1)
                    Log.WriteLine(configuration.Backups.Count().ToString() + " backup available");
                else
                    Log.WriteLine(configuration.Backups.Count().ToString() + " backups available");

                // Present the options to the user.
                foreach (ConfigurationSection section in configuration.Backups)
                {
                    Log.WriteLine();
                    Help.Usage(section);
                }

                Log.WriteLine();
                Log.Write(" --all", ConsoleColor.White);
                Help.tab(20);
                Log.WriteLine("Performs all available backups.");

                Exit(0);
            }


            // Check the arguments
            bool loggedStart = false;
            foreach (ConfigurationSection section in configuration.Backups)
            {
                if (args.Contains("--" + section.title) || args.Contains("--all"))
                {
                    if (!loggedStart)
                    {
                        Log.Add("Backup Process Started At: " + DateTime.Now.ToString());
                        loggedStart = true;
                    }
                    Log.Add("---------------------------------------------------------");
                    Log.Add(section.title);
                    Log.Add(section.description);
                    Log.Add("---------------------------------------------------------");
                    Foldup.BackupSrcFolder(section.source, section.dest, section.ignoreFolders, "");
                }
            }
            Exit(0);
        }



        /// <summary>
        /// Ends the program.
        /// If a debug session is in progress this will prompt for a key press
        /// to give the developer time to see the console before closing.
        /// </summary>
        /// <param name="code">
        /// Integer value for the exit code (Default is 0)
        /// </param>
        static void Exit(int code = 0)
        {
#if DEBUG
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Debug session in progress, press any key to exit.");
            Console.ReadKey();
#endif
            Environment.Exit(code);
        }
    }
}
