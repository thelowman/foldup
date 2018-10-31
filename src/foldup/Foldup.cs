using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace foldup
{
    internal static class Foldup
    {
        /// <summary>
        /// Defines a standard indention for sub folders and files.
        /// </summary>
        const string indention = "  ";

        /// <summary>
        /// Backs up source folders recursively
        /// </summary>
        public static void BackupSrcFolder(
            DirectoryInfo source,
            DirectoryInfo dest,
            string[] ignoreFolders,
            string indent = indention)
        {
            Log.Add(indent + "Folder " + dest.Name, ConsoleColor.DarkGray, ConsoleColor.Black);
            if (!dest.Exists) dest.Create();

            UpdateFiles(source, dest, indent);

            DirectoryInfo[] srcSubs = source.GetDirectories();

            // delete any sub folders that aren't in the source
            DirectoryInfo[] destSubs = dest.GetDirectories();
            for (int d = 0; d < destSubs.Length; d++)
            {
                bool found = false;
                for (int s = 0; s < srcSubs.Length; s++)
                {
                    if (srcSubs[s].Name == destSubs[d].Name)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    try
                    {
                        Directory.Delete(destSubs[d].FullName, true);
                        Log.Add(indent + "Deleted folder " + destSubs[d].Name, ConsoleColor.Red, ConsoleColor.Black);
                    }
                    catch(Exception delEx)
                    {
                        string failedMsg = indent + indention +
                            "Failed to delete folder " +
                            destSubs[d].Name;
                        failedMsg += ": \"" + delEx.Message.Substring(0, delEx.Message.IndexOf("\r\n")) + "\"";
                        Log.Add(failedMsg, ConsoleColor.Red, ConsoleColor.Black);
                    }
                }
            }



            // process all folders within the source
            for (int i = 0; i < srcSubs.Length; i++)
            {
                //if (!ignoreFolders.Contains(srcSubs[i].Name.ToUpper()))
                if (!ignoreFolders.Contains(srcSubs[i].Name, new NameComparer()))
                {
                    DirectoryInfo destSub = new DirectoryInfo(dest.FullName + "\\" + srcSubs[i].Name);
                    BackupSrcFolder(srcSubs[i], destSub, ignoreFolders, indent + indention);
                }
            }
        }








        /// <summary>
        /// Updates the files in the destination with copies from the source
        /// if the source file is newer or if it doesn't exist.  Also deletes
        /// files from the destination if they have been deleted from the source
        /// </summary>
        static void UpdateFiles(
            DirectoryInfo source,
            DirectoryInfo dest,
            string indent = "  ")
        {
            FileInfo[] sourceFiles = source.GetFiles();
            FileInfo[] destFiles = dest.GetFiles();

            // delete file if not in source
            for (int d = 0; d < destFiles.Length; d++)
            {
                bool found = false;
                for (int s = 0; s < sourceFiles.Length; s++)
                {
                    if (sourceFiles[s].Name == destFiles[d].Name)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    Log.Add(indent + "Deleted file " + destFiles[d].Name, ConsoleColor.Red, ConsoleColor.Black);
                    File.Delete(destFiles[d].FullName);
                }
            }

            // now copy in the source if new or newer
            for (int s = 0; s < sourceFiles.Length; s++)
            {
                bool found = false;
                for (int d = 0; d < destFiles.Length; d++)
                {
                    if (destFiles[d].Name == sourceFiles[s].Name)
                    {
                        if (sourceFiles[s].LastWriteTime > destFiles[d].LastWriteTime)
                        {
                            Log.Add(indent + "Updated file " + sourceFiles[s].Name, ConsoleColor.Green, ConsoleColor.Black);
                            File.Copy(sourceFiles[s].FullName, destFiles[d].FullName, true);
                        }
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    Log.Add(indent + "Added file " + sourceFiles[s].Name, ConsoleColor.Yellow, ConsoleColor.Black);
                    File.Copy(sourceFiles[s].FullName, dest.FullName + "\\" + sourceFiles[s].Name);
                }
            }
        }
    }
}
