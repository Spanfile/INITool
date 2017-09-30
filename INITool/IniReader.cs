using System;
using INITool.Structure.Properties;
using INITool.Structure.Sections;

namespace INITool
{
    /// <inheritdoc />
    /// <summary>
    /// Reads configuration files
    /// </summary>
    public class IniReader : IDisposable
    {
        private static (string section, string property) ParsePropertyIdentifier(string identifier)
        {
            var args = identifier.Split('.');
            switch (args.Length)
            {
                default:
                    throw new InvalidPropertyIdentifierException(identifier);

                case 2:
                    return (args[0], args[1]);

                case 1:
                    if (string.IsNullOrWhiteSpace(args[0]))
                        throw new InvalidPropertyIdentifierException(identifier);

                    return (Section.DefaultSectionName, args[0]);
            }
        }

        private static (string section, string property) ParseCommentableIdentifier(string identifier)
        {
            var args = identifier.Split('.');
            switch (args.Length)
            {
                default:
                    throw new InvalidPropertyIdentifierException(identifier);

                case 2:
                    return (args[0], args[1]);

                case 1:
                    if (string.IsNullOrWhiteSpace(args[0]))
                        throw new InvalidPropertyIdentifierException(identifier);

                    return (args[0], null);
            }
        }

        private readonly ParsedSectionCollection sections;

        /// <summary>
        /// Creates a new configuration file reader
        /// </summary>
        /// <param name="filename">Path to a configuration file</param>
        /// <param name="options">Options to use when reading</param>
        public IniReader(string filename, IniOptions options)
        {
            sections = new ParsedSectionCollection(Parser.Parser.FromFile(filename, options), options);
        }

        public void Dispose()
        {
            sections.Dispose();
        }

        //public short GetInt16(string identifier) => GetProperty(identifier).GetValueAs<short>();

        //public ushort GetUInt16(string identifier) => GetProperty(identifier).GetValueAs<ushort>();

        //public int GetInt32(string identifier) => GetProperty(identifier).GetValueAs<int>();

        //public uint GetUInt32(string identifier) => GetProperty(identifier).GetValueAs<uint>();

        /// <summary>
        /// Read a 64-bit signed integer
        /// </summary>
        /// <param name="identifier">Property identifier of form section.property. If no section is given, default section is assumed</param>
        /// <returns></returns>
        public long? GetInt64(string identifier) => (long)GetProperty(identifier)?.GetValueAs<long>();

        //public ulong GetUInt64(string identifier) => GetProperty(identifier).GetValueAs<ulong>();

        /// <summary>
        /// Read a string
        /// </summary>
        /// <param name="identifier">Property identifier of form section.property. If no section is given, default section is assumed</param>
        /// <returns></returns>
        public string GetString(string identifier) => (string)GetProperty(identifier)?.GetValueAs<string>();

        //public float GetFloat(string identifier) => GetProperty(identifier).GetValueAs<float>();

        /// <summary>
        /// Read a 64-bit IEEE floating point number
        /// </summary>
        /// <param name="identifier">Property identifier of form section.property. If no section is given, default section is assumed</param>
        /// <returns></returns>
        public double? GetDouble(string identifier) => (double)GetProperty(identifier)?.GetValueAs<double>();

        /// <summary>
        /// Read a boolean
        /// </summary>
        /// <param name="identifier">Property identifier of form section.property. If no section is given, default section is assumed</param>
        /// <returns></returns>
        public bool? GetBool(string identifier) => (bool)GetProperty(identifier)?.GetValueAs<bool>();

        /// <summary>
        /// Get the comment, if any, for a given property
        /// </summary>
        /// <param name="identifier">Property identifier of form section.property. If no section is given, default section is assumed</param>
        /// <returns></returns>
        public string GetComment(string identifier)
        {
            var (sectionId, propertyId) = ParseCommentableIdentifier(identifier);
            
            // test default section first
            // NOTE: we pass sectionId because it actually functions as the property ID in the default section
            if (sections.GetDefaultSection().HasProperty(sectionId))
                return sections.GetDefaultSection().GetProperty(sectionId).Comment;

            var section = sections.GetSection(sectionId);
            return propertyId != null ? section.GetProperty(propertyId).Comment : section.Comment;
        }

        private Property GetProperty(string identifier)
        {
            var (section, property) = ParsePropertyIdentifier(identifier);
            return sections.GetSection(section).GetProperty(property);
        }
    }
}
