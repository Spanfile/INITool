using System.IO;
using INITool.Parser.Units;
using INITool.Structure.Properties;
using Xunit;
// ReSharper disable RedundantArgumentDefaultValue

namespace INITool.Tests.StructureTests
{
    public class PropertyTests
    {
        [Fact]
        public void TestComment()
        {
            using (var reader = new StringReader("#comment\nvalue=10"))
            using (var parser = new Parser.Parser(reader))
            {
                var prop = Property.FromParsedAssignmentUnit(parser.ParseUnitOfType<AssignmentUnit>(), IniOptions.Default);

                Assert.Equal("comment", prop.Comment);
            }
        }

        [Fact]
        public void TestFromIntegerAssignment()
        {
            using (var reader = new StringReader("value=10"))
            using (var parser = new Parser.Parser(reader))
            {
                var prop = Property.FromParsedAssignmentUnit(parser.ParseUnitOfType<AssignmentUnit>(), IniOptions.Default);

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
                var prop = Property.FromParsedAssignmentUnit(parser.ParseUnitOfType<AssignmentUnit>(), IniOptions.Default);

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
                var prop = Property.FromParsedAssignmentUnit(parser.ParseUnitOfType<AssignmentUnit>(), IniOptions.Default);

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
                var prop = Property.FromParsedAssignmentUnit(parser.ParseUnitOfType<AssignmentUnit>(), IniOptions.Default);

                Assert.Equal("value", prop.Name);
                Assert.Equal("Hello, World!", prop.GetValueAs<string>());
            }
        }

        [Fact]
        public void TestSerialiseInteger()
        {
            var prop = new Property("value", 10L, IniOptions.Default);
            Assert.Equal("value=10", prop.SerialiseToString());
        }

        [Fact]
        public void TestSerialiseFloatingPoint()
        {
            var prop = new Property("value", 1.1d, IniOptions.Default);
            Assert.Equal("value=1.1", prop.SerialiseToString());
        }

        [Fact]
        public void TestSerialiseBoolean()
        {
            var prop = new Property("value", true, IniOptions.Default);
            Assert.Equal("value=true", prop.SerialiseToString());
        }

        [Fact]
        public void TestSerialiseNormalString()
        {
            var prop = new Property("value", "Hello, World!", IniOptions.Default);
            Assert.Equal("value=\"Hello, World!\"", prop.SerialiseToString());
        }

        [Fact]
        public void TestSerialiseStringWithNewlineToVerbatim()
        {
            var prop = new Property("value", "Hello,\nWorld!", new IniOptions(stringSerialisationPolicy: StringSerialisationPolicy.ToVerbatim));
            Assert.Equal("value=@\"Hello,\nWorld!\"", prop.SerialiseToString());
        }

        [Fact]
        public void TestSerialiseStringWithNewlineToEscapeSequences()
        {
            var prop = new Property("value", "Hello,\nWorld!", new IniOptions(stringSerialisationPolicy: StringSerialisationPolicy.ToEscapeSequences));
            Assert.Equal("value=\"Hello,\\nWorld!\"", prop.SerialiseToString());
        }
    }
}
