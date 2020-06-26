using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Xchanger.UI
{
    public static class ConsoleExtension
    {
        private const char progressBarFullChar = '#';
        private const char progressBarEmptyChar = '.';
        private const int progressBarLength = 10;

        private static readonly object writeLocker = new object();
        private static Mutex writeMutex = new Mutex();
        private class CursorPosition
        {
            public int Left;
            public int Top;

            public CursorPosition(int left = 0, int top = 0) => Set(left, top);

            public void Set(int left, int top)
            {
                Left = left;
                Top = top;
            }
        }

        private static CursorPosition cursor = new CursorPosition(0,0);

        public static void BeginWriting()
        {
            writeMutex.WaitOne();
            cursor.Set(Console.CursorLeft, Console.CursorTop);
            Console.CursorVisible = false;
        }

        public static void EndWriting()
        {
            Console.SetCursorPosition(cursor.Left, cursor.Top);
            Console.CursorVisible = true;
            writeMutex.ReleaseMutex();
        }

        public static void WriteAt(string value, int x = 0, int y = 0, ConsoleColor color = ConsoleColor.White)
        {
            lock (writeLocker)
            {
                Console.SetCursorPosition(x, y);
                Console.ForegroundColor = color;
                Console.WriteLine(value);
                Console.ResetColor();
            }
        }

        public static string GetProgressBar(double current, double max)
        {
            var stringBuilder = new StringBuilder();
            int completeCount = (int)(progressBarLength * current / max);
            stringBuilder.Append('[')
                .Append(new string(progressBarFullChar, completeCount))
                .Append(new string(progressBarEmptyChar, progressBarLength - completeCount))
                .Append(']');
            return stringBuilder.ToString();
        }

        public static string CropString(this string source, int maxSize, string ending = "...")
        {
            if (source.Length <= maxSize) return source;
            var stringBuilder = new StringBuilder(maxSize);
            stringBuilder.Append(source.Substring(0, maxSize - ending.Length))
                .Append(ending);
            return stringBuilder.ToString();
        }
    }
}
