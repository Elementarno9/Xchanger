using System;
using System.Collections.Generic;
using System.Text;

namespace Xchanger.UI
{
    public class LogListItem : IConsoleListItem
    {
        public readonly string Message;
        public readonly DateTime Time;


        public LogListItem(string message)
            : this(message, DateTime.Now) { }

        public LogListItem(string message, DateTime time)
        {
            Message = message;
            Time = time;
        }

        public string ToString(int totalWidth) => 
            string.Format("[{0}] {1}", Time.ToString("HH:mm:ss"), Message).CropString(totalWidth);
    }
}
