using System;
using System.IO;
using INITool.Parser.Units;
using INITool.Structure.Properties;
using INITool.Structure.Sections;
using NUnit.Framework;
// ReSharper disable ArgumentsStyleNamedExpression

namespace INITool.Tests.StructureTests
{
    [TestFixture]
    public class SectionTests
    {
        [Test]
        public void TestFromParsedSection()
        {
            using (var reader = new StringReader("#comment\n[section]"))
            using (var parser = new Parser.Parser(reader, IniOptions.Default))
            {
                var section = Section.FromParsedSectionUnit(parser.ParseUnitOfType<SectionUnit>(), IniOptions.Default);

                Assert.AreEqual("comment", section.Comment);
                Assert.AreEqual("section", section.Name);
            }
        }

        [Test]
        public void TestDuplicateProperty()
        {
            var section = Section.CreateDefault(IniOptions.Default);

            section.AddProperty(new Property("a", "a", IniOptions.Default));
            Assert.Throws<ArgumentException>(() => section.AddProperty(new Property("a", "a", IniOptions.Default)));
        }

        [Test]
        public void TestNonexistantPropertyThrowsException()
        {
            var section = Section.CreateDefault(IniOptions.Default);

            Assert.Throws<ArgumentException>(() => section.GetProperty("a"));
        }

        [Test]
        public void TestNonexistantPropertyReturnsNull()
        {
            var section = Section.CreateDefault(new IniOptions(throwAtNonexistentProperty: false));

            Assert.Null(section.GetProperty("a"));
        }
    }
}
