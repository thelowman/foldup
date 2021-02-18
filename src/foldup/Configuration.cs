using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace foldup
{
    public class Configuration
    {
        /// <summary>
        /// Copied (almost) verbatim from
        /// https://stackoverflow.com/questions/52797/how-do-i-get-the-path-of-the-assembly-the-code-is-in
        /// </summary>
        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return System.IO.Path.GetDirectoryName(path);
            }
        }

        /// <summary>
        /// A generic list of configured backups.  If none exist this
        /// list will be empty.
        /// </summary>
        public List<ConfigurationSection> Backups = new List<ConfigurationSection>();

        /// <summary>
        /// Non fatal exceptions are listed here.
        /// These will not stop program execution but will stop individual
        /// backup sections from being loaded from the configuration file.
        /// </summary>
        public List<Exception> Errors = new List<Exception>();

        /// <summary>
        /// Attempts to read available backup configurations from "configuration.json"
        /// located in the executable directory.
        /// </summary>
        public Configuration()
        {
            string cFile;
            ConfigFileSection[] configFile;

            // Read the configuration file
            try
            {
                cFile = File.ReadAllText(AssemblyDirectory + "\\configuration.json");
            }
            catch (FileNotFoundException notFoundEx)
            {
                Help.ConfigurationFile();
                throw new Exception("The configuration.json file was not found.", notFoundEx);
            }
            catch (Exception ex)
            {
                Help.ConfigurationFile();
                throw new Exception("There was a general failure reading the configuration.json file.", ex);
            }



            // Parse the JSON configuration file
            Log.Write("Foldup is checking the configuration file... ");
            try
            {
                configFile = Newtonsoft.Json.JsonConvert.DeserializeObject<ConfigFileSection[]>(cFile);
            }
            catch (Exception nsEx)
            {
                Help.ConfigurationFile();
                throw new Exception("Unable to parse the configuration.json file.", nsEx);
            }


            

            // Validate the data in the configuration file
            if (configFile.Count() == 0)
            {
                Help.ConfigurationFile();
                throw new Exception("No \"backups\" section was found in the configuration file.");
            }



            // Build a new list of validated configuration sections
            List<ConfigFileSection> validSections = new List<ConfigFileSection>();
            // Build a list of invalid configuration sections for error reporting
            List<ConfigFileSection> invalidSections = new List<ConfigFileSection>();


            // sectionNumber is used for error reporting if a section does not have a title
            int sectionNumber = 1;
            foreach (ConfigFileSection section in configFile)
            {
                bool valid = true;
                if (string.IsNullOrWhiteSpace(section.title))
                {
                    section.title = "section " + sectionNumber.ToString();
                    Errors.Add(new Exception("Configuration section " + sectionNumber.ToString() + " has no title."));
                    valid = false;
                }
                // --all is a command line switch so it can't be a backup also
                if (section.title == "all")
                {
                    Errors.Add(new Exception("A configuration section cannot be named \"all\"."));
                    valid = false;
                }
                if (section.title.IndexOf(' ') > -1)
                {
                    Errors.Add(new Exception("Configuration section \"" + section.title + "\" cannot have spaces in its title."));
                    valid = false;
                }
                if (string.IsNullOrWhiteSpace(section.source))
                {
                    Errors.Add(new Exception("Configuration section \"" + section.title + "\" has no source folder specified."));
                    valid = false;
                }
                if (string.IsNullOrWhiteSpace(section.dest))
                {
                    Errors.Add(new Exception("Configuration section \"" + section.title + "\" has no destination folder specified."));
                    valid = false;
                }
                if (valid == true) validSections.Add(section);
                else invalidSections.Add(section);
                sectionNumber++;
            }



            // If errors were encountered list them.
            if (Errors.Count() > 0)
            {
                string errCount = "";
                errCount += Errors.Count().ToString();
                if (Errors.Count == 1) errCount += " error was";
                else errCount += " errors were";
                errCount += " encountered.";
                Log.WriteLine(errCount, ConsoleColor.Red);

                foreach (Exception ex in Errors)
                {
                    Log.WriteLine(ex.Message);
                }
                Log.WriteLine();
            }
            else Log.WriteLine("Seems fine.");



            // Are there any valid sections?
            if (validSections.Count() == 0)
            {
                // No valid sections so we can't possibly continue.
                Help.ConfigurationFileSection();
                throw new Exception("No valid backup configurations were found.");
            }
            // At least one good section exists so we can continue.
            if (invalidSections.Count() > 0)
            {
                string badSectionCount = invalidSections.Count().ToString();
                if (invalidSections.Count() == 1) badSectionCount += " section";
                else badSectionCount += " sections";
                badSectionCount += " could not be processed.";
                Log.Write(badSectionCount);
            }

            // Convert the JSON data into ConfigurationSections
            foreach (ConfigFileSection cFileSection in validSections)
            {
                ConfigurationSection section;
                try
                {
                    section = new ConfigurationSection(cFileSection);
                    this.Backups.Add(section);
                }
                catch (Exception configEx)
                {
                    throw new Exception("Error in configuration section \"" + cFileSection.title + "\"", configEx);
                }
            }
        }
    }

    public class ConfigurationSection
    {
        public string title;
        public string description;
        public DirectoryInfo source;
        public DirectoryInfo dest;
        public string[] ignoreFolders;

        public ConfigurationSection(ConfigFileSection jsonConfig)
        {
            this.title = jsonConfig.title;
            this.description = jsonConfig.description;
            try
            {
                this.source = getDirectoryInfo(jsonConfig.source);
            }
            catch (Exception sourceEx)
            {
                throw new Exception("Failed to initialize the source folder.", sourceEx);
            }
            try
            {
                this.dest = new DirectoryInfo(jsonConfig.dest);
            }
            catch (Exception destEx)
            {
                throw new Exception("Failed to initialize the destination folder.", destEx);
            }
            this.ignoreFolders = jsonConfig.ignoreFolders;
        }


        /// <summary>
        /// Creates a DirectoryInfo structure from the path provided and
        /// supplies meaningful error messages if the process fails.
        /// </summary>
        /// <param name="path">The path for which the DirectoryInfo object is needed.</param>
        /// <returns>DirectoryInfo</returns>
        private DirectoryInfo getDirectoryInfo(string path)
        {
            DirectoryInfo info;
            try { info = new DirectoryInfo(path); }
            catch (ArgumentNullException aEx)
            {
                throw new Exception("No path was specified.", aEx);
            }
            catch (System.Security.SecurityException sEx)
            {
                throw new Exception("Security settings prevent access to the \"" + path + "\" folder.", sEx);
            }
            catch (ArgumentException argEx)
            {
                throw new Exception("The path \"" + path + "\" contains invalid characters.", argEx);
            }
            catch (PathTooLongException pEx)
            {
                throw new Exception("The path is too long.", pEx);
            }

            return info;
        }
    }



    public class ConfigFile
    {
        public ConfigFileSection[] backups;
    }
    public class ConfigFileSection
    {
        public string title;
        public string description;
        public string source;
        public string dest;
        public string[] ignoreFolders;
    }
}
