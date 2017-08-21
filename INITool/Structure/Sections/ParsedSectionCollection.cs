using System;
using INITool.Parser;
using INITool.Parser.Units;
using INITool.Structure.Properties;

namespace INITool.Structure.Sections
{
    internal class ParsedSectionCollection : SectionCollection, IDisposable
    {
        private readonly Parser.Parser parser;

        public ParsedSectionCollection(Parser.Parser parser, IniOptions options) : base(options)
        {
            this.parser = parser;
        }

        public void Dispose()
        {
            parser.Dispose();
        }

        public override Section GetSection(string name)
        {
            if (!Options.CaseSensitive)
                name = name.ToLowerInvariant();

            // the wanted section might not be completely parsed yet
            if (CurrentSection.Equals(name, StringComparison))
            {
                ParseCurrentSection();
                return Sections[name];
            }

            if (Sections.TryGetValue(name, out var section))
                return section;

            if (!ParseSection(name))
                throw new ArgumentException($"section not found: {name}", nameof(name));

            return Sections[name];
        }

        public Section GetDefaultSection() => GetSection(Section.DefaultSectionName);

        private bool ParseSection(string name)
        {
            if (parser.EndOfDocument)
                return false;

            while (true)
            {
                Unit unit;
                try
                {
                    unit = ParseNext();
                }
                catch (EndOfDocumentException)
                {
                    return false;
                }

                if (!(unit is SectionUnit))
                    continue;

                if ((unit as SectionUnit).Name.Equals(name, StringComparison))
                    break;
            }

            ParseCurrentSection();
            return true;
        }

        private void ParseCurrentSection()
        {
            var section = CurrentSection;

            do
            {
                try
                {
                    ParseNext();
                }
                catch (EndOfDocumentException)
                {
                    return;
                }
            } while (CurrentSection == section);
        }

        private Unit ParseNext()
        {
            var nextUnit = parser.ParseUnit();
            switch (nextUnit)
            {
                default:
                    throw new InvalidUnitException(nextUnit, typeof(SectionUnit), typeof(AssignmentUnit));

                case SectionUnit section:
                    StartSection(section, Options);
                    break;

                case AssignmentUnit assignment:
                    AddProperty(assignment, Options);
                    break;
            }

            return nextUnit;
        }

        private void StartSection(SectionUnit sectionUnit, IniOptions options) => StartSection(Section.FromParsedSectionUnit(sectionUnit, options));

        private void AddProperty(AssignmentUnit assignmentUnit, IniOptions options) => AddProperty(Property.FromParsedAssignmentUnit(assignmentUnit, options));
    }
}
