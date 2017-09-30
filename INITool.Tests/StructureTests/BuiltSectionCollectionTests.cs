using System;
using INITool.Structure.Sections;
using NUnit.Framework;
// ReSharper disable ArgumentsStyleNamedExpression

namespace INITool.Tests.StructureTests
{
    [TestFixture]
    public class BuiltSectionCollectionTests
    {
        [Test]
        public void TestMultipleSectionsWithProperties()
        {
            var builder = new BuiltSectionCollection(IniOptions.Default);

            builder.StartSection("section1");
            builder.AddProperty("value", 10L);
            builder.StartSection("section2");
            builder.AddProperty("value", 10L);

            var built = builder.SerialiseToString();
            Assert.AreEqual("[section1]\nvalue=10\n[section2]\nvalue=10\n", built);
        }

        [Test]
        public void TestOmittedDefaultSection()
        {
            var builder = new BuiltSectionCollection(IniOptions.Default);

            builder.AddProperty("value", 10L);
            builder.StartSection("section2");
            builder.AddProperty("value", 10L);

            var built = builder.SerialiseToString();
            Assert.AreEqual("value=10\n[section2]\nvalue=10\n", built);
        }

        [Test]
        public void TestInvalidPropertyType()
        {
            var builder = new BuiltSectionCollection(IniOptions.Default);

            Assert.Throws<ArgumentException>(() => builder.AddProperty("invalid", new DateTime(1970, 1, 1)));
        }

        [Test]
        public void TestDefaultSectionBlock()
        {
            var builder = new BuiltSectionCollection(IniOptions.Default);

            Assert.Throws<ArgumentException>(() => builder.StartSection(Section.DefaultSectionName));
        }

        [Test]
        public void TestDefaultSectionBlockCaseInvariant()
        {
            var builder = new BuiltSectionCollection(IniOptions.Default);

            Assert.Throws<ArgumentException>(() => builder.StartSection(Section.DefaultSectionName.ToUpperInvariant()));
        }
    }
}
