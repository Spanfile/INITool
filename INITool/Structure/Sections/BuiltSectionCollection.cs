using System.IO;
using INITool.Structure.Properties;

namespace INITool.Structure.Sections
{
    internal class BuiltSectionCollection : SectionCollection
    {
        public void StartSection(string name, string comment = null) => StartSection(new Section(name, Options) {
            Comment = comment
        });
        public void AddProperty(string name, object value, string comment = null) => AddProperty(new Property(name, value, Options) {
            Comment = comment
        });

        public BuiltSectionCollection(IniOptions options) : base(options)
        {
        }

        public void WriteSerialisedToStream(TextWriter writer)
        {
            foreach (var line in GetSerialisedLines())
                writer.Write(line);
        }
    }
}
