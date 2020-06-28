using System;
using System.Collections.Generic;
using System.Text;

namespace Xchanger.UI
{
    public class ConsoleList<T> where T : IConsoleListItem
    {
        private const int itemsOffset = 2;

        public string Title { get; private set; }
        public int[] Borders { get; private set; }

        public int ItemPointer { get; private set; }
        public bool Autoscroll { get; private set; }

        private int totalHeight { get => Borders[3] - Borders[1]; }
        private int totalWidth { get => Borders[2] - Borders[0]; }
        private int outputHeight { get => totalHeight - itemsOffset; }
        private List<T> items;

        public ConsoleList(string title, int left, int top, int width, int height, bool autoscroll = false)
        {
            Title = title;
            items = new List<T>();
            Borders = new int[4] { left, top, left + width, top + height };
            ItemPointer = 0;
            Autoscroll = autoscroll;
        }

        public void Draw()
        {
            ConsoleExtension.BeginWriting();

            ConsoleExtension.WriteAt(Title.PadRight(totalWidth), Borders[0], Borders[1]);

            for (int y = 0; y < outputHeight; y++)
            {
                string item;
                if (y < Math.Min(outputHeight, items.Count - ItemPointer) && (ItemPointer + y) < items.Count)
                {
                    item = items[ItemPointer + y].ToString(totalWidth).PadRight(totalWidth);
                }
                else item = new string(' ', totalWidth);
                ConsoleExtension.WriteAt(item, Borders[0], Borders[1] + itemsOffset + y);
            }
            ConsoleExtension.EndWriting();
        }

        public void SetBorders(int left, int top, int right, int bottom) => Borders = new[] { left, top, right, bottom };

        public void SetPointer(int pointer)
        {
            if (pointer >= 0 && pointer < items.Count) ItemPointer = pointer;
        }

        public void Add(T obj)
        {
            items.Add(obj);
            if (Autoscroll && items.Count - ItemPointer + 1 > outputHeight)
            {
                ItemPointer++;
            }
        }
    }
}
