using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace foldup
{
    /// <summary>
    /// Contains static methods for updating a log file or the console.
    /// </summary>
    internal class Log
    {
        /// <summary>
        /// Appends a line of text to the log file and sends the text to the console.
        /// </summary>
        /// <param name="text">Text to write to the log and to the screen.</param>
        public static void Add(string text)
        {
            Append(text);
            WriteLine(text);
        }
        /// <summary>
        /// Appends a line of text to the log file and sends the text to the console
        /// using the specified foreground and background colors.
        /// </summary>
        /// <param name="text">Text to write to the log and to the screen.</param>
        /// <param name="foreground">One of the ConsoleColor values to use as the text color on screen.</param>
        /// <param name="background">One of the ConsoleColor values to use as the background color on screen.</param>
        public static void Add(string text, ConsoleColor foreground, ConsoleColor background)
        {
            Append(text);
            WriteLine(text, foreground, background);
        }
        /// <summary>
        /// Appends a line of text to the log file.
        /// </summary>
        /// <param name="text">Text to write to the log file.</param>
        public static void Append(string text)
        {
            using (StreamWriter w = File.AppendText(Configuration.AssemblyDirectory + "\\log_" + DateTime.Now.ToString("MM-dd-yy-HH-mm") + ".txt"))
            {
                w.WriteLine(text);
            }
        }
        /// <summary>
        /// Writes text to the console using the specified foreground and background colors.
        /// </summary>
        /// <param name="text">Text to write to the sreen.</param>
        /// <param name="foreground">A ConsoleColor value for the text.</param>
        /// <param name="background">A ConsoleColor value for the background of the text.</param>
        public static void Write(string text, ConsoleColor foreground, ConsoleColor background)
        {
            ConsoleColor fgNorm = System.Console.ForegroundColor;
            ConsoleColor bgNorm = System.Console.BackgroundColor;

            System.Console.ForegroundColor = foreground;
            System.Console.BackgroundColor = background;
            Write(text);

            System.Console.ForegroundColor = fgNorm;
            System.Console.BackgroundColor = bgNorm;
        }
        /// <summary>
        /// Writes text to the console using the specified foreground color.
        /// </summary>
        /// <param name="text">Text to write to the sreen.</param>
        /// <param name="foreground">A ConsoleColor value for the text.</param>
        public static void Write(string text, ConsoleColor foreground)
        {
            Write(text, foreground, System.Console.BackgroundColor);
        }
        /// <summary>
        /// Writes text to the console using the default foreground and background colors.
        /// </summary>
        /// <param name="text">The text to write to the console.</param>
        public static void Write(string text)
        {
            System.Console.Write(text);
        }

        /// <summary>
        /// Writes a line of text to the console using the specified foreground and background colors.
        /// </summary>
        /// <param name="text">Text to write to the sreen.</param>
        /// <param name="foreground">A ConsoleColor value for the text.</param>
        /// <param name="background">A ConsoleColor value for the background of the text.</param>
        public static void WriteLine(string text, ConsoleColor foreground, ConsoleColor background)
        {
            ConsoleColor fgNorm = System.Console.ForegroundColor;
            ConsoleColor bgNorm = System.Console.BackgroundColor;

            System.Console.ForegroundColor = foreground;
            System.Console.BackgroundColor = background;
            WriteLine(text);

            System.Console.ForegroundColor = fgNorm;
            System.Console.BackgroundColor = bgNorm;
        }
        /// <summary>
        /// Writes a line of text to the console using the specified foreground color.
        /// </summary>
        /// <param name="text">Text to write to the screen.</param>
        /// <param name="foreground">A ConsoleColor value for the text.</param>
        public static void WriteLine(string text, ConsoleColor foreground)
        {
            WriteLine(text, foreground, System.Console.BackgroundColor);
        }
        /// <summary>
        /// Writes a line of text to the console using the default foreground and background colors.
        /// </summary>
        /// <param name="text">The text to write to the console.</param>
        public static void WriteLine(string text)
        {
            System.Console.WriteLine(text);
        }
        /// <summary>
        /// Writes the current line terminator to the console. 
        /// </summary>
        public static void WriteLine()
        {
            System.Console.WriteLine();
        }
    }
}
