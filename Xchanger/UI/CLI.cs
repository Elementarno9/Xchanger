using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Xchanger.UI
{
    public class CLI
    {
        private const int commonOffset = 1;
        private const int inputBottomOffset = 2;

        public const int Delay = 100;

        public static readonly CLI Instance = new CLI();

        public ConsoleList<FileListItem> DownloadList { get; private set; }
        public ConsoleList<FileListItem> UploadList { get; private set; }
        public ConsoleList<LogListItem> LogList { get; private set; }

        public ConsoleInput Input { get; private set; }

        public bool IsWorking { get; private set; }

        private Thread thread;

        private CLI()
        {
            int listHeight = (Console.WindowHeight - commonOffset * 2 - inputBottomOffset) / 2;
            int largeListWidth = Console.WindowWidth - commonOffset * 2;
            int listWidth = (largeListWidth - commonOffset) / 2;

            DownloadList = new ConsoleList<FileListItem>("Downloading", commonOffset, commonOffset, listWidth, listHeight);
            UploadList = new ConsoleList<FileListItem>("Uploading", listWidth + 2 * commonOffset, commonOffset, listWidth, listHeight);
            LogList = new ConsoleList<LogListItem>("Log", commonOffset, listHeight + 2 * commonOffset, largeListWidth, listHeight, true);
            Input = new ConsoleInput(commonOffset, Console.WindowHeight - inputBottomOffset, largeListWidth - commonOffset);

            thread = new Thread(Work);
        }

        public void Start() => thread.Start();

        public void Stop()
        {
            IsWorking = false;
            thread.Abort();
        }

        public void Work()
        {
            IsWorking = true;

            Input.Start();

            while (IsWorking)
            {
                Draw();
                Thread.Sleep(Delay);
            }
        }

        private void Draw()
        {
            UploadList.Draw();
            DownloadList.Draw();
            LogList.Draw();
        }
    }
}
