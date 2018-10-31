using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace foldup
{
    public static class Help
    {
        public static void tab(int tabTo) { Console.CursorLeft = tabTo; }

        public static void General()
        {
            Log.Write(GeneralInfo);
        }

        public static void ConfigurationFile()
        {
            Log.Write(ConfigFileInstructions);
            Log.Write(ConfigFileExample, ConsoleColor.DarkGray);
            Log.WriteLine();
            Log.Write(ConfigSectionInstructions);
            Log.Write(ConfigSectionExample, ConsoleColor.DarkGray);
        }
        public static void ConfigurationFileSection()
        {
            Log.Write(ConfigSectionInstructions);
            Log.Write(ConfigSectionExample, ConsoleColor.DarkGray);
        }
        public static void Usage(ConfigurationSection section)
        {
            List<string> notes = new List<string>();

            Log.Write(" --" + section.title, ConsoleColor.White);
            tab(20);
            Log.WriteLine(section.description);

            tab(10);
            Log.Write("from:");
            tab(20);
            if (section.source.Exists) Log.WriteLine(section.source.ToString(), ConsoleColor.Green);
            else
            {
                Log.WriteLine(section.source.ToString(), ConsoleColor.Red);
                notes.Add("The specified source directory does not exist.  The process will fail.");
            }

            tab(10);
            Log.Write("to:");
            tab(20);
            if (section.dest.Exists) Log.WriteLine(section.dest.ToString(), ConsoleColor.Green);
            else
            {
                Log.WriteLine(section.dest.ToString(), ConsoleColor.Yellow);
                notes.Add("The specified destination directory does not exist.  Will attempt to create it.");
            }

            tab(10);
            Log.Write("ignore:");
            if (section.ignoreFolders.Length == 0)
            {
                tab(20);
                Log.WriteLine("none");
            }
            else
            {
                tab(20);
                for (int i = 0; i < section.ignoreFolders.Length; i++)
                {
                    Log.Write(section.ignoreFolders[i]);
                    if (i < section.ignoreFolders.Length - 1)
                        Log.Write(", ");
                }
                Log.WriteLine();
            }

            foreach (string note in notes)
            {
                tab(10);
                Log.WriteLine(note);
            }
            //tab(10);
            //Log.Write("Use ");
            //Log.Write("--" + section.title, ConsoleColor.White);
            //Log.WriteLine(" to run this from the command line.");
        }


        private static string GeneralInfo = @"
Foldup is a very simple folder syncronization program.  It uses each file's
modified date to only copy files from the source that are newer than the same
file in the destination folder.

* Hidden and read-only files are also copied.
* Specific sub-folders can be skipped based on their name.
* Files and folders deleted from the source will be deleted from the destination.

";

        private static string ConfigFileInstructions = @"
The program is controlled by a file named ""configuration.json"" located in the
same folder as the executable program.  The content of the file is as follows:
";
        private static string ConfigFileExample = @"
[ one or more backup configuration sections (see below) ]

";


        private static string ConfigSectionInstructions = @"
Configuration sections specify individual folders to back up.
The title of each section becomes a command-line argument that executes that particular backup.
";
        private static string ConfigSectionExample = @"
{
  ""title"": ""myBackup"", (required - no empty spaces)
  ""description"": ""Backup Server Source..."",
  ""source"": ""C:\\path\\to\\source\\directory"", (required - use double backslash marks)
  ""dest"": ""C:\\path\\to\\destination\\directory"", (required - use double backslash marks)
  ""ignoreFolders"": [
    ""SKIP"",
    ""IGNORE"",
    "".GIT"",
    ""ETC""
  ]
}
";
    }
}
