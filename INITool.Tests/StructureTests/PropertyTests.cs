using System.IO;
using INITool.Parser.Units;
using INITool.Structure.Properties;
using NUnit.Framework;
// ReSharper disable RedundantArgumentDefaultValue
// ReSharper disable ArgumentsStyleNamedExpression

namespace INITool.Tests.StructureTests
{
    [TestFixture]
    public class PropertyTests
    {
        [Test]
        public void TestComment()
        {
            using (var reader = new StringReader("#comment\nvalue=10"))
            using (var parser = new Parser.Parser(reader, IniOptions.Default))
            {
                var prop = Property.FromParsedAssignmentUnit(parser.ParseUnitOfType<AssignmentUnit>(), IniOptions.Default);

                Assert.AreEqual("comment", prop.Comment);
            }
        }

        [Test]
        public void TestFromIntegerAssignment()
        {
            using (var reader = new StringReader("value=10"))
            using (var parser = new Parser.Parser(reader, IniOptions.Default))
            {
                var prop = Property.FromParsedAssignmentUnit(parser.ParseUnitOfType<AssignmentUnit>(), IniOptions.Default);

                Assert.AreEqual("value", prop.Name);
                Assert.AreEqual(10L, prop.GetValueAs<long>());
            }
        }

        [Test]
        public void TestFromFloatingPointAssignment()
        {
            using (var reader = new StringReader("value=1.1"))
            using (var parser = new Parser.Parser(reader, IniOptions.Default))
            {
                var prop = Property.FromParsedAssignmentUnit(parser.ParseUnitOfType<AssignmentUnit>(), IniOptions.Default);

                Assert.AreEqual("value", prop.Name);
                Assert.AreEqual(1.1d, prop.GetValueAs<double>());
            }
        }

        [Test]
        public void TestFromBooleanAssignment()
        {
            using (var reader = new StringReader("value=true"))
            using (var parser = new Parser.Parser(reader, IniOptions.Default))
            {
                var prop = Property.FromParsedAssignmentUnit(parser.ParseUnitOfType<AssignmentUnit>(), IniOptions.Default);

                Assert.AreEqual("value", prop.Name);
                Assert.AreEqual(true, prop.GetValueAs<bool>());
            }
        }

        [Test]
        public void TestFromStringAssignment()
        {
            using (var reader = new StringReader("value=\"Hello, World!\""))
            using (var parser = new Parser.Parser(reader, IniOptions.Default))
            {
                var prop = Property.FromParsedAssignmentUnit(parser.ParseUnitOfType<AssignmentUnit>(), IniOptions.Default);

                Assert.AreEqual("value", prop.Name);
                Assert.AreEqual("Hello, World!", prop.GetValueAs<string>());
            }
        }

        [Test]
        public void TestSerialiseInteger()
        {
            var prop = new Property("value", 10L, IniOptions.Default);
            Assert.AreEqual("value=10", prop.SerialiseToString());
        }

        [Test]
        public void TestSerialiseFloatingPoint()
        {
            var prop = new Property("value", 1.1d, IniOptions.Default);
            Assert.AreEqual("value=1.1", prop.SerialiseToString());
        }

        [Test]
        public void TestSerialiseBoolean()
        {
            var prop = new Property("value", true, IniOptions.Default);
            Assert.AreEqual("value=true", prop.SerialiseToString());
        }

        [Test]
        public void TestSerialiseNormalString()
        {
            var prop = new Property("value", "Hello, World!", IniOptions.Default);
            Assert.AreEqual("value=\"Hello, World!\"", prop.SerialiseToString());
        }

        [Test]
        public void TestSerialiseStringWithNewlineToVerbatim()
        {
            var prop = new Property("value", "Hello,\nWorld!", new IniOptions(stringSerialisationPolicy: StringSerialisationPolicy.ToVerbatim));
            Assert.AreEqual("value=@\"Hello,\nWorld!\"", prop.SerialiseToString());
        }

        [Test]
        public void TestSerialiseStringWithNewlineToEscapeSequences()
        {
            var prop = new Property("value", "Hello,\nWorld!", new IniOptions(stringSerialisationPolicy: StringSerialisationPolicy.ToEscapeSequences));
            Assert.AreEqual("value=\"Hello,\\nWorld!\"", prop.SerialiseToString());
        }
    }
}
