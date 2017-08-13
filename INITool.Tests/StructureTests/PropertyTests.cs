using System.IO;
using INITool.Parser.Units;
using INITool.Structure.Properties;
using Xunit;

namespace INITool.Tests.StructureTests
{
    public class PropertyTests
    {
        [Fact]
        public void TestFromIntegerAssignment()
        {
            using (var reader = new StringReader("value=10"))
            using (var parser = new Parser.Parser(reader))
            {
                var prop = Property.FromParsedAssignmentUnit(parser.ParseUnitOfType<AssignmentUnit>());

                Assert.Equal("value", prop.Name);
                Assert.Equal(10L, prop.GetValueAs<long>());
            }
        }

        [Fact]
        public void TestFromFloatingPointAssignment()
        {
            using (var reader = new StringReader("value=1.1"))
            using (var parser = new Parser.Parser(reader))
            {
                var prop = Property.FromParsedAssignmentUnit(parser.ParseUnitOfType<AssignmentUnit>());

                Assert.Equal("value", prop.Name);
                Assert.Equal(1.1d, prop.GetValueAs<double>());
            }
        }

        [Fact]
        public void TestFromBooleanAssignment()
        {
            using (var reader = new StringReader("value=true"))
            using (var parser = new Parser.Parser(reader))
            {
                var prop = Property.FromParsedAssignmentUnit(parser.ParseUnitOfType<AssignmentUnit>());

                Assert.Equal("value", prop.Name);
                Assert.Equal(true, prop.GetValueAs<bool>());
            }
        }

        [Fact]
        public void TestFromStringAssignment()
        {
            using (var reader = new StringReader("value=\"Hello, World!\""))
            using (var parser = new Parser.Parser(reader))
            {
                var prop = Property.FromParsedAssignmentUnit(parser.ParseUnitOfType<AssignmentUnit>());

                Assert.Equal("value", prop.Name);
                Assert.Equal("Hello, World!", prop.GetValueAs<string>());
            }
        }
    }
}
