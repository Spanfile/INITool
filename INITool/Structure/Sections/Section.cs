using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using INITool.Parser.Units;
using INITool.Structure.Properties;

namespace INITool.Structure.Sections
{
    internal class Section : StructurePiece, ISerialisableToString
    {
        public const string DefaultSectionName = "_default";

        public static Section FromParsedSectionUnit(SectionUnit sectionUnit, IniOptions options) => new Section(sectionUnit.Name, options) {
            Comment = sectionUnit.Comment
        };
        public static Section CreateDefault(IniOptions options) => new Section(DefaultSectionName, options);

        public string Name { get; }

        private readonly Dictionary<string, Property> properties;

        public Section(string name, IniOptions options) : base(options)
        {
            Name = CaseSensitiviseString(name);
            properties = new Dictionary<string, Property>();
        }

        public string SerialiseToString()
        {
            var sb = new StringBuilder();

            AppendCommentToStringBuilder(sb);

            sb.Append("[").Append(Name).Append("]");
            return sb.ToString();
        }

        public void AddProperty(Property property)
        {
            if (HasProperty(property.Name))
                throw new ArgumentException($"property {property.Name} already exists in {Name}", nameof(property));

            properties.Add(property.Name, property);
        }

        public Property GetProperty(string name)
        {
            if (!properties.TryGetValue(name, out var prop))
                throw new ArgumentException($"property {name} not found in {Name}", nameof(name));

            return prop;
        }

        public bool HasProperty(string name) => properties.ContainsKey(name);

        public IEnumerable<Property> GetProperties() => properties.Values.AsEnumerable();
    }
}
