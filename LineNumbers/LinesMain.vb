'
' MIT License
'
' Copyright (c) 2021 EoflaOE and its companies
'
' Permission is hereby granted, free of charge, to any person obtaining a copy
' of this software and associated documentation files (the "Software"), to deal
' in the Software without restriction, including without limitation the rights
' to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
' copies of the Software, and to permit persons to whom the Software is
' furnished to do so, subject to the following conditions:
' 
' The above copyright notice and this permission notice shall be included in all
' copies or substantial portions of the Software.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
' IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
' FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
' LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
' OUT OF OR IN CONNECTION WITH THE SOFTWARE Or THE USE OR OTHER DEALINGS IN THE
' SOFTWARE.
' 

Imports System.IO
Imports LineNumbers.Core
Imports Microsoft.CodeAnalysis
Imports Microsoft.CodeAnalysis.MSBuild

Module LinesMain

    Sub Main(args As String())
        Dim Total As Long

        'Check to see if solution exists
        Dim Workspace As MSBuildWorkspace = MSBuildWorkspace.Create()
        If File.Exists(args(0)) Then
            Dim Projects As List(Of Project) = ReturnProjects(args(0), Workspace)

            'Enumerate through each project
            For Each Project As Project In Projects
                Dim ProjectTotal As Long
                Dim CodeFiles As List(Of String) = ReturnCodeFiles(Project)

                'Enumerate through each code file
                For Each CodeFile As String In CodeFiles
                    Debug.WriteLine(CodeFile)
                    Dim FileLines() As String = File.ReadAllLines(CodeFile)
                    Dim FileLinesLength As Long = FileLines.Length
                    Total += FileLinesLength
                    ProjectTotal += FileLinesLength
                    Debug.WriteLine(FileLinesLength.ToString + ", " + Total.ToString)
                    Console.WriteLine("File {0}: {1} lines", Path.GetFileName(CodeFile), FileLinesLength)
                Next

                'Total for solution
                Console.WriteLine(Environment.NewLine + "Total for project {0}: {1} lines" + Environment.NewLine, Project.Name, ProjectTotal)
                ProjectTotal = 0
            Next

            'Return the total
            Console.WriteLine(Environment.NewLine + "Total: {0} lines", Total)
            Console.ReadKey()
        Else
            Console.WriteLine("Solution not found.")
            Environment.Exit(1)
        End If
    End Sub

End Module
