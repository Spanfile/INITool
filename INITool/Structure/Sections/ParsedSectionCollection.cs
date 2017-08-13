using System;
using INITool.Parser;
using INITool.Parser.Units;
using INITool.Structure.Properties;

namespace INITool.Structure.Sections
{
    internal class ParsedSectionCollection : SectionCollection, IDisposable
    {
        private readonly Parser.Parser parser;

        public ParsedSectionCollection(Parser.Parser parser)
        {
            this.parser = parser;
        }

        public void Dispose()
        {
            parser.Dispose();
        }

        public override Section GetSection(string name)
        {
            // the wanted section might not be completely parsed yet
            if (CurrentSection == name)
            {
                ParseCurrentSection();
                return Sections[name];
            }

            if (Sections.TryGetValue(name, out Section section))
                return section;

            if (!ParseSection(name))
                throw new SectionNotFoundException(name);

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

                if ((unit as SectionUnit).Name == name)
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
                    throw new InvalidUnitException(nextUnit);

                case SectionUnit section:
                    StartSection(section);
                    break;

                case AssignmentUnit assignment:
                    AddProperty(assignment);
                    break;
            }

            return nextUnit;
        }

        private void StartSection(SectionUnit sectionUnit) => StartSection(Section.FromParsedSectionUnit(sectionUnit));

        private void AddProperty(AssignmentUnit assignmentUnit) => AddProperty(Property.FromParsedAssignmentUnit(assignmentUnit));
    }
}
