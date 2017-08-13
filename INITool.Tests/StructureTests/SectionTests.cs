using System.IO;
using INITool.Parser.Units;
using INITool.Structure.Sections;
using Xunit;

namespace INITool.Tests.StructureTests
{
    public class SectionTests
    {
        [Fact]
        public void TestFromParsedSection()
        {
            using (var reader = new StringReader("[section]"))
            using (var parser = new Parser.Parser(reader))
            {
                var section = Section.FromParsedSectionUnit(parser.ParseUnitOfType<SectionUnit>());

                Assert.Equal("section", section.Name);
            }
        }
    }
}
