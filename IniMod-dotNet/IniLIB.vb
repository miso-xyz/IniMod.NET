Public Class IniLIB
    Public file As String
    Public Function GetAllSections() As String()
        Dim data As String() = IO.File.ReadAllLines(file)
        Dim retrive As New List(Of String)
        For x = 0 To data.Count - 1
            If data(x).StartsWith("[") Then
                retrive.Add(data(x).Replace("[", Nothing).Replace("]", Nothing))
            End If
        Next
        Return retrive.ToArray
    End Function
#Region "Private Functions (used to make life easier)"
    Private Function GetLineOfSectionName(ByVal sectionName As String) As Integer
        Dim data As String() = IO.File.ReadAllLines(file)
        For x = 0 To data.Count - 1
            If data(x) = "[" & sectionName & "]" Then
                Return x
            End If
        Next
        Return -1
    End Function
    Private Function GetLineOfKey(ByVal keyName As String) As Integer
        Dim data As String() = IO.File.ReadAllLines(file)
        For x = 0 To data.Count - 1
            If data(x).StartsWith(keyName & "=") Then
                Return x
            End If
        Next
        Return -1
    End Function
    Public Sub changeKeyValue(ByVal keyName As String, ByVal newValue As String)
        Dim data As New List(Of String)
        data = IO.File.ReadAllLines(file).ToList()
        For x = 0 To data.Count - 1
            If data(x).StartsWith(keyName) Then
                data(x) = data(x).Split("=", 2, StringSplitOptions.None)(0) & "=" & newValue
                IO.File.WriteAllLines(file, data)
                Return
            End If
        Next
        Throw New Exception("Key not found!")
    End Sub
    Private Function getKeyValue(ByVal keyName As String) As String
        Dim data As String() = IO.File.ReadAllLines(file)
        For x = 0 To data.Count - 1
            If data(x).StartsWith(keyName) Then
                Return data(x).Split("=", 2, StringSplitOptions.None)(1)
            End If
        Next
        Return -1
    End Function
