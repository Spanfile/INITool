using System.IO;
using INITool.Parser.Units;
using Xunit;

namespace INITool.Tests.ParserTests
{
    public class BooleanParseTests
    {
        [Fact]
        public void TestParseTrue()
        {
            using (var reader = new StringReader("true"))
            using (var parser = new Parser.Parser(reader))
            {
                var unit = parser.ParseUnitOfType<ValueUnit>();
                Assert.Equal("true", unit.SourceTokenString());
                Assert.Equal(true, unit.Value);
            }
        }

        [Fact]
        public void TestParseFalse()
        {
            using (var reader = new StringReader("false"))
            using (var parser = new Parser.Parser(reader))
            {
                var unit = parser.ParseUnitOfType<ValueUnit>();
                Assert.Equal("false", unit.SourceTokenString());
                Assert.Equal(false, unit.Value);
            }
        }
    }
}
