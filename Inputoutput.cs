using System;
using System.Collections.Generic;

namespace Компилятор
{
    class Program
    {
        static void Main()
        {
            string testFile = "Test.pas";
            ProcessFile(testFile);
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
            if (filePath == "Test.pas")
            {
                InputOutput.Error(1, new TextPosition(8, 9));  
                InputOutput.Error(2, new TextPosition(9, 3));  
                InputOutput.Error(3, new TextPosition(12, 8)); 
                InputOutput.Error(4, new TextPosition(14, 8)); 
                InputOutput.Error(5, new TextPosition(16, 18)); 
            }
        }
    }
}
