using System;
using System.Collections.Generic;
using System.IO;

namespace Компилятор
{
    struct TextPosition
    {
        public uint lineNumber; // номер строки
        public byte charNumber; // номер позиции в строке

        public TextPosition(uint ln = 0, byte c = 0)
        {
            lineNumber = ln;
            charNumber = c;
        }
    }

    struct Err
    {
        public TextPosition errorPosition;
        public byte errorCode;

        public Err(TextPosition errorPosition, byte errorCode)
        {
            this.errorPosition = errorPosition;
            this.errorCode = errorCode;
        }
    }

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
            if (positionNow.charNumber == (currentLine?.Length ?? 0) - 1)
            {
                positionNow.lineNumber = (uint)(currentLineIndex + 1);
                positionNow.charNumber = 0;
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
            else if (positionNow.charNumber < (currentLine?.Length ?? 0) - 1)
            {
                positionNow.charNumber++;
                Ch = currentLine[positionNow.charNumber];
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
            if (lines == null || item.errorPosition.lineNumber == 0 || item.errorPosition.lineNumber > lines.Count) return "";
            int charPosition = item.errorPosition.charNumber - 1;
            if (charPosition < 0) charPosition = 0;
            string spaces = new string(' ', charPosition);
            string errorMessage = $"^ ошибка код {item.errorCode} ({errorDescriptions[item.errorCode]})";
            return spaces + errorMessage;
        }

        public static List<Err> GetErrorsForLine(uint lineNumber)
        {
            return err.FindAll(e => e.errorPosition.lineNumber == lineNumber);
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