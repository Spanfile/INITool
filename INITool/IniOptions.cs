// ReSharper disable InconsistentNaming

namespace INITool
{
    public enum LineEnding
    {
        CR, LF, CRLF
    }

    public enum StringSerialisationPolicy
    {
        ToVerbatim, ToEscapeSequences
    }

    public class IniOptions
    {
        public static IniOptions Default => new IniOptions();

        public bool CaseSensitive { get; }
        public LineEnding LineEndings { get; }
        public StringSerialisationPolicy StringSerialisationPolicy { get; }

        public IniOptions(
            bool caseSensitive = true,
            LineEnding lineEndings = LineEnding.LF,
            StringSerialisationPolicy stringSerialisationPolicy = StringSerialisationPolicy.ToEscapeSequences)
        {
            CaseSensitive = caseSensitive;
            LineEndings = lineEndings;
            StringSerialisationPolicy = stringSerialisationPolicy;
        }
    }
}
