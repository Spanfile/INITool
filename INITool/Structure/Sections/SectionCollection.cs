using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using INITool.Structure.Properties;

namespace INITool.Structure.Sections
{
    internal abstract class SectionCollection : ISerialisableToString
    {
        protected StringComparison StringComparison => Options.CaseSensitive
            ? StringComparison.InvariantCulture
            : StringComparison.InvariantCultureIgnoreCase;

        protected readonly Dictionary<string, Section> Sections;
        protected string CurrentSection;
        protected IniOptions Options;

        protected SectionCollection(IniOptions options)
        {
            Sections = new Dictionary<string, Section> {{Section.DefaultSectionName, Section.CreateDefault(options)}};
            CurrentSection = Section.DefaultSectionName;
            Options = options;
        }

        public virtual string SerialiseToString()
        {
            var builder = new StringBuilder();
            foreach (var line in GetSerialisedLines())
                builder.Append(line);

            return builder.ToString();
        }

        protected IEnumerable<string> GetSerialisedLines()
        {
            string lineEnding;
            switch (Options.LineEndings)
            {
                case LineEnding.LF:
                    lineEnding = "\n";
                    break;

                case LineEnding.CR:
                    lineEnding = "\r";
                    break;

                case LineEnding.CRLF:
                    lineEnding = "\r\n";
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            foreach (var section in Sections.Values)
            {
                // omit default section
                if (!section.Name.Equals(Section.DefaultSectionName, StringComparison.InvariantCultureIgnoreCase))
                    yield return $"{section.SerialiseToString()}{lineEnding}";

                foreach (var property in section.GetProperties())
                    yield return $"{property.SerialiseToString()}{lineEnding}";
            }
        }

        public virtual IEnumerable<Section> GetSections() => Sections.Values.AsEnumerable();

        public virtual Section GetSection(string name)
        {
            if (Sections.TryGetValue(name, out var section))
                return section;

            throw new ArgumentException($"section not found: {name}", nameof(name));
        }

        protected virtual void StartSection(Section sectionUnit)
        {
            if (sectionUnit.Name.Equals(Section.DefaultSectionName, StringComparison.InvariantCultureIgnoreCase))
                throw new ArgumentException("cannot start default section");

            var name = Options.CaseSensitive ? sectionUnit.Name : sectionUnit.Name.ToLowerInvariant();

            Sections.Add(name, sectionUnit);
            CurrentSection = name;
        }

        protected virtual void AddProperty(Property property)
        {
            Sections[CurrentSection].AddProperty(property);
        }
    }
}
