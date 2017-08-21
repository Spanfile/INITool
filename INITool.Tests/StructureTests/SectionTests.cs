using System;
using System.IO;
using INITool.Parser.Units;
using INITool.Structure.Properties;
using INITool.Structure.Sections;
using Xunit;

namespace INITool.Tests.StructureTests
{
    public class SectionTests
    {
        [Fact]
        public void TestFromParsedSection()
        {
            using (var reader = new StringReader("#comment\n[section]"))
            using (var parser = new Parser.Parser(reader))
            {
                var section = Section.FromParsedSectionUnit(parser.ParseUnitOfType<SectionUnit>(), IniOptions.Default);

                Assert.Equal("comment", section.Comment);
                Assert.Equal("section", section.Name);
            }
        }

        [Fact]
        public void TestDuplicateProperty()
        {
            var section = Section.CreateDefault(IniOptions.Default);

            section.AddProperty(new Property("a", "a", IniOptions.Default));
            Assert.Throws<ArgumentException>(() => section.AddProperty(new Property("a", "a", IniOptions.Default)));
        }

        [Fact]
        public void TestNonexistantProperty()
        {
            var section = Section.CreateDefault(IniOptions.Default);

            Assert.Throws<ArgumentException>(() => section.GetProperty("a"));
        }
    }
}
