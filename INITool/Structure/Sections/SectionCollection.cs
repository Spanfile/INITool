using System.Collections.Generic;
using System.Linq;
using System.Text;
using INITool.Structure.Properties;

namespace INITool.Structure.Sections
{
    internal abstract class SectionCollection : ISerialisableToString
    {
        protected readonly Dictionary<string, Section> Sections;
        protected string CurrentSection;

        protected SectionCollection()
        {
            Sections = new Dictionary<string, Section> {{Section.DefaultSectionName, Section.CreateDefault()}};
            CurrentSection = Section.DefaultSectionName;
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
            foreach (var section in Sections.Values)
            {
                // omit default section
                if (section.Name != Section.DefaultSectionName)
                    yield return $"{section.SerialiseToString()}\n";

                foreach (var property in section.GetProperties())
                    yield return $"{property.SerialiseToString()}\n";
            }
        }

        public virtual IEnumerable<Section> GetSections() => Sections.Values.AsEnumerable();

        public virtual Section GetSection(string name)
        {
            if (Sections.TryGetValue(name, out Section section))
                return section;

            throw new SectionNotFoundException(name);
        }

        protected virtual void StartSection(Section sectionUnit)
        {
            Sections.Add(sectionUnit.Name, sectionUnit);
            CurrentSection = sectionUnit.Name;
        }

        protected virtual void AddProperty(Property property)
        {
            Sections[CurrentSection].AddProperty(property);
        }
    }
}
