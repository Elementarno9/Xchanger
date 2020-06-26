using System;
using System.Collections.Generic;
using System.Text;

namespace Xchanger.UI
{
    public class FileListItem : IConsoleListItem
    {
        private const int minOffset = 2;
        public string Name { get; private set; }
        public double CompleteData { get; private set; }
        public double MaxData { get; private set; }

        public FileListItem(string name, double maxData)
        {
            Name = name;
            MaxData = maxData;
            CompleteData = 0;
        }

        public void Update(double completeData) => CompleteData = completeData <= MaxData && completeData > 0 ? completeData : CompleteData;

        public string ToString(int totalWidth)
        {
            var stringBuilder = new StringBuilder(totalWidth);
            string info = string.Format("{0}MB/{1}MB {2}", CompleteData, MaxData, ConsoleExtension.GetProgressBar(CompleteData, MaxData));
            int remainLength = totalWidth - info.Length;
            string name = Name.CropString(remainLength - minOffset);
            remainLength -= name.Length;
            stringBuilder.Append(name)
                .Append(new string(' ', remainLength))
                .Append(info);
            return stringBuilder.ToString();
        }
    }
}
