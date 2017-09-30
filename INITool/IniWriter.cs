using System;
using System.IO;
using INITool.Structure.Sections;

namespace INITool
{
    /// <inheritdoc />
    /// <summary>
    /// Writes configuration files
    /// </summary>
    public class IniWriter : IDisposable
    {
        private readonly BuiltSectionCollection sectionCollection;
        private readonly TextWriter textWriter;

        private IniWriter(IniOptions options)
        {
            sectionCollection = new BuiltSectionCollection(options);
        }

        /// <summary>
        /// Create a new configuration file writer
        /// </summary>
        /// <param name="filename">Path to the configuration file to write</param>
        /// <param name="options">Options to use when writing</param>
        public IniWriter(string filename, IniOptions options)
            : this(options)
        {
            textWriter = new StreamWriter(File.OpenRead(filename));
        }

        /// <summary>
        /// Create a new configuration file writer
        /// </summary>
        /// <param name="textWriter">The text writer to use</param>
        /// <param name="options">Options to use when writing</param>
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

        /// <summary>
        /// Start a new section
        /// </summary>
        /// <param name="name">The section's name</param>
        /// <param name="comment">Optional comment for the section</param>
        public void StartSection(string name, string comment = null)
        {
            sectionCollection.StartSection(name, comment);
        }

        /// <summary>
        /// Add a new property to the current section
        /// </summary>
        /// <param name="name">The property's name</param>
        /// <param name="value">The property's value</param>
        /// <param name="comment">Optional comment for the property</param>
        public void AddProperty(string name, object value, string comment = null)
        {
            sectionCollection.AddProperty(name, value, comment);
        }

        /// <summary>
        /// Write the current configuration structure to file
        /// </summary>
        public void Write()
        {
            sectionCollection.WriteSerialisedToStream(textWriter);
        }

        /// <summary>
        /// Flush the current text writer
        /// </summary>
        public void Flush() => textWriter.Flush();
    }
}
