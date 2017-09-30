using System.IO;
using INITool.Parser;
using INITool.Parser.Units;
using NUnit.Framework;
// ReSharper disable ArgumentsStyleNamedExpression

namespace INITool.Tests.ParserTests
{
    [TestFixture]
    public class AssignmentParseTests
    {
        [Test]
        public void TestParseIntegerAssignment()
        {
            using (var reader = new StringReader("value=10"))
            using (var parser = new Parser.Parser(reader, IniOptions.Default))
            {
                var unit = parser.ParseUnitOfType<AssignmentUnit>();
                Assert.AreEqual("value=10", unit.SourceTokenString());

                Assert.AreEqual("value", unit.Name);
                Assert.AreEqual(10L, unit.Value);
            }
        }

        [Test]
        public void TestParseFloatingPointAssignment()
        {
            using (var reader = new StringReader("value=1.1"))
            using (var parser = new Parser.Parser(reader, IniOptions.Default))
            {
                var unit = parser.ParseUnitOfType<AssignmentUnit>();
                Assert.AreEqual("value=1.1", unit.SourceTokenString());

                Assert.AreEqual("value", unit.Name);
                Assert.AreEqual(1.1d, unit.Value);
            }
        }

        [Test]
        public void TestParseBooleanAssignment()
        {
            using (var reader = new StringReader("value=true"))
            using (var parser = new Parser.Parser(reader, IniOptions.Default))
            {
                var unit = parser.ParseUnitOfType<AssignmentUnit>();
                Assert.AreEqual("value=true", unit.SourceTokenString());

                Assert.AreEqual("value", unit.Name);
                Assert.AreEqual(true, unit.Value);
            }
        }

        [Test]
        public void TestParseStringAssignment()
        {
            using (var reader = new StringReader("value='Hello, World!'"))
            using (var parser = new Parser.Parser(reader, IniOptions.Default))
            {
                var unit = parser.ParseUnitOfType<AssignmentUnit>();
                Assert.AreEqual("value='Hello, World!'", unit.SourceTokenString());

                Assert.AreEqual("value", unit.Name);
                Assert.AreEqual("Hello, World!", unit.Value);
            }
        }

        [Test]
        public void TestNewlineInAssignmentStrongParsing()
        {
            using (var reader = new StringReader("value=\n10"))
            using (var parser = new Parser.Parser(reader, IniOptions.Default))
            {
                Assert.Throws<InvalidTokenException>(() => parser.ParseUnit());
            }
        }

        [Test]
        public void TestNewlineInAssignmentFlexibleParsing()
        {
            var options = new IniOptions(propertyParsing: PropertyParsing.Flexible);

            using (var reader = new StringReader("value=\n10"))
            using (var parser = new Parser.Parser(reader, options))
            {
                Assert.Throws<InvalidTokenException>(() => parser.ParseUnit());
            }
        }

        [Test]
        public void TestIncompleteAssignment()
        {
            using (var reader = new StringReader("value="))
            using (var parser = new Parser.Parser(reader, IniOptions.Default))
            {
                Assert.Throws<EndOfDocumentException>(() => parser.ParseUnit());
            }
        }

        [Test]
        public void TestInvalidLeftAssignment()
        {
            using (var reader = new StringReader("10=10"))
            using (var parser = new Parser.Parser(reader, IniOptions.Default))
            {
                Assert.Throws<InvalidUnitException>(() => parser.ParseUnit());
            }
        }


        [Test]
        public void TestInvalidAssignment()
        {
            using (var reader = new StringReader("value=value"))
            using (var parser = new Parser.Parser(reader, IniOptions.Default))
            {
                Assert.Throws<InvalidUnitException>(() => parser.ParseUnitOfType<AssignmentUnit>());
            }
        }
        
        [Test]
        public void TestInvariantUnitConversionAssignment()
        {
            var options = new IniOptions(propertyParsing: PropertyParsing.Flexible);

            using (var reader = new StringReader("value=value"))
            using (var parser = new Parser.Parser(reader, options))
            {
                var unit = parser.ParseUnitOfType<AssignmentUnit>();
                Assert.AreEqual("value=value", unit.SourceTokenString());

                Assert.AreEqual("value", unit.Name);
                Assert.AreEqual("value", unit.Value);
            }
        }
        
        [Test]
        public void TestInvariantAssignment()
        {
            var options = new IniOptions(propertyParsing: PropertyParsing.Flexible);

            using (var reader = new StringReader("value=/value"))
            using (var parser = new Parser.Parser(reader, options))
            {
                var unit = parser.ParseUnitOfType<AssignmentUnit>();
                Assert.AreEqual("value=/value", unit.SourceTokenString());

                Assert.AreEqual("value", unit.Name);
                Assert.AreEqual("/value", unit.Value);
            }
        }

        [Test]
        public void TestInvariantWithWhitespaceAssignment()
        {
            var options = new IniOptions(propertyParsing: PropertyParsing.Flexible);

            using (var reader = new StringReader("value=value with whitespace"))
            using (var parser = new Parser.Parser(reader, options))
            {
                var unit = parser.ParseUnitOfType<AssignmentUnit>();
                Assert.AreEqual("value=value with whitespace", unit.SourceTokenString());

                Assert.AreEqual("value", unit.Name);
                Assert.AreEqual("value with whitespace", unit.Value);
            }
        }

        [Test]
        public void TestLooseAssignment()
        {
            var options = new IniOptions(allowLooseProperties: true);

            using (var reader = new StringReader("value"))
            using (var parser = new Parser.Parser(reader, options))
            {
                var unit = parser.ParseUnitOfType<AssignmentUnit>();
                Assert.AreEqual("value", unit.SourceTokenString());

                Assert.AreEqual("value", unit.Name);
                Assert.AreEqual(true, unit.Value);
            }
        }
    }
}
