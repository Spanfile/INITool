using System.IO;
using INITool.Parser.Units;
using NUnit.Framework;
// ReSharper disable ArgumentsStyleNamedExpression

namespace INITool.Tests.ParserTests
{
    [TestFixture]
    public class BooleanParseTests
    {
        [Test]
        public void TestParseTrue()
        {
            using (var reader = new StringReader("true"))
            using (var parser = new Parser.Parser(reader, IniOptions.Default))
            {
                var unit = parser.ParseUnitOfType<ValueUnit>();
                Assert.AreEqual("true", unit.SourceTokenString());
                Assert.AreEqual(true, unit.Value);
            }
        }

        [Test]
        public void TestParseFalse()
        {
            using (var reader = new StringReader("false"))
            using (var parser = new Parser.Parser(reader, IniOptions.Default))
            {
                var unit = parser.ParseUnitOfType<ValueUnit>();
                Assert.AreEqual("false", unit.SourceTokenString());
                Assert.AreEqual(false, unit.Value);
            }
        }
    }
}
