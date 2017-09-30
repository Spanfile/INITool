// ReSharper disable InconsistentNaming
// ReSharper disable ArgumentsStyleLiteral
// ReSharper disable ArgumentsStyleNamedExpression
// ReSharper disable RedundantArgumentDefaultValue

namespace INITool
{
    /// <summary>
    /// Line endings
    /// </summary>
    public enum LineEnding
    {
        /// <summary>
        /// Carriage Return
        /// </summary>
        CR,
        /// <summary>
        /// Line Feed
        /// </summary>
        LF,
        /// <summary>
        /// Carriage Return followed by a Line Feed
        /// </summary>
        CRLF
    }

    /// <summary>
    /// Policies on what to serialise special strings to
    /// </summary>
    public enum StringSerialisationPolicy
    {
        /// <summary>
        /// Serialise strings to verbatim strings
        /// </summary>
        ToVerbatim,
        /// <summary>
        /// Replace special characters with escape sequences
        /// </summary>
        ToEscapeSequences
    }

    /// <summary>
    /// Property parsing method
    /// </summary>
    public enum PropertyParsing
    {
        /// <summary>
        /// Require all values be of strong types
        /// </summary>
        Strong,
        /// <summary>
        /// Assume all values are strings. Allows for any kinds of values
        /// </summary>
        Flexible
    }

    /// <summary>
    /// Option collection for INITool
    /// </summary>
    public class IniOptions
    {
        /// <summary>
        /// Default option template
        /// </summary>
        public static IniOptions Default => new IniOptions();
        /// <summary>
        /// Flexible option template
        /// </summary>
        public static IniOptions Flexible => new IniOptions(
            allowValueConversion: true,
            propertyParsing: PropertyParsing.Flexible,
            throwAtNonexistentProperty: false,
            allowLooseProperties: true);
        /// <summary>
        /// Windows option template
        /// </summary>
        public static IniOptions Windows => new IniOptions(caseSensitive: false, lineEndings: LineEnding.CRLF);
        /// <summary>
        /// Unix option template
        /// </summary>
        public static IniOptions Unix => new IniOptions(caseSensitive: true, lineEndings: LineEnding.LF);

        /// <summary>
        /// Case-sensitivity when parsing
        /// </summary>
        public bool CaseSensitive { get; }
        /// <summary>
        /// Line endings to use when writing
        /// </summary>
        public LineEnding LineEndings { get; }
        /// <summary>
        /// String serialisation policy to use when writing
        /// </summary>
        public StringSerialisationPolicy StringSerialisationPolicy { get; }
        /// <summary>
        /// Allow value conversion for compatible values when reading
        /// </summary>
        public bool AllowValueConversion { get; }
        /// <summary>
        /// Property parsing method to use when parsing
        /// </summary>
        public PropertyParsing PropertyParsing { get; }
        /// <summary>
        /// If set, throw an exception when trying to access a nonexistent property
        /// </summary>
        public bool ThrowAtNonexistentProperty { get; }
        /// <summary>
        /// If set, allow loose properties
        /// </summary>
        public bool AllowLooseProperties { get; }

        /// <summary>
        /// Construct a new IniOptions object
        /// </summary>
        /// <param name="caseSensitive">Case-sensitivity when parsing</param>
        /// <param name="lineEndings">Line endings to use when writing</param>
        /// <param name="stringSerialisationPolicy">String serialisation policy to use when writing</param>
        /// <param name="allowValueConversion">Allow value conversion for compatible values when reading</param>
        /// <param name="propertyParsing">Property parsing method to use when parsing</param>
        /// <param name="throwAtNonexistentProperty">If set, throw an exception when trying to access a nonexistent property</param>
        /// <param name="allowLooseProperties">If set, allow loose properties</param>
        public IniOptions(
            bool caseSensitive = true,
            LineEnding lineEndings = LineEnding.LF,
            StringSerialisationPolicy stringSerialisationPolicy = StringSerialisationPolicy.ToEscapeSequences,
            bool allowValueConversion = false,
            PropertyParsing propertyParsing = PropertyParsing.Strong,
            bool throwAtNonexistentProperty = true,
            bool allowLooseProperties = false)
        {
            CaseSensitive = caseSensitive;
            LineEndings = lineEndings;
            StringSerialisationPolicy = stringSerialisationPolicy;
            AllowValueConversion = allowValueConversion;
            PropertyParsing = propertyParsing;
            ThrowAtNonexistentProperty = throwAtNonexistentProperty;
            AllowLooseProperties = allowLooseProperties;
        }
    }
}
