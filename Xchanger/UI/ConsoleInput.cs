﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Xchanger.UI
{
    class ConsoleInput
    {
        private const string beforeInputText = "> ";
        public bool IsWorking { get; private set; }
        public int Top { get; private set; }
        public int Left { get; private set; }
        public int MaxLength { get; private set; }

        public delegate void RecieveCommand(string command);
        public event RecieveCommand OnSendCommand;

        private StringBuilder builder;
        private Thread thread;
        private int carriagePosition = 0;
        private int displayPosition = 0;

        public ConsoleInput(int left, int top, int maxLength = 40)
        {
            thread = new Thread(Work);
            builder = new StringBuilder(maxLength);
            SetPosition(left, top, maxLength);
        }

        public void Start()
        {
            IsWorking = true;
            thread.Start();
        }

        public void Stop()
        {
            IsWorking = false;
        }

        public void Work()
        {
            while (IsWorking)
            {
                var input = Console.ReadKey(true);
                //Console.CursorTop--;
                switch (input.Key)
                {
                    case ConsoleKey.Enter:
                        SendMessage();
                        break;
                    case ConsoleKey.LeftArrow:
                        MoveCarriageLeft();
                        break;
                    case ConsoleKey.RightArrow:
                        MoveCarriageRight();
                        break;
                    case ConsoleKey.Backspace:
                        if (builder.Length > 0 && carriagePosition > 0)
                        {
                            builder.Remove(carriagePosition - 1, 1);
                            MoveCarriageLeft();
                        }
                        break; ;
                    default:
                        builder.Insert(carriagePosition, input.KeyChar);
                        MoveCarriageRight();
                        break;
                }

                ConsoleExtension.BeginWriting();
                string buffer = builder.ToString().PadRight(MaxLength + displayPosition);
                string output = string.Format("> {0}", buffer.Substring(displayPosition, MaxLength));
                ConsoleExtension.WriteAt(output.PadRight(MaxLength, ' '), Left, Top);
                ConsoleExtension.EndWriting();
                Console.SetCursorPosition(Left + (carriagePosition - displayPosition) + beforeInputText.Length, Top);
            }
        }

        public void SetPosition(int left, int top, int maxLength)
        {
            Left = left;
            Top = top;
            MaxLength = maxLength;
        }
        public void SetPosition(int left, int top) => SetPosition(left, top, MaxLength);

        private void MoveCarriageLeft()
        {
            if (carriagePosition > 0)
            {
                carriagePosition--;
                if (displayPosition > carriagePosition) displayPosition = carriagePosition;
            }
        }

        private void MoveCarriageRight()
        {
            if (carriagePosition < builder.Length)
            {
                carriagePosition++;
                if (carriagePosition - displayPosition > MaxLength) displayPosition = carriagePosition - MaxLength;
            }
        }

        private void SendMessage()
        {
            string result = builder.ToString();
            carriagePosition = 0;
            displayPosition = 0;
            builder.Clear();
            OnSendCommand?.Invoke(result);
        }
    }
}
