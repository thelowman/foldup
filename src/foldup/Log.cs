using System;

namespace foldup
{
    /// <summary>
    /// Contains static methods for updating a log file or the console.
    /// </summary>
    internal class Log
    {
        /// <summary>
        /// Writes a line of text to the console.
        /// </summary>
        /// <param name="text">Text to write to the log and to the screen.</param>
        public static void Add(string text)
        {
            WriteLine(text);
        }
        /// <summary>
        /// Writes the line of text to the console using the specified foreground and background colors.
        /// </summary>
        /// <param name="text">Text to write to the log and to the screen.</param>
        /// <param name="foreground">One of the ConsoleColor values to use as the text color on screen.</param>
        /// <param name="background">One of the ConsoleColor values to use as the background color on screen.</param>
        public static void Add(string text, ConsoleColor foreground, ConsoleColor background)
        {
            WriteLine(text, foreground, background);
        }
        /// <summary>
        /// Writes text to the console using the specified foreground and background colors.
        /// </summary>
        /// <param name="text">Text to write to the sreen.</param>
        /// <param name="foreground">A ConsoleColor value for the text.</param>
        /// <param name="background">A ConsoleColor value for the background of the text.</param>
        public static void Write(string text, ConsoleColor foreground, ConsoleColor background)
        {
            ConsoleColor fgNorm = Console.ForegroundColor;
            ConsoleColor bgNorm = Console.BackgroundColor;

            Console.ForegroundColor = foreground;
            Console.BackgroundColor = background;
            Write(text);

            Console.ForegroundColor = fgNorm;
            Console.BackgroundColor = bgNorm;
        }
        /// <summary>
        /// Writes text to the console using the specified foreground color.
        /// </summary>
        /// <param name="text">Text to write to the sreen.</param>
        /// <param name="foreground">A ConsoleColor value for the text.</param>
        public static void Write(string text, ConsoleColor foreground)
        {
            Write(text, foreground, Console.BackgroundColor);
        }
        /// <summary>
        /// Writes text to the console using the default foreground and background colors.
        /// </summary>
        /// <param name="text">The text to write to the console.</param>
        public static void Write(string text)
        {
            Console.Write(text);
        }

        /// <summary>
        /// Writes a line of text to the console using the specified foreground and background colors.
        /// </summary>
        /// <param name="text">Text to write to the sreen.</param>
        /// <param name="foreground">A ConsoleColor value for the text.</param>
        /// <param name="background">A ConsoleColor value for the background of the text.</param>
        public static void WriteLine(string text, ConsoleColor foreground, ConsoleColor background)
        {
            ConsoleColor fgNorm = Console.ForegroundColor;
            ConsoleColor bgNorm = Console.BackgroundColor;

            Console.ForegroundColor = foreground;
            Console.BackgroundColor = background;
            WriteLine(text);

            Console.ForegroundColor = fgNorm;
            Console.BackgroundColor = bgNorm;
        }
        /// <summary>
        /// Writes a line of text to the console using the specified foreground color.
        /// </summary>
        /// <param name="text">Text to write to the screen.</param>
        /// <param name="foreground">A ConsoleColor value for the text.</param>
        public static void WriteLine(string text, ConsoleColor foreground)
        {
            WriteLine(text, foreground, Console.BackgroundColor);
        }
        /// <summary>
        /// Writes a line of text to the console using the default foreground and background colors.
        /// </summary>
        /// <param name="text">The text to write to the console.</param>
        public static void WriteLine(string text)
        {
            Console.WriteLine(text);
        }
        /// <summary>
        /// Writes the current line terminator to the console. 
        /// </summary>
        public static void WriteLine()
        {
            Console.WriteLine();
        }
    }
}
