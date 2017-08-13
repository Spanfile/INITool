# INITool
Tool for reading and writing configuration (.ini) files.

# Disclaimer
I will be pushing new versions to this repository as I finish them - I do not use this repository for active development.

## Format
```ini
# comment
; comment
[Section]
int=10
float=1.1
bool=true
bool=false
string="text wrapped in single or double quotes"
```

## Supported data types
* `double`
* `long`
* `string`
* `bool`

## Usage
```csharp
using (var reader = new IniReader("sample.ini"))
{
    Console.WriteLine(reader.GetInt64("Section.int"));
    Console.WriteLine(reader.GetString("Section.string"));
    Console.WriteLine(reader.GetBool("Section.bool"));
    Console.WriteLine(reader.GetDouble("Section.float"));
}
```
