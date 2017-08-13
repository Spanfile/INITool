using System.IO;
using INITool.Structure.Sections;

namespace INITool
{
    public class IniWriter
    {
        private readonly string filename;
        private readonly BuiltSectionCollection sectionCollection;

        public IniWriter(string filename)
        {
            this.filename = filename;
            sectionCollection = new BuiltSectionCollection();
        }

        public void StartSection(string name)
        {
            sectionCollection.StartSection(name);
        }

        public void AddProperty(string name, object value)
        {
            sectionCollection.AddProperty(name, value);
        }

        internal void WriteToStream(TextWriter writer)
        {
            sectionCollection.WriteSerialisedToStream(writer);
        }

        public void Write()
        {
            using (var writer = new StreamWriter(File.OpenWrite(filename)))
            {
                WriteToStream(writer);
            }
        }
    }
}
