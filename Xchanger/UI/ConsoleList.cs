using System;
using System.Collections.Generic;
using System.Text;

namespace Xchanger.UI
{
    public class ConsoleList<T> where T : IConsoleListItem
    {
        private const int itemsOffset = 2;

        public string Title { get; private set; }
        public List<T> Items { get; private set; }
        public int[] Borders { get; private set; }

        public int ItemPointer { get; private set; }

        public ConsoleList(string title)
        {
            Title = title;
            Items = new List<T>();
            Borders = new int[4];
            ItemPointer = 0;
        }

        public void Draw()
        {
            int totalHeight = Borders[3] - Borders[1];
            int totalWidth = Borders[2] - Borders[0];
            int outputHeight = totalHeight - itemsOffset;

            ConsoleExtension.BeginWriting();

            ConsoleExtension.WriteAt(Title.PadRight(totalWidth), Borders[0], Borders[1]);

            for (int y = 0; y < outputHeight; y++)
            {
                string item;
                if (y < Math.Min(outputHeight, Items.Count - ItemPointer) && (ItemPointer + y) < Items.Count)
                {
                    item = Items[ItemPointer + y].ToString(totalWidth).PadRight(totalWidth);
                }
                else item = new string(' ', totalWidth);
                ConsoleExtension.WriteAt(item, Borders[0], Borders[1] + itemsOffset + y);
            }
            ConsoleExtension.EndWriting();
        }

        public void SetBorders(int left, int top, int right, int bottom) => Borders = new[] { left, top, right, bottom };

        public void SetPointer(int pointer)
        {
            if (pointer >= 0 && pointer < Items.Count) ItemPointer = pointer;
        }
    }
}
