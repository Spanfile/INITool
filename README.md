# INITool
Tool for reading and writing configuration (.ini) files.

## Disclaimer
I will be pushing new versions to this repository as I finish them - I do not use this repository for active development.

## Format
```ini
# comment
; comment
no_section=10
[Section]
# 64-bit integer
int=10
# 64-bit floating point
float=1.1
bool=true
string="text wrapped in single or double quotes. there are also\nescape sequences"
verbatim_string=@"verbatim strings include everything
between the quotation marks
they also ignore \escape sequences"
```

## Supported data types
* `double`
* `long`
* `string`
* `bool`

## Usage
```csharp
using (var reader = new IniReader("sample.ini", IniOptions.Default))
{
    Console.WriteLine(reader.GetInt64("no_section"));
    Console.WriteLine(reader.GetInt64("Section.int"));
    Console.WriteLine(reader.GetString("Section.string"));
    Console.WriteLine(reader.GetBool("Section.bool"));
    Console.WriteLine(reader.GetDouble("Section.float"));

    Console.WriteLine(reader.GetComment("Section.int"));
}

using (var writer = new IniWriter("file.ini", IniOptions.Default))
{
    writer.StartSection("sample");
    writer.AddProperty("string", "Hello, World!", comment: "string");
}
```
