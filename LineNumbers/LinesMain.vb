
'    Line Numbers  Copyright (C) 2018-2020  EoflaOE
'
'    This file is part of Line Numbers
'
'    Line Numbers is free software: you can redistribute it and/or modify
'    it under the terms of the GNU General Public License as published by
'    the Free Software Foundation, either version 3 of the License, or
'    (at your option) any later version.
'
'    Line Numbers is distributed in the hope that it will be useful,
'    but WITHOUT ANY WARRANTY; without even the implied warranty of
'    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'    GNU General Public License for more details.
'
'    You should have received a copy of the GNU General Public License
'    along with this program.  If not, see <https://www.gnu.org/licenses/>.

Imports System.IO

Module LinesMain
    ReadOnly vbNewLine As String = Environment.NewLine
    Sub Main(args As String())
        Dim ToParse As New List(Of String)
        Dim Total As Long
        If Directory.Exists(args(0)) And CheckForProject(args(0)) Then
            Dim Files = Directory.EnumerateFileSystemEntries(args(0), "", SearchOption.AllDirectories)
            For Each File As String In Files
                If File.EndsWith(".vb") And Not File.Contains("My Project\") And Not File.Contains("obj\") Then
                    Debug.WriteLine(File)
                    ToParse.Add(File)
                End If
            Next
            ToParse.Sort()
            For Each File As String In ToParse
                Dim FileLines() As String = IO.File.ReadAllLines(File)
                Dim FileLinesLength As Long = FileLines.Length
                Total += FileLinesLength
                Debug.WriteLine(FileLinesLength.ToString + ", " + Total.ToString)
                Console.WriteLine("File {0}: {1} lines", Path.GetFileName(File), FileLinesLength)
            Next
            Console.WriteLine(vbNewLine + "Total: {0} lines", Total)
            Console.ReadKey()
        Else
            Console.WriteLine("Directory not found.")
            Environment.Exit(1)
        End If
    End Sub
    Function CheckForProject(ByVal Dir As String) As Boolean
        Dim Files = Directory.EnumerateFiles(Dir)
        For Each File As String In Files
            If File.Contains(".vbproj") Then
                Return True
            End If
        Next
        Return False
    End Function
End Module
