using System.Collections.Generic;
using System.Linq;
using INITool.Parser.Units;
using INITool.Structure.Properties;

namespace INITool.Structure.Sections
{
    internal class Section : ISerialisableToString
    {
        public const string DefaultSectionName = "_default";
        public static Section FromParsedSectionUnit(SectionUnit sectionUnit) => new Section(sectionUnit.Name);
        public static Section CreateDefault() => new Section(DefaultSectionName);

        public string Name { get; }

        private readonly Dictionary<string, Property> properties;

        public Section(string name)
        {
            Name = name;
            properties = new Dictionary<string, Property>();
        }

        public string SerialiseToString() => $"[{Name}]";

        public void AddProperty(Property property)
        {
            if (HasProperty(property.Name))
                throw new PropertyExistsException(property.Name, Name);

            properties.Add(property.Name, property);
        }

        public Property GetProperty(string name)
        {
            if (!properties.TryGetValue(name, out Property prop))
                throw new PropertyNotFoundException(name, Name);

            return prop;
        }

        public bool HasProperty(string name) => properties.ContainsKey(name);

        public IEnumerable<Property> GetProperties() => properties.Values.AsEnumerable();
    }
}
