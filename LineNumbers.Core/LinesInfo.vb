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
Imports Microsoft.CodeAnalysis
Imports Microsoft.CodeAnalysis.MSBuild

Public Class LinesInfo

    ''' <summary>
    ''' The line numbers by project
    ''' </summary>
    Public ReadOnly Property LineNumbersByProject() As Dictionary(Of String, Long)

    ''' <summary>
    ''' The line number of the whole solution
    ''' </summary>
    Public ReadOnly Property SolutionLineNumber As Long

    ''' <summary>
    ''' Paths to the individual code files
    ''' </summary>
    Public ReadOnly Property CodeFiles() As List(Of String)

    ''' <summary>
    ''' Paths to the individual code files by project
    ''' </summary>
    Public ReadOnly Property CodeFilesByProject() As Dictionary(Of String, List(Of String))

    ''' <summary>
    ''' Line numbers of individual code files
    ''' </summary>
    Public ReadOnly Property LineNumbersByCodeFiles() As Dictionary(Of String, Long)

    ''' <summary>
    ''' Line numbers of individual code files by project
    ''' </summary>
    Public ReadOnly Property LineNumbersByCodeFilesByProject() As Dictionary(Of String, Dictionary(Of String, Long))

    ''' <summary>
    ''' Creates a new lines information instance from the Visual Studio solution
    ''' </summary>
    ''' <param name="SolutionPath">Path to the Visual Studio solution</param>
    Public Sub New(SolutionPath As String)
        Me.New(MSBuildWorkspace.Create().OpenSolutionAsync(SolutionPath).Result)
    End Sub

    ''' <summary>
    ''' Creates a new lines information instance from the Visual Studio solution
    ''' </summary>
    ''' <param name="Solution">The Visual Studio solution</param>
    Public Sub New(Solution As Solution)
        Dim Projects As List(Of Project) = ReturnProjects(Solution)
        Dim LineNumbersByProject As New Dictionary(Of String, Long)
        Dim CodeFiles As New List(Of String)
        Dim CodeFilesByProject As New Dictionary(Of String, List(Of String))
        Dim LineNumbersByCodeFiles As New Dictionary(Of String, Long)
        Dim LineNumbersByCodeFilesByProject As New Dictionary(Of String, Dictionary(Of String, Long))
        Dim Total As Long

        'Enumerate through each project
        For Each Project As Project In Projects
            Dim ProjectTotal As Long
            Dim ProjectCodeFiles As List(Of String) = ReturnCodeFiles(Project)
            Dim LineNumbersByCodeFilesOfProject As New Dictionary(Of String, Long)

            'Enumerate through each code file
            For Each CodeFile As String In ProjectCodeFiles
                Debug.WriteLine(CodeFile)
                Dim FileLines() As String = File.ReadAllLines(CodeFile)
                Dim FileLinesLength As Long = FileLines.Length
                Total += FileLinesLength
                ProjectTotal += FileLinesLength
                Debug.WriteLine(FileLinesLength.ToString + ", " + Total.ToString)
                LineNumbersByCodeFiles.Add(CodeFile, FileLinesLength)
                LineNumbersByCodeFilesOfProject.Add(CodeFile, FileLinesLength)
            Next

            'Install the values
            CodeFiles.AddRange(ProjectCodeFiles)
            CodeFilesByProject.Add(Project.Name, ProjectCodeFiles)
            LineNumbersByProject.Add(Project.Name, ProjectTotal)
            LineNumbersByCodeFilesByProject.Add(Project.Name, LineNumbersByCodeFilesOfProject)

            'Reset some values
            ProjectTotal = 0
        Next

        'Install all the values to the new instance
        Me.LineNumbersByProject = LineNumbersByProject
        Me.CodeFilesByProject = CodeFilesByProject
        Me.CodeFiles = CodeFiles
        Me.LineNumbersByCodeFiles = LineNumbersByCodeFiles
        Me.LineNumbersByCodeFilesByProject = LineNumbersByCodeFilesByProject
        SolutionLineNumber = Total
    End Sub

End Class
