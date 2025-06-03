using System;
using System.Collections.Generic;

namespace Компилятор
{
    class Program
    {
        static void Main()
        {
            string[] testFiles = new string[] { "t1.pas", "t2.pas", "t3.pas", "t4.pas", "t5.pas" };

            foreach (var file in testFiles)
            {
                ProcessFile(file);
            }
        }

        static void ProcessFile(string filePath)
        {
            InputOutput.Initialize(filePath);
            SimulateErrors(filePath);
            List<string> lines = InputOutput.GetLines();
            for (uint lineNum = 1; lineNum <= lines.Count; lineNum++)
            {
                Console.WriteLine(lines[(int)lineNum - 1]);
                List<Err> errorsInLine = InputOutput.GetErrorsForLine(lineNum);
                foreach (var err in errorsInLine)
                {
                    string errorLine = InputOutput.GetErrorLine(err);
                    if (!string.IsNullOrEmpty(errorLine))
                        Console.WriteLine(errorLine);
                }
            }

            InputOutput.ClearErrors();
        }

        static void SimulateErrors(string filePath)
        {
            switch (filePath)
            {
                case "t1.pas":
                    InputOutput.Error(1, new TextPosition(4, 9));
                    break;

                case "t2.pas":
                    InputOutput.Error(2, new TextPosition(4, 3));
                    break;

                case "t3.pas":
                    InputOutput.Error(3, new TextPosition(5, 8));
                    break;

                case "t4.pas":
                    InputOutput.Error(4, new TextPosition(6, 8));
                    break;

                case "t5.pas":
                    InputOutput.Error(5, new TextPosition(4, 17));
                    break;
            }
        }
    }
}