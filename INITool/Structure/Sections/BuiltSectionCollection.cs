using System.IO;
using INITool.Structure.Properties;

namespace INITool.Structure.Sections
{
    internal class BuiltSectionCollection : SectionCollection
    {
        public void StartSection(string name) => StartSection(new Section(name));
        public void AddProperty(string name, object value) => AddProperty(new Property(name, value));

        public void WriteSerialisedToStream(TextWriter writer)
        {
            foreach (var line in GetSerialisedLines())
                writer.Write(line);
        }
    }
}
