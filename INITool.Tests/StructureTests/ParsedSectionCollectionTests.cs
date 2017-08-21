using System;
using System.IO;
using System.Linq;
using INITool.Parser;
using INITool.Structure.Properties;
using INITool.Structure.Sections;
using Xunit;
// ReSharper disable RedundantArgumentDefaultValue

namespace INITool.Tests.StructureTests
{
    public class ParsedSectionCollectionTests
    {
        [Fact]
        public void TestGetSections()
        {
            using (var reader = new StringReader("[section1]\n[section2]\n[section3]"))
            using (var parser = new Parser.Parser(reader))
            using (var doc = new ParsedSectionCollection(parser, IniOptions.Default))
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
        public void TestGetNonexistentSection()
        {
            using (var reader = new StringReader("[section]"))
            using (var parser = new Parser.Parser(reader))
            using (var doc = new ParsedSectionCollection(parser, IniOptions.Default))
            {
                Assert.Throws<ArgumentException>(() => doc.GetSection("nonexistent"));
            }
        }

        [Fact]
        public void TestDontParseDefaultSection()
        {
            using (var reader = new StringReader($"[{Section.DefaultSectionName}]"))
            using (var parser = new Parser.Parser(reader))
            using (var doc = new ParsedSectionCollection(parser, IniOptions.Default))
            {
                Assert.Throws<InvalidTokenException>(() => doc.GetDefaultSection());
            }
        }

        [Fact]
        public void TestInvalidUnit()
        {
            using (var reader = new StringReader("invalid"))
            using (var parser = new Parser.Parser(reader))
            using (var doc = new ParsedSectionCollection(parser, IniOptions.Default))
            {
                Assert.Throws<InvalidUnitException>(() => doc.GetDefaultSection());
            }
        }

        [Fact]
        public void TestGetSectionsWithProperties()
        {
            using (var reader = new StringReader("[section1]\nvalue=10\n[section2]\nvalue=10\n[section3]\nvalue=10"))
            using (var parser = new Parser.Parser(reader))
            using (var doc = new ParsedSectionCollection(parser, IniOptions.Default))
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

        [Fact]
        public void GetSectionCaseInsensitive()
        {
            using (var reader = new StringReader("[section]"))
            using (var parser = new Parser.Parser(reader))
                // ReSharper disable once ArgumentsStyleLiteral
            using (var doc = new ParsedSectionCollection(parser, new IniOptions(caseSensitive: false)))
            {
                Assert.NotNull(doc.GetSection("SECTION"));
            }
        }

        [Fact]
        public void GetSectionCaseInsensitiveParsing()
        {
            using (var reader = new StringReader("[SECTION]"))
            using (var parser = new Parser.Parser(reader))
                // ReSharper disable once ArgumentsStyleLiteral
            using (var doc = new ParsedSectionCollection(parser, new IniOptions(caseSensitive: false)))
            {
                Assert.NotNull(doc.GetSection("section"));
            }
        }

        [Fact]
        public void GetSectionCaseSensitive()
        {
            using (var reader = new StringReader("[section]"))
            using (var parser = new Parser.Parser(reader))
                // ReSharper disable once ArgumentsStyleLiteral
            using (var doc = new ParsedSectionCollection(parser, new IniOptions(caseSensitive: true)))
            {
                Assert.Throws<ArgumentException>(() => doc.GetSection("SECTION"));
            }
        }

        [Fact]
        public void GetSectionCaseSensitiveParsing()
        {
            using (var reader = new StringReader("[SECTION]"))
            using (var parser = new Parser.Parser(reader))
                // ReSharper disable once ArgumentsStyleLiteral
            using (var doc = new ParsedSectionCollection(parser, new IniOptions(caseSensitive: true)))
            {
                Assert.Throws<ArgumentException>(() => doc.GetSection("section"));
            }
        }
    }
}
