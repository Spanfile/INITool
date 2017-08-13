using System.IO;
using System.Linq;
using INITool.Structure.Properties;
using INITool.Structure.Sections;
using Xunit;

namespace INITool.Tests.StructureTests
{
    public class ParsedSectionCollectionTests
    {
        [Fact]
        public void TestGetSections()
        {
            using (var reader = new StringReader("[section1]\n[section2]\n[section3]"))
            using (var parser = new Parser.Parser(reader))
            using (var doc = new ParsedSectionCollection(parser))
            {
                for (var i = 0; i < 3; i++)
                {
                    var name = $"section{i + 1}";
                    var section = doc.GetSection(name);
                    Assert.Equal(name, section.Name);
                }
            }
        }

        [Fact]
        public void TestGetSectionsWithProperties()
        {
            using (var reader = new StringReader("[section1]\nvalue=10\n[section2]\nvalue=10\n[section3]\nvalue=10"))
            using (var parser = new Parser.Parser(reader))
            using (var doc = new ParsedSectionCollection(parser))
            {
                for (var i = 0; i < 3; i++)
                {
                    var name = $"section{i + 1}";
                    var section = doc.GetSection(name);
                    Assert.Equal(name, section.Name);
                    Assert.Equal(1, section.GetProperties().Count());
                    Assert.Equal(true, section.HasProperty("value"));
                    Assert.Equal(10L, section.GetProperty("value").GetValueAs<long>());
                }
            }
        }

        // TODO: these two tests might work better in SectionTests
        [Fact]
        public void TestNonexistantProperty()
        {
            using (var reader = new StringReader("[section]"))
            using (var parser = new Parser.Parser(reader))
            using (var doc = new ParsedSectionCollection(parser))
            {
                var section = doc.GetSection("section");

                Assert.Throws<PropertyNotFoundException>(() => section.GetProperty("nonexistant"));
            }
        }

        [Fact]
        public void TestDuplicateProperty()
        {
            using (var reader = new StringReader("[section]\nvalue=10\nvalue=10"))
            using (var parser = new Parser.Parser(reader))
            using (var doc = new ParsedSectionCollection(parser))
            {
                Assert.Throws<PropertyExistsException>(() => doc.GetSection("section"));
            }
        }
    }
}
