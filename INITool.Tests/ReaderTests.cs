using Xunit;

namespace INITool.Tests
{
    public class ReaderTests
    {
        [Fact]
        public void TestReadInt64()
        {
            using (var reader = new IniReader("basic.ini"))
            {
                Assert.Equal(10L, reader.GetInt64("Section.int"));
            }
        }

        [Fact]
        public void TestReadString()
        {
            using (var reader = new IniReader("basic.ini"))
            {
                Assert.Equal("Hello, World!", reader.GetString("Section.string"));
            }
        }

        [Fact]
        public void TestReadDouble()
        {
            using (var reader = new IniReader("basic.ini"))
            {
                Assert.Equal(1.1d, reader.GetDouble("Section.float"));
            }
        }

        [Fact]
        public void TestReadBool()
        {
            using (var reader = new IniReader("basic.ini"))
            {
                Assert.Equal(true, reader.GetBool("Section.bool"));
            }
        }
    }
}
