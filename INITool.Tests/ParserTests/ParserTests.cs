using System.IO;
using INITool.Parser;
using INITool.Parser.Tokeniser;
using INITool.Parser.Units;
using INITool.Structure.Sections;
using NUnit.Framework;
// ReSharper disable ArgumentsStyleNamedExpression

namespace INITool.Tests.ParserTests
{
    [TestFixture]
    public class ParserTests
    {
        [Test]
        public void TestSkipLeadingWhitespace()
        {
            using (var reader = new StringReader(" \t\f\v[section]"))
            using (var parser = new Parser.Parser(reader, IniOptions.Default))
            {
                var unit = parser.ParseUnitOfType<SectionUnit>();
                Assert.AreEqual("[section]", unit.SourceTokenString());
                Assert.AreEqual("section", unit.Name);
            }
        }

        [Test]
        public void TestNoSkipLeadingWhitespaceStrongParsing()
        {
            using (var reader = new StringReader(" \t\f\va"))
            using (var parser = new Parser.Parser(reader, IniOptions.Default))
            {
                Assert.Throws<InvalidTokenException>(() => parser.ParseUnitOfType<NameUnit>(skipWhitespace: false));
            }
        }

        [Test]
        public void TestNoSkipLeadingWhitespaceFlexibleParsing()
        {
            var options = new IniOptions(propertyParsing: PropertyParsing.Flexible);

            using (var reader = new StringReader(" \t\f\va"))
            using (var parser = new Parser.Parser(reader, options))
            {
                var unit = parser.ParseUnitOfType<ValueUnit>(skipWhitespace: false);
                Assert.AreEqual(" \t\f\va", unit.SourceTokenString());
                Assert.AreEqual(" \t\f\va", unit.Value);
            }
        }

        [Test]
        public void TestSkipLeadingNewline()
        {
            using (var reader = new StringReader("\n[section]"))
            using (var parser = new Parser.Parser(reader, IniOptions.Default))
            {
                var unit = parser.ParseUnitOfType<SectionUnit>();
                Assert.AreEqual("[section]", unit.SourceTokenString());
                Assert.AreEqual("section", unit.Name);
            }
        }

        [Test]
        public void TestNoSkipLeadingNewlineStrongParsing()
        {
            using (var reader = new StringReader("\na"))
            using (var parser = new Parser.Parser(reader, IniOptions.Default))
            {
                Assert.Throws<InvalidTokenException>(() => parser.ParseUnitOfType<NameUnit>(skipNewline: false));
            }
        }

        [Test]
        public void TestNoSkipLeadingNewlineFlexibleParsing()
        {
            var options = new IniOptions(propertyParsing: PropertyParsing.Flexible);

            using (var reader = new StringReader("\na"))
            using (var parser = new Parser.Parser(reader, options))
            {
                var unit = parser.ParseUnitOfType<ValueUnit>(skipNewline: false);
                Assert.AreEqual("\na", unit.SourceTokenString());
                Assert.AreEqual("\na", unit.Value);
            }
        }

        [Test]
        public void TestParseMultipleUnits()
        {
            using (var reader = new StringReader("[section] [section]\n[section]"))
            using (var parser = new Parser.Parser(reader, IniOptions.Default))
            {
                for (var i = 0; i < 3; i++)
                {
                    var unit = parser.ParseUnitOfType<SectionUnit>();
                    Assert.AreEqual("[section]", unit.SourceTokenString());
                    Assert.AreEqual("section", unit.Name);
                }
            }
        }

        [Test]
        public void TestParseOfType()
        {
            using (var reader = new StringReader("[section]"))
            using (var parser = new Parser.Parser(reader, IniOptions.Default))
            {
                Assert.Throws<InvalidUnitException>(() => parser.ParseUnitOfType<NameUnit>());
            }
        }

        [Test]
        public void TestParseNameUnit()
        {
            using (var reader = new StringReader("name123_"))
            using (var parser = new Parser.Parser(reader, IniOptions.Default))
            {
                var unit = parser.ParseUnitOfType<NameUnit>(allowSingleUnitTransform: false);
                Assert.AreEqual("name123_", unit.SourceTokenString());
                Assert.AreEqual("name123_", unit.Name);
            }
        }

        [Test]
        public void TestParseSectionUnit()
        {
            using (var reader = new StringReader("[section]"))
            using (var parser = new Parser.Parser(reader, IniOptions.Default))
            {
                var unit = parser.ParseUnitOfType<SectionUnit>();
                Assert.AreEqual("[section]", unit.SourceTokenString());
                Assert.AreEqual("section", unit.Name);
            }
        }

        [Test]
        public void TestParseIncompleteSectionUnit()
        {
            using (var reader = new StringReader("[section"))
            using (var parser = new Parser.Parser(reader, IniOptions.Default))
            {
                Assert.Throws<InvalidTokenException>(() => parser.ParseUnitOfType<SectionUnit>());
            }
        }

        [Test]
        public void TestDontParseDefaultSection()
        {
            using (var reader = new StringReader($"[{Section.DefaultSectionName}]"))
            using (var parser = new Parser.Parser(reader, IniOptions.Default))
            {
                Assert.Throws<InvalidTokenException>(() => parser.ParseUnitOfType<SectionUnit>());
            }
        }

        [Test]
        public void TestComments()
        {
            using (var reader = new StringReader("#comment\n#comment\n[section]"))
            using (var parser = new Parser.Parser(reader, IniOptions.Default))
            {
                var unit = parser.ParseUnitOfType<SectionUnit>();

                Assert.AreEqual("comment\ncomment", unit.Comment);
            }
        }

        [Test]
        public void TestPositions()
        {
            using (var reader = new StringReader("#comment\n[section]"))
            using (var parser = new Parser.Parser(reader, IniOptions.Default))
            {
                var unit = parser.ParseUnitOfType<SectionUnit>();

                Assert.AreEqual(new Position(1, 0), unit.Position);
            }
        }
    }
}
