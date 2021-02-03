# IniMod.NET
This is a full port of [Jacek Pazera](https://github.com/jackdp), [IniMod](https://github.com/jackdp/IniMod) application (currently in version 1.0), this port does NOT bring anything to the application. This is suposed to be a faithful port, nothing else

# Possible Improvements
- Most algorithms, especially the comments-related ones, i would say they're can be improved on a lot more
- If short commands does not work, use its full alternative (for example, `-k=%KeyName%` might not work for some reason, so you'll have to use `--key=%KeyName%` instead (they should work, but just incase)
- More error messages

# Compatibility
Features | .NET Port  | Original Application
------------- | ------------- | -------------
Can Write Keys | Yes | Yes
Can Read Keys  | Yes | Yes
Can Rename Keys | Yes | Yes
Can Delete Keys | Yes | Yes
||
Can List Sections | Yes | Yes
Can Remove Sections | Yes | Yes
Can Remove All Sections | Yes | Yes
Can Read Sections | Yes | Yes
||
Can Write Section Comment | Yes | Yes
Can Read Section Comment | Yes | Yes
Can Write File Comment | Yes | Yes
Can Remove File Comment | Yes | Yes
||
Has Error Messages | No (barely any) | Yes
# Credits
- [Jacek Pazera](https://github.com/jackdp) - Creating [IniMod](https://github.com/jackdp/IniMod)