#End Region
    Public Function getKeyValueFromSection(ByVal sectionName As String, ByVal keyName As String) As String
        Dim data As String() = GetAllKeysOfSections(sectionName)
        Dim iniFile As String() = IO.File.ReadAllLines(file)
        For x = 0 To data.Count - 1
            If data(x).StartsWith(keyName) Then
                Return iniFile(GetLineOfKey(keyName)).Split("=")(1)
            End If
        Next
        Return "Key """ & keyName & """ does not exist!"
    End Function
    Private Function countCommentsOfSection(ByVal sectionName As String)
        Dim data As String() = IO.File.ReadAllLines(file)
        Dim sections As String() = GetAllSections()
        Dim commentCounter As Integer = 0
        For x = 0 To GetLineOfSectionName(sections(sections.ToList().IndexOf(sectionName) + 1)) - GetLineOfSectionName(sectionName)
            If data(x).StartsWith(";") Then
                commentCounter += 1
            End If
        Next
        Return commentCounter
    End Function
    Private Function getLineOfComment(ByVal sectionName As String, ByVal commentString As String)
        Dim data As New List(Of String)
        data = IO.File.ReadAllLines(file).ToList
        Return data.IndexOf(commentString)
    End Function
    Public Sub writeCommentToSection(ByVal sectionName As String, ByVal comment As String, ByVal paddingSpaces As Integer)
        Dim data As New List(Of String)
        data = IO.File.ReadAllLines(file).ToList
        Dim commentString As String = ";"
        For x = 0 To paddingSpaces - 1
            commentString += " "
        Next
        commentString += comment
        data.Insert((GetLineOfSectionName(sectionName) + countCommentsOfSection(sectionName)) + 1, commentString)
        IO.File.WriteAllLines(file, data)
    End Sub
    Private Function getAllCommentsFromSection(ByVal sectionName As String)
        Dim data As New List(Of String)
        Dim sections As String() = GetAllSections()
        data = IO.File.ReadAllLines(file).ToList
        Dim sectionData = data.GetRange(GetLineOfSectionName(sectionName), GetLineOfSectionName(sections(sections.ToList().IndexOf(sectionName) + 1)))
        For x = 0 To sectionData.Count
            If Not sectionData(x).StartsWith(";") Then
                sectionData.RemoveAt(x)
            End If
        Next
        Return sectionData.ToArray
    End Function
    Private Function getAllFileComments() As String()
        Dim data As New List(Of String)
        data = IO.File.ReadAllLines(file).ToList
        Dim sections As New List(Of String)
        sections = GetAllSections().ToList
        data.RemoveRange(GetLineOfSectionName(sections(0)), data.Count - -(0 - GetLineOfSectionName(sections(0))))
        Return data.ToArray
    End Function
    Private Function getLineOfFileComment(ByVal fileComment As String)
        Dim data As New List(Of String)
        data = IO.File.ReadAllLines(file).ToList
        Dim sections As New List(Of String)
        sections = GetAllSections().ToList
        data.RemoveRange(GetLineOfSectionName(sections(0)), data.Count - -(0 - GetLineOfSectionName(sections(0))))
        For x = 0 To data.Count - 1
            If data(x) = fileComment Then
                Return x
            End If
        Next
        Return -1
    End Function
    Public Sub WriteFileComment(ByVal comment As String, ByVal paddingSpaces As Integer)
        Dim data As New List(Of String)
        Dim commentString As String = ";"
        For x = 0 To paddingSpaces - 1
            commentString += " "
        Next
        commentString += comment
        data = IO.File.ReadAllLines(file).ToList
        data.Insert(getAllFileComments.Count, commentString)
        IO.File.WriteAllLines(file, data)
    End Sub
    Public Sub RemoveFileComment()
        Dim data As New List(Of String)
removeFileComment:
        data = IO.File.ReadAllLines(file).ToList
        Dim comments As String() = getAllFileComments()
        If comments.Count = 0 Then
            Return
        Else
            For x = 0 To comments.Count - 1
                data.RemoveAt(getLineOfFileComment(comments(x)))
                IO.File.WriteAllLines(file, data)
            Next
            GoTo removeFileComment
        End If
        IO.File.WriteAllLines(file, data)
    End Sub
    Private Sub RemoveComment(ByVal sectionName As String, ByVal comment As String, ByVal paddingSpaces As Integer)
        Dim data As New List(Of String)
        Dim commentString As String = ";"
        For x = 0 To paddingSpaces - 1
            commentString += " "
        Next
        data = IO.File.ReadAllLines(file).ToList()
        data.RemoveAt(getLineOfComment(sectionName, commentString))
    End Sub
    Public Sub RemoveCommentsFromSection(ByVal sectionName As String)
        Dim data As New List(Of String)
        data = IO.File.ReadAllLines(file).ToList()
        Dim comments As New List(Of Integer)
        For x = 0 To data.Count - 1
            If data(x).StartsWith(";") Then
                comments.Add(x)
            End If
        Next
        For x = 0 To comments.Count - 1
            data.RemoveAt(comments(x))
        Next
        IO.File.WriteAllLines(file, data)
    End Sub
    Public Sub RenameKey(ByVal sectionName As String, ByVal keyName As String, ByVal newKeyName As String)
        Dim data = IO.File.ReadAllLines(file)
        data(GetLineOfKey(keyName)) = data(GetLineOfKey(keyName)).Replace(keyName, newKeyName)
        IO.File.WriteAllLines(file, data)
    End Sub
    Public Sub RemoveKey(ByVal sectionName As String, ByVal keyName As String)
        Dim data As New List(Of String)
        data = IO.File.ReadAllLines(file).ToList()
        data.RemoveAt(GetLineOfKey(keyName))
        IO.File.WriteAllLines(file, data.ToArray)
    End Sub

    Public Sub RemoveSection(ByVal sectionName As String)
        Dim data As New List(Of String)
        Dim sections As String() = GetAllSections()
        data = IO.File.ReadAllLines(file).ToList()
        data.RemoveRange(GetLineOfSectionName(sectionName), GetLineOfSectionName(sections(sections.ToList().IndexOf(sectionName) + 1)))
        IO.File.WriteAllLines(file, data.ToArray)
    End Sub

    Public Sub RemoveAllSections()
        IO.File.WriteAllText(file, Nothing)
    End Sub

    Public Function GetAllKeysOfSections(ByVal sectionName As String) As String()
        Dim data As String() = IO.File.ReadAllLines(file)
        Dim retrive As String() = GetAllSections()
        Dim countKey As Integer
        Try
            Dim check = retrive(retrive.ToList().IndexOf(sectionName) + 1)
            countKey = GetLineOfSectionName(retrive(retrive.ToList().IndexOf(sectionName) + 1)) - (GetLineOfSectionName(sectionName) + 1)
        Catch ex As Exception
            countKey = (data.Count - 1) - (GetLineOfSectionName(sectionName) + 1)
        End Try
        Dim keys As New List(Of String)
        For x = 0 To countKey
            keys.Add(data(x).Split("=")(0))
        Next
        keys.RemoveAt(0)
        Return keys.ToArray
    End Function
End Class
