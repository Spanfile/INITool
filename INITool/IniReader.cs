using System;
using INITool.Structure.Properties;
using INITool.Structure.Sections;

namespace INITool
{
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
                    return (args[0], null);
            }
        }

        private readonly ParsedSectionCollection sections;

        public IniReader(string filename, IniOptions options)
        {
            sections = new ParsedSectionCollection(Parser.Parser.FromFile(filename), options);
        }

        public void Dispose()
        {
            sections.Dispose();
        }

        //public short GetInt16(string identifier) => GetProperty(identifier).GetValueAs<short>();

        //public ushort GetUInt16(string identifier) => GetProperty(identifier).GetValueAs<ushort>();

        //public int GetInt32(string identifier) => GetProperty(identifier).GetValueAs<int>();

        //public uint GetUInt32(string identifier) => GetProperty(identifier).GetValueAs<uint>();

        public long GetInt64(string identifier) => GetProperty(identifier).GetValueAs<long>();

        //public ulong GetUInt64(string identifier) => GetProperty(identifier).GetValueAs<ulong>();

        public string GetString(string identifier) => GetProperty(identifier).GetValueAs<string>();

        //public float GetFloat(string identifier) => GetProperty(identifier).GetValueAs<float>();

        public double GetDouble(string identifier) => GetProperty(identifier).GetValueAs<double>();

        public bool GetBool(string identifier) => GetProperty(identifier).GetValueAs<bool>();

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
