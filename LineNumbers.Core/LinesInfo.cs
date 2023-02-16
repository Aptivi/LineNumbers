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

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;

namespace LineNumbers.Core
{

    public class LinesInfo
    {

        /// <summary>
        /// The line numbers by project
        /// </summary>
        public Dictionary<string, long> LineNumbersByProject { get; private set; }

        /// <summary>
        /// The line number of the whole solution
        /// </summary>
        public long SolutionLineNumber { get; private set; }

        /// <summary>
        /// Paths to the individual code files
        /// </summary>
        public List<string> CodeFiles { get; private set; }

        /// <summary>
        /// Paths to the individual code files by project
        /// </summary>
        public Dictionary<string, List<string>> CodeFilesByProject { get; private set; }

        /// <summary>
        /// Line numbers of individual code files
        /// </summary>
        public Dictionary<string, long> LineNumbersByCodeFiles { get; private set; }

        /// <summary>
        /// Line numbers of individual code files by project
        /// </summary>
        public Dictionary<string, Dictionary<string, long>> LineNumbersByCodeFilesByProject { get; private set; }

        /// <summary>
        /// Creates a new lines information instance from the Visual Studio solution
        /// </summary>
        /// <param name="SolutionPath">Path to the Visual Studio solution</param>
        public LinesInfo(string SolutionPath) : this(MSBuildWorkspace.Create().OpenSolutionAsync(SolutionPath).Result)
        {
        }

        /// <summary>
        /// Creates a new lines information instance from the Visual Studio solution
        /// </summary>
        /// <param name="Solution">The Visual Studio solution</param>
        public LinesInfo(Solution Solution)
        {
            var Projects = Tools.ReturnProjects(Solution);
            var LineNumbersByProject = new Dictionary<string, long>();
            var CodeFiles = new List<string>();
            var CodeFilesByProject = new Dictionary<string, List<string>>();
            var LineNumbersByCodeFiles = new Dictionary<string, long>();
            var LineNumbersByCodeFilesByProject = new Dictionary<string, Dictionary<string, long>>();
            var Total = default(long);

            // Enumerate through each project
            var ProjectTotal = default(long);
            foreach (Project Project in Projects)
            {
                var ProjectCodeFiles = Tools.ReturnCodeFiles(Project);
                var LineNumbersByCodeFilesOfProject = new Dictionary<string, long>();

                // Enumerate through each code file
                foreach (string CodeFile in ProjectCodeFiles)
                {
                    Debug.WriteLine(CodeFile);
                    var FileLines = File.ReadAllLines(CodeFile);
                    long FileLinesLength = FileLines.Length;
                    Total += FileLinesLength;
                    ProjectTotal += FileLinesLength;
                    Debug.WriteLine(FileLinesLength.ToString() + ", " + Total.ToString());
                    LineNumbersByCodeFiles.Add(CodeFile, FileLinesLength);
                    LineNumbersByCodeFilesOfProject.Add(CodeFile, FileLinesLength);
                }

                // Install the values
                CodeFiles.AddRange(ProjectCodeFiles);
                CodeFilesByProject.Add(Project.Name, ProjectCodeFiles);
                LineNumbersByProject.Add(Project.Name, ProjectTotal);
                LineNumbersByCodeFilesByProject.Add(Project.Name, LineNumbersByCodeFilesOfProject);

                // Reset some values
                ProjectTotal = 0L;
            }

            // Install all the values to the new instance
            this.LineNumbersByProject = LineNumbersByProject;
            this.CodeFilesByProject = CodeFilesByProject;
            this.CodeFiles = CodeFiles;
            this.LineNumbersByCodeFiles = LineNumbersByCodeFiles;
            this.LineNumbersByCodeFilesByProject = LineNumbersByCodeFilesByProject;
            SolutionLineNumber = Total;
        }

    }
}