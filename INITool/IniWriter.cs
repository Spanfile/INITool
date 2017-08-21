using System;
using System.IO;
using INITool.Structure.Sections;

namespace INITool
{
    public class IniWriter : IDisposable
    {
        private readonly BuiltSectionCollection sectionCollection;
        private readonly TextWriter textWriter;

        private IniWriter(IniOptions options)
        {
            sectionCollection = new BuiltSectionCollection(options);
        }

        public IniWriter(string filename, IniOptions options)
            : this(options)
        {
            textWriter = new StreamWriter(File.OpenRead(filename));
        }

        public IniWriter(TextWriter textWriter, IniOptions options)
            : this(options)
        {
            this.textWriter = textWriter;
        }

        public void Dispose()
        {
            Write();
            textWriter.Dispose();
        }

        public void StartSection(string name, string comment = null)
        {
            sectionCollection.StartSection(name, comment);
        }

        public void AddProperty(string name, object value, string comment = null)
        {
            sectionCollection.AddProperty(name, value, comment);
        }

        public void Write()
        {
            sectionCollection.WriteSerialisedToStream(textWriter);
        }

        public void Flush() => textWriter.Flush();
    }
}
