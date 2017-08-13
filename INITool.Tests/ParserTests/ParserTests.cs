using System.IO;
using INITool.Parser;
using INITool.Parser.Units;
using Xunit;

namespace INITool.Tests.ParserTests
{
    public class ParserTests
    {
        [Fact]
        public void TestLeadingWhitespace()
        {
            using (var reader = new StringReader(" \t\f\va"))
            using (var parser = new Parser.Parser(reader))
            {
                var unit = parser.ParseUnitOfType<NameUnit>();
                Assert.Equal("a", unit.SourceTokenString());
                Assert.Equal("a", unit.Name);
            }
        }

        [Fact]
        public void TestLeadingNewline()
        {
            using (var reader = new StringReader("\na"))
            using (var parser = new Parser.Parser(reader))
            {
                var unit = parser.ParseUnitOfType<NameUnit>();
                Assert.Equal("a", unit.SourceTokenString());
                Assert.Equal("a", unit.Name);
            }
        }

        [Fact]
        public void TestParseMultipleUnits()
        {
            using (var reader = new StringReader("a a\na"))
            using (var parser = new Parser.Parser(reader))
            {
                for (var i = 0; i < 3; i++)
                {
                    var unit = parser.ParseUnitOfType<NameUnit>();
                    Assert.Equal("a", unit.SourceTokenString());
                    Assert.Equal("a", unit.Name);
                }
            }
        }

        [Fact]
        public void TestParseOfType()
        {
            using (var reader = new StringReader("[section]"))
            using (var parser = new Parser.Parser(reader))
            {
                Assert.Throws<UnexpectedUnitException>(() => parser.ParseUnitOfType<NameUnit>());
            }
        }

        [Fact]
        public void TestParseNameUnit()
        {
            using (var reader = new StringReader("name123"))
            using (var parser = new Parser.Parser(reader))
            {
                var unit = parser.ParseUnitOfType<NameUnit>();
                Assert.Equal("name123", unit.SourceTokenString());
                Assert.Equal("name123", unit.Name);
            }
        }

        [Fact]
        public void TestParseSectionUnit()
        {
            using (var reader = new StringReader("[section]"))
            using (var parser = new Parser.Parser(reader))
            {
                var unit = parser.ParseUnitOfType<SectionUnit>();
                Assert.Equal("[section]", unit.SourceTokenString());
                Assert.Equal("section", unit.Name);
            }
        }
    }
}
