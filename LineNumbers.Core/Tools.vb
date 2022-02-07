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
Imports Microsoft.CodeAnalysis.MSBuild
Imports Microsoft.CodeAnalysis

Public Module Tools

    ''' <summary>
    ''' Get the projects in the solution
    ''' </summary>
    ''' <param name="Solution">The solution file name</param>
    Function ReturnProjects(Solution As String, Workspace As MSBuildWorkspace) As List(Of Project)
        If File.Exists(Solution) Then
            Dim SolutionInstance As Solution = Workspace.OpenSolutionAsync(Solution).Result
            Return SolutionInstance.Projects.ToList
        Else
            Throw New FileNotFoundException("The solution file specified is not found.", Solution)
        End If
    End Function

    ''' <summary>
    ''' Get the projects in the solution
    ''' </summary>
    ''' <param name="Solution">The solution</param>
    Function ReturnProjects(Solution As Solution) As List(Of Project)
        Return Solution.Projects.ToList
    End Function

    ''' <summary>
    ''' Returns the code files that are in the project
    ''' </summary>
    ''' <param name="ProjectSolution">The project in solution</param>
    Function ReturnCodeFiles(ProjectSolution As Project) As List(Of String)
        If File.Exists(ProjectSolution.FilePath) And ProjectSolution.SupportsCompilation Then
            Dim SolutionDirectory As String = Path.GetDirectoryName(ProjectSolution.FilePath)
            Dim Files = Directory.EnumerateFileSystemEntries(SolutionDirectory, "", SearchOption.AllDirectories)
            Dim TargetExtension As String = ""
            Dim ToParse As New List(Of String)

            'Determine the language
            Debug.WriteLine(ProjectSolution.Language)
            Select Case ProjectSolution.Language
                Case "Visual Basic"
                    TargetExtension = ".vb"
                Case "C#"
                    TargetExtension = ".cs"
            End Select

            'Get the files
            For Each File As String In Files
                If Not File.Contains("My Project\") And Not File.Contains("obj\") Then
                    If Not String.IsNullOrEmpty(TargetExtension) And Path.GetExtension(File) = TargetExtension Then
                        Debug.WriteLine(File)
                        ToParse.Add(File)
                    End If
                End If
            Next

            'Return the list
            ToParse.Sort()
            Return ToParse
        Else
            Throw New DirectoryNotFoundException("The project specified is not found.")
        End If
    End Function

End Module
