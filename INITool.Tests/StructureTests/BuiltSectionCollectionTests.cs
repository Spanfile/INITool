using System;
using INITool.Structure.Sections;
using Xunit;

namespace INITool.Tests.StructureTests
{
    public class BuiltSectionCollectionTests
    {
        [Fact]
        public void TestMultipleSectionsWithProperties()
        {
            var builder = new BuiltSectionCollection(IniOptions.Default);

            builder.StartSection("section1");
            builder.AddProperty("value", 10L);
            builder.StartSection("section2");
            builder.AddProperty("value", 10L);

            var built = builder.SerialiseToString();
            Assert.Equal("[section1]\nvalue=10\n[section2]\nvalue=10\n", built);
        }

        [Fact]
        public void TestOmittedDefaultSection()
        {
            var builder = new BuiltSectionCollection(IniOptions.Default);

            builder.AddProperty("value", 10L);
            builder.StartSection("section2");
            builder.AddProperty("value", 10L);

            var built = builder.SerialiseToString();
            Assert.Equal("value=10\n[section2]\nvalue=10\n", built);
        }

        [Fact]
        public void TestInvalidPropertyType()
        {
            var builder = new BuiltSectionCollection(IniOptions.Default);

            Assert.Throws<ArgumentException>(() => builder.AddProperty("invalid", new DateTime(1970, 1, 1)));
        }

        [Fact]
        public void TestDefaultSectionBlock()
        {
            var builder = new BuiltSectionCollection(IniOptions.Default);

            Assert.Throws<ArgumentException>(() => builder.StartSection(Section.DefaultSectionName));
        }

        [Fact]
        public void TestDefaultSectionBlockCaseInvariant()
        {
            var builder = new BuiltSectionCollection(IniOptions.Default);

            Assert.Throws<ArgumentException>(() => builder.StartSection(Section.DefaultSectionName.ToUpperInvariant()));
        }
    }
}
