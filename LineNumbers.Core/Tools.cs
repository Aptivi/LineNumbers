
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
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;

namespace LineNumbers.Core
{
    /// <summary>
    /// Line number tools
    /// </summary>
    public static class Tools
    {

        /// <summary>
        /// Get the projects in the solution
        /// </summary>
        /// <param name="Solution">The solution file name</param>
        /// <param name="Workspace">MSBuild workspace</param>
        public static List<Project> ReturnProjects(string Solution, MSBuildWorkspace Workspace)
        {
            if (File.Exists(Solution))
            {
                var SolutionInstance = Workspace.OpenSolutionAsync(Solution).Result;
                return SolutionInstance.Projects.ToList();
            }
            else
            {
                throw new FileNotFoundException("The solution file specified is not found.", Solution);
            }
        }

        /// <summary>
        /// Get the projects in the solution
        /// </summary>
        /// <param name="Solution">The solution</param>
        public static List<Project> ReturnProjects(Solution Solution)
        {
            return Solution.Projects.ToList();
        }

        /// <summary>
        /// Returns the code files that are in the project
        /// </summary>
        /// <param name="ProjectSolution">The project in solution</param>
        public static List<string> ReturnCodeFiles(Project ProjectSolution)
        {
            if (File.Exists(ProjectSolution.FilePath) & ProjectSolution.SupportsCompilation)
            {
                string SolutionDirectory = Path.GetDirectoryName(ProjectSolution.FilePath);
                var Files = Directory.EnumerateFileSystemEntries(SolutionDirectory, "", SearchOption.AllDirectories);
                string TargetExtension = "";
                var ToParse = new List<string>();

                // Determine the language
                Debug.WriteLine(ProjectSolution.Language);
                switch (ProjectSolution.Language ?? "")
                {
                    case "Visual Basic":
                        {
                            TargetExtension = ".vb";
                            break;
                        }
                    case "C#":
                        {
                            TargetExtension = ".cs";
                            break;
                        }
                }

                // Get the files
                foreach (string File in Files)
                {
                    if (!File.Contains(@"My Project\") & !File.Contains(@"obj\"))
                    {
                        if (!string.IsNullOrEmpty(TargetExtension) & (Path.GetExtension(File) ?? "") == (TargetExtension ?? ""))
                        {
                            Debug.WriteLine(File);
                            ToParse.Add(File);
                        }
                    }
                }

                // Return the list
                ToParse.Sort();
                return ToParse;
            }
            else
            {
                throw new DirectoryNotFoundException("The project specified is not found.");
            }
        }

    }
}
