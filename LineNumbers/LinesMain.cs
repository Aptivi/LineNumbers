
// 
// MIT License
// 
// Copyright (c) 2021 Aptivi
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE Or THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.IO;
using LineNumbers.Core;

namespace LineNumbers
{

    static class LinesMain
    {

        public static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                // Check to see if solution exists
                if (File.Exists(args[0]))
                {
                    var LinesInfo = new LinesInfo(args[0]);
                    var LineNumbers = LinesInfo.LineNumbersByProject;

                    // Enumerate through each project
                    foreach (string ProjectName in LineNumbers.Keys)
                    {
                        var CodeFileLines = LinesInfo.LineNumbersByCodeFilesByProject[ProjectName];
                        foreach (string FileName in CodeFileLines.Keys)
                            Console.WriteLine("File {0}: {1} lines", Path.GetFileName(FileName), CodeFileLines[FileName]);
                        Console.WriteLine(Environment.NewLine + "Total for project {0}: {1} lines" + Environment.NewLine, ProjectName, LineNumbers[ProjectName]);
                    }

                    // Return the total
                    Console.WriteLine("Total: {0} lines", LinesInfo.SolutionLineNumber);
                }
                else
                {
                    Console.WriteLine("Solution not found.");
                    Environment.Exit(1);
                }
            }
            else
            {
                Console.WriteLine("Specify path to solution.");
                Environment.Exit(1);
            }
        }

    }
}