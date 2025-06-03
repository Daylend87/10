using System;
using System.Collections.Generic;
using System.IO;

namespace Компилятор
{
    class InputOutput
    {
        private const byte ERRMAX = 9;
        private static char Ch { get; set; }
        private static TextPosition positionNow = new TextPosition();
        private static List<string> lines;
        private static int currentLineIndex = -1;
        private static List<Err> err;

        private static readonly Dictionary<byte, string> errorDescriptions = new Dictionary<byte, string>
        {
            { 1, "Отсутствует точка с запятой (;)" },
            { 2, "Неизвестное имя переменной" },
            { 3, "Несовместимость типов при присваивании" },
            { 4, "Переменная не инициализирована" },
            { 5, "Отсутствует закрывающий апостроф" }
        };

        public static void Initialize(string filePath)
        {
            lines = new List<string>();
            err = new List<Err>();
            try
            {
                using (StreamReader file = new StreamReader(filePath))
                {
                    string line;
                    while ((line = file.ReadLine()) != null)
                    {
                        lines.Add(line);
                    }
                }
                if (lines.Count > 0)
                {
                    currentLineIndex = 0;
                    Ch = lines[0].Length > 0 ? lines[0][0] : '\0';
                }
            }
            catch (Exception)
            {
            }
        }

        public static void NextCh()
        {
            if (currentLineIndex < 0 || currentLineIndex >= lines.Count)
            {
                return;
            }

            string currentLine = lines[currentLineIndex];
            if (positionNow.CharNumber == (currentLine?.Length ?? 0) - 1)
            {
                positionNow.LineNumber = (uint)(currentLineIndex + 1);
                positionNow.CharNumber = 0;
                currentLineIndex++;
                if (currentLineIndex < lines.Count)
                {
                    Ch = lines[currentLineIndex].Length > 0 ? lines[currentLineIndex][0] : '\0';
                }
                else
                {
                    Ch = '\0';
                }
            }
            else if (positionNow.CharNumber < (currentLine?.Length ?? 0) - 1)
            {
                positionNow.CharNumber++;
                Ch = currentLine[positionNow.CharNumber];
            }
            else
            {
                Ch = '\0';
            }
        }

        public static List<string> GetLines()
        {
            return new List<string>(lines);
        }

        public static string GetErrorLine(Err item)
        {
            if (lines == null || item.ErrorPosition.LineNumber == 0 || item.ErrorPosition.LineNumber > lines.Count) return "";
            int charPosition = item.ErrorPosition.CharNumber - 1;
            if (charPosition < 0) charPosition = 0;
            string spaces = new string(' ', charPosition);
            string errorMessage = $"^ ошибка код {item.ErrorCode} ({errorDescriptions[item.ErrorCode]})";
            return spaces + errorMessage;
        }

        public static List<Err> GetErrorsForLine(uint lineNumber)
        {
            return err.FindAll(e => e.ErrorPosition.LineNumber == lineNumber);
        }

        public static void ClearErrors()
        {
            err.Clear();
        }

        public static void Error(byte errorCode, TextPosition position)
        {
            if (err.Count < ERRMAX)
            {
                err.Add(new Err(position, errorCode));
            }
        }

        public static bool EndOfFile()
        {
            return currentLineIndex >= lines.Count;
        }
    }
}