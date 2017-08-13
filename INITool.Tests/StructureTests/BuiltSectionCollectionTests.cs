using INITool.Structure.Sections;
using Xunit;

namespace INITool.Tests.StructureTests
{
    public class BuiltSectionCollectionTests
    {
        [Fact]
        public void TestMultipleSectionsWithProperties()
        {
            var builder = new BuiltSectionCollection();

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
            var builder = new BuiltSectionCollection();

            builder.AddProperty("value", 10L);
            builder.StartSection("section2");
            builder.AddProperty("value", 10L);

            var built = builder.SerialiseToString();
            Assert.Equal("value=10\n[section2]\nvalue=10\n", built);
        }
    }
}
