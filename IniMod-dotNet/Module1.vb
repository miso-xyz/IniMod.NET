Module Module1
    Dim IniLib As New IniLIB
    Sub VersionText()
        Console.WriteLine("IniMod 1.0 [x86] (2021.03.02)")
    End Sub
    Sub StartText()
        VersionText()
        Console.WriteLine("Console application for managing INI files.")
        Console.WriteLine("Original Application: Jacek Pazera, https://www.pazera-software.com/products/inimod/")
        Console.WriteLine(".NET Remake: misonothx, https://github.com/miso-xyz")
    End Sub

    Sub LicenseText()
        Console.ForegroundColor = ConsoleColor.White
        Console.WriteLine("Freeware, Open Source")
        Console.ResetColor()
        Console.WriteLine("This program is completely free. You can use it without any restrictions, also for commercial purposes.")
        Console.WriteLine("The program's source files are available at https://github.com/miso-xyz/IniMod.NET")
        Console.WriteLine("The original program's source files are available at https://github.com/jackdp/IniMod")
        Console.WriteLine("Compiled binaries for the remake are available on the remake's github repository, the original application binaries are available at https://www.pazera-software.com/products/inimod/")
    End Sub

    Sub ShowHelp()
        Console.Write("Usage: IniMod ")
        Console.ForegroundColor = ConsoleColor.Cyan
        Console.WriteLine("COMMAND")
        Console.ResetColor()
        Console.WriteLine("FILES OPTIONS")
        Console.WriteLine()
        Console.WriteLine("Options are case-sensitive. Options and values in square brackets are optional. All parameters that do not start with the ""-"" or ""/"" sign are treated as file names/masks. Options and input files can be placed in any order, but -- (double dash followed by space) indicates the end of parsing options and all subsequent parameters are treated as file names/masks.")
        Console.WriteLine()
        Console.WriteLine("COMMANDS")
        DisplayCommandWithStyle("w", "Write", "Writes a key value", New String() {"Section", "Key", "Value"})
        DisplayCommandWithStyle("r", "Read", "Reads and displays the value of a key.", New String() {"Section", "Key"})
        DisplayCommandWithStyle("rnk", "RenameKey", "Renames the key.", New String() {"Section", "Old Key", "New Key"})
        DisplayCommandWithStyle("rmk", "RemoveKey", "Removes the key.", New String() {"Section", "Key"})
        DisplayCommandWithStyle("rms", "RemoveSection", "Removes the given section.", New String() {"Section"})
        DisplayCommandWithStyle("ras", "RemoveAllSections", "Removes all sections.", Nothing)
        DisplayCommandWithStyle("rs", "ReadSection", "Displays section keys and values.", New String() {"Section"})
        DisplayCommandWithStyle("rk", "ReadKeys", "Displays section keys.", New String() {"Section"})
        DisplayCommandWithStyle("ls", "ListSections", "Displays the names of all sections.", Nothing)
        DisplayCommandWithStyle("wsc", "WriteSectionComment", Nothing, New String() {"Section", "Comment", "Padding"})
        DisplayCommandWithStyle("rsc", "ReadSectionComment", Nothing, New String() {"Section"})
        DisplayCommandWithStyle("wfc", "WriteFileComment", "Adds one line to the file comment.", New String() {"Comment", "Padding"})
        DisplayCommandWithStyle("rfc", "RemoveFileComment", "Clears file comment.", Nothing)
        Console.WriteLine()
        Console.WriteLine("MAIN OPTIONS")
        ListOptions(MainOptions.Section, "Section name.")
        ListOptions(MainOptions.Key, "Key name.")
        ListOptions(MainOptions.NewKey, "Key name.")
        ListOptions(MainOptions.Value, "Key value.")
        ListOptions(MainOptions.Comment, "Section or file comment.")
        ListOptions(MainOptions.Padding, "Padding spaces (for comments). NUM - a positive integer")
        ListOptions(MainOptions.RecursiveDepth, "Recursion depth when searching for files. NUM - a positive integer.")
        ListOptions("     --silent", "Do not display some messages.")
        Console.WriteLine()
        Console.WriteLine("INFO")
        ListOptions("-h", "--help", "Show this help.")
        ListOptions("  ", "--version", "Show application version.")
        ListOptions("  ", "--license", "Display program license.")
        ListOptions("  ", "--home", "Opens program home page in the default browser (original application's homepage)")
        ListOptions("  ", "--github", "Opens the GitHub page with the program's source files.")
        ListOptions("  ", "--org-github", "Opens the Original application's Github page with the program's source files.")
        Console.WriteLine()
        Console.WriteLine("FILES - Any combination of file names/masks.")
        Console.WriteLine("     Eg.: file.ini *config*.ini ""long file name.ini""")
        Console.WriteLine("------------------------------------------------------")
        Console.WriteLine("EXIT CODES")
        Console.WriteLine("   0 - OK - no errors.")
        Console.WriteLine("   1 - Other error.")
        Console.WriteLine("   2 - Syntax error.")
    End Sub
    Enum MainOptions
        Section
        Key
        OldKey
        NewKey
        Value
        Comment
        Padding
        RecursiveDepth
        Unknown
    End Enum
    Enum infoCommands
        Help
        Version
        License
        Home
        dotNetPortGithub
        OriginalApplicationGithub
    End Enum
    Function getOptionFromName(ByVal name As String) As MainOptions
        Select Case name
            Case "Section"
                Return MainOptions.Section
            Case "Key"
                Return MainOptions.Key
            Case "Old Key"
                Return MainOptions.OldKey
            Case "New Key"
                Return MainOptions.NewKey
            Case "Value"
                Return MainOptions.Value
            Case "Comment"
                Return MainOptions.Comment
            Case "Recursive Depth"
                Return MainOptions.RecursiveDepth
            Case "Padding"
                Return MainOptions.Padding
            Case Else
                Return MainOptions.Unknown
        End Select
    End Function
    Function getInfoFromOption(ByVal mainOption As MainOptions) As String()
        Select Case mainOption
            ' String Output Pattern: %Type%, %ShortCommand%, %argsWithCommand%, %AlternativeCommmand&example%
            Case MainOptions.Comment
                Return New String() {"Comment", "-c", "", "--comment=STR"}
            Case MainOptions.Padding
                Return New String() {"Padding Space", "-x", "NUM", ""}
            Case MainOptions.Key
                Return New String() {"Key", "-k", "", "--key=NAME"}
            Case MainOptions.OldKey
                Return New String() {"OldKeyName", "-k", "", ""}
            Case MainOptions.NewKey
                Return New String() {"New Key", "-kn", "", "--new-key-name=NAME"}
            Case MainOptions.RecursiveDepth
                Return New String() {"Recursive Depth", "-rd", "", "--recurse-depth=NUM"}
            Case MainOptions.Section
                Return New String() {"Section", "-s", "", "--section=NAME"}
            Case MainOptions.Value
                Return New String() {"Value", "-v", "", "--value=STR"}
            Case Else
                'Return "???"
                Throw New Exception("Invalid Main Option. Given Option: " & mainOption.ToString)
        End Select
    End Function
    Sub addMainOptiontoCommand(ByVal options As MainOptions)
        Dim optionData = getInfoFromOption(options)
        If optionData(2) <> "" Then
            Console.Write("[" & optionData(1) & " " & optionData(2) & "]")
        Else
            Select Case optionData(1)
                Case "-s"
                    Console.ForegroundColor = ConsoleColor.Green
                Case "-v"
                    Console.ForegroundColor = ConsoleColor.Cyan
                Case "-c"
                    Console.ForegroundColor = ConsoleColor.Magenta
                Case "-k"
                    Console.ForegroundColor = ConsoleColor.Yellow
                Case "-kn"
                    Console.ForegroundColor = ConsoleColor.Yellow
            End Select
            Console.Write(optionData(1) & " " & optionData(0))
            Console.ResetColor()
        End If
    End Sub
    Sub DisplayCommandWithStyle(ByVal command As String, ByVal commandMeaning As String, ByVal description As String, ByVal optionAvailable As String())
        Console.ForegroundColor = ConsoleColor.Cyan
        Console.Write(command)
        Console.ResetColor()
        Console.Write(", ")
        Console.ForegroundColor = ConsoleColor.Cyan
        Console.Write(commandMeaning & "   ")
        Console.ResetColor()
        Console.Write(description & " ")
        Console.ForegroundColor = ConsoleColor.White
        Console.Write("IniMod ")
        Console.ForegroundColor = ConsoleColor.Cyan
        Console.Write(command)
        Console.ResetColor()
        If Not optionAvailable Is Nothing Then
            Console.Write(" FILES")
            For x = 0 To optionAvailable.Count - 1
                Console.Write(" ")
                addMainOptiontoCommand(getOptionFromName(optionAvailable(x)))
            Next
            Console.WriteLine()
        Else
            Console.WriteLine(" FILES")
        End If
    End Sub
    Sub ListOptions(ByVal mainOptions As MainOptions, ByVal description As String)
        Dim optionData = getInfoFromOption(mainOptions)
        If optionData(3) <> Nothing Then
            Console.WriteLine(optionData(1) & "," & vbTab & optionData(3) & vbTab & description)
        Else
            Console.WriteLine(optionData(1) & " " & optionData(2) & vbTab & description)
        End If
    End Sub
    Sub ListOptions(ByVal command As String, ByVal description As String)
        Console.WriteLine("   " & command & "   " & description)
    End Sub
    Sub ListOptions(ByVal command As String, ByVal example As String, ByVal description As String)
        Console.WriteLine("  " & command & ", " & example & "   " & description)
    End Sub
    Sub Main(ByVal args() As String)
        Console.Title = "IniMod.NET v1.0"
        If args.Count = 0 Then
            StartText()
            Console.WriteLine()
            ShowHelp()
            Console.ReadKey()
        End If
        Dim totalFiles As Integer
        For x = 0 To args.Count - 1
            If IO.File.Exists(args(x)) Then
                totalFiles += 1
            End If
        Next
        For x = 0 To args.Count - 1
            If Not IO.File.Exists(args(x)) Then
                Continue For
            End If
            IniLib.file = args(x)
            Console.Write("Processing file " & x & "/" & totalFiles & " : ")
            Console.ForegroundColor = ConsoleColor.White
            Console.WriteLine(IO.Path.GetFullPath(args(x)))
            Console.ResetColor()
            Select Case args(0)
                Case "--help"
                    ShowHelp()
                Case "--version"
                    VersionText()
                Case "--"
                Case "ls" ' List all sections
                    Dim sections As String() = IniLib.GetAllSections()
                    Console.WriteLine("Section list (" & sections.Count & "):")
                    Console.ForegroundColor = ConsoleColor.Green
                    For x2 = 0 To sections.Count - 1
                        Console.WriteLine(sections(x2))
                    Next
                Case "rk" ' List all keys of section
                    Dim keys = IniLib.GetAllKeysOfSections(args(args.Count - 1).Split("=")(1))
                    Console.ForegroundColor = ConsoleColor.Yellow
                    For x2 = 0 To keys.Count - 1
                        Console.WriteLine(keys(x2))
                    Next
                Case "w" ' Write key
                    IniLib.changeKeyValue(args(args.Count - 2).Split("=")(1), args(args.Count - 1).Split("=")(1))
                Case "r" ' Read key
                    Console.ForegroundColor = ConsoleColor.Cyan
                    Console.WriteLine(IniLib.getKeyValueFromSection(args(args.Count - 2).Split("=")(1), args(args.Count - 1).Split("=")(1)))
                Case "rnk" ' Rename Key
                    Console.Write("Renaming key: ")
                    Console.ForegroundColor = ConsoleColor.Yellow
                    Console.Write(args(args.Count - 2).Split("=")(1))
                    Console.ForegroundColor = ConsoleColor.DarkGray
                    Console.Write(" -> ")
                    Console.ForegroundColor = ConsoleColor.Yellow
                    Console.Write(args(args.Count - 1).Split("=")(1))
                    IniLib.RenameKey(args(args.Count - 3).Split("=")(1), args(args.Count - 2).Split("=")(1), args(args.Count - 1).Split("=")(1))
                Case "rmk" ' Remove Key
                    Console.Write("Removing key: ")
                    Console.ForegroundColor = ConsoleColor.Yellow
                    Console.Write(args(args.Count - 1).Split("=")(1))
                    IniLib.RemoveKey(args(args.Count - 2).Split("=")(1), args(args.Count - 1).Split("=")(1))
                Case "rms" ' Remove Section
                    Console.Write("Removing section: ")
                    Console.ForegroundColor = ConsoleColor.Green
                    Console.Write(args(args.Count - 1).Split("=")(1))
                    IniLib.RemoveSection(args(args.Count - 1).Split("=")(1))
                Case "ras" ' Remove all sections
                    Console.WriteLine("Removing all sections.")
                    IniLib.RemoveAllSections()
                Case "rs" ' List all keys & their value of the section
                    Dim keys = IniLib.GetAllKeysOfSections(args(args.Count - 1).Split("=")(1))
                    For x2 = 0 To keys.Count - 1
                        If keys(x2) = "" Then
                            Console.WriteLine()
                        Else
                            Console.ForegroundColor = ConsoleColor.Yellow
                            Console.Write(keys(x2))
                            Console.ForegroundColor = ConsoleColor.DarkGray
                            Console.Write("=")
                            Console.ForegroundColor = ConsoleColor.Cyan
                            Console.WriteLine(IniLib.getKeyValueFromSection(args(args.Count - 1).Split("=")(1), keys(x2)))
                        End If
                    Next
                    Console.ReadKey()
                Case "wsc" ' Write a comment on the given section
                    Console.Write("Writing section comment: ")
                    Console.ForegroundColor = ConsoleColor.Magenta
                    Console.WriteLine(args(args.Count - 2).Split("=")(1))
                    IniLib.writeCommentToSection(args(args.Count - 3).Split("=")(1), args(args.Count - 2).Split("=")(1), args(args.Count - 1).Split("=")(1))
                Case "rsc" ' Remove a comment on the given section
                    Console.Write("Removing section comment: [")
                    Console.ForegroundColor = ConsoleColor.Green
                    Console.Write(args(args.Count - 1).Split("=")(1))
                    Console.ResetColor()
                    Console.WriteLine("]")
                    IniLib.RemoveCommentsFromSection(args(args.Count - 1).Split("=")(1))
                Case "wfc" ' Write file comment
                    Console.Write("Writing file comment: ")
                    Console.ForegroundColor = ConsoleColor.Magenta
                    Console.WriteLine(args(args.Count - 2).Split("=")(1))
                    IniLib.WriteFileComment(args(args.Count - 2).Split("=")(1), args(args.Count - 1).Split("=")(1))
                Case "rfc"
                    Console.WriteLine("Removing file comment.")
                    IniLib.RemoveFileComment()
            End Select
            Console.ResetColor()
        Next
    End Sub

End Module