using System;
using System.IO;
using System.Linq;
using INITool.Parser;
using INITool.Structure.Sections;
using NUnit.Framework;
// ReSharper disable RedundantArgumentDefaultValue
// ReSharper disable ArgumentsStyleNamedExpression
// ReSharper disable ArgumentsStyleLiteral

namespace INITool.Tests.StructureTests
{
    [TestFixture]
    public class ParsedSectionCollectionTests
    {
        [Test]
        public void TestGetSection()
        {
            using (var reader = new StringReader("[section1]\n[section2]\n[section3]"))
            using (var parser = new Parser.Parser(reader, IniOptions.Default))
            using (var doc = new ParsedSectionCollection(parser, IniOptions.Default))
            {
                for (var i = 0; i < 3; i++)
                {
                    var name = $"section{i + 1}";
                    var section = doc.GetSection(name);
                    Assert.AreEqual(name, section.Name);
                }
            }
        }

        [Test]
        public void TestGetNonexistentSection()
        {
            using (var reader = new StringReader("[section]"))
            using (var parser = new Parser.Parser(reader, IniOptions.Default))
            using (var doc = new ParsedSectionCollection(parser, IniOptions.Default))
            {
                Assert.Throws<ArgumentException>(() => doc.GetSection("nonexistent"));
            }
        }

        [Test]
        public void TestDontParseDefaultSection()
        {
            using (var reader = new StringReader($"[{Section.DefaultSectionName}]"))
            using (var parser = new Parser.Parser(reader, IniOptions.Default))
            using (var doc = new ParsedSectionCollection(parser, IniOptions.Default))
            {
                Assert.Throws<InvalidTokenException>(() => doc.GetDefaultSection());
            }
        }

        [Test]
        public void TestInvalidUnit()
        {
            using (var reader = new StringReader("invalid"))
            using (var parser = new Parser.Parser(reader, IniOptions.Default))
            using (var doc = new ParsedSectionCollection(parser, IniOptions.Default))
            {
                Assert.Throws<InvalidUnitException>(() => doc.GetDefaultSection());
            }
        }

        [Test]
        public void TestGetSectionsWithProperties()
        {
            using (var reader = new StringReader("[section1]\nvalue=10\n[section2]\nvalue=10\n[section3]\nvalue=10"))
            using (var parser = new Parser.Parser(reader, IniOptions.Default))
            using (var doc = new ParsedSectionCollection(parser, IniOptions.Default))
            {
                for (var i = 0; i < 3; i++)
                {
                    var name = $"section{i + 1}";
                    var section = doc.GetSection(name);
                    Assert.AreEqual(name, section.Name);
                    Assert.AreEqual(1, section.GetProperties().Count());
                    Assert.AreEqual(true, section.HasProperty("value"));
                    Assert.AreEqual(10L, section.GetProperty("value").GetValueAs<long>());
                }
            }
        }

        [Test]
        public void GetSectionCaseInsensitive()
        {
            var options = new IniOptions(caseSensitive: false);

            using (var reader = new StringReader("[section]"))
            using (var parser = new Parser.Parser(reader, options))
            using (var doc = new ParsedSectionCollection(parser, options))
            {
                Assert.NotNull(doc.GetSection("SECTION"));
            }
        }

        [Test]
        public void GetSectionCaseInsensitiveParsing()
        {
            var options = new IniOptions(caseSensitive: false);

            using (var reader = new StringReader("[SECTION]"))
            using (var parser = new Parser.Parser(reader, options))
            using (var doc = new ParsedSectionCollection(parser, options))
            {
                Assert.NotNull(doc.GetSection("section"));
            }
        }

        [Test]
        public void GetSectionCaseSensitive()
        {
            var options = new IniOptions(caseSensitive: true);

            using (var reader = new StringReader("[section]"))
            using (var parser = new Parser.Parser(reader, options))
            using (var doc = new ParsedSectionCollection(parser, options))
            {
                Assert.Throws<ArgumentException>(() => doc.GetSection("SECTION"));
            }
        }

        [Test]
        public void GetSectionCaseSensitiveParsing()
        {
            var options = new IniOptions(caseSensitive: true);

            using (var reader = new StringReader("[SECTION]"))
            using (var parser = new Parser.Parser(reader, options))
            using (var doc = new ParsedSectionCollection(parser, options))
            {
                Assert.Throws<ArgumentException>(() => doc.GetSection("section"));
            }
        }
    }
}
