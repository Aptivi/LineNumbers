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

Module LinesMain

    Sub Main(args As String())
        If args.Length > 0 Then
            'Check to see if solution exists
            If File.Exists(args(0)) Then
                Dim LinesInfo As New LinesInfo(args(0))
                Dim LineNumbers As Dictionary(Of String, Long) = LinesInfo.LineNumbersByProject

                'Enumerate through each project
                For Each ProjectName As String In LineNumbers.Keys
                    Dim CodeFileLines As Dictionary(Of String, Long) = LinesInfo.LineNumbersByCodeFilesByProject(ProjectName)
                    For Each FileName As String In CodeFileLines.Keys
                        Console.WriteLine("File {0}: {1} lines", Path.GetFileName(FileName), CodeFileLines(FileName))
                    Next
                    Console.WriteLine(Environment.NewLine + "Total for project {0}: {1} lines" + Environment.NewLine, ProjectName, LineNumbers(ProjectName))
                Next

                'Return the total
                Console.WriteLine("Total: {0} lines", LinesInfo.SolutionLineNumber)
            Else
                Console.WriteLine("Solution not found.")
                Environment.Exit(1)
            End If
        Else
            Console.WriteLine("Specify path to solution.")
            Environment.Exit(1)
        End If
    End Sub

End Module
