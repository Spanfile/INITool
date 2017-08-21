using System.IO;
using Xunit;
// ReSharper disable InconsistentNaming

namespace INITool.Tests
{
    public class WriterTests
    {
        [Fact]
        public void TestBasicWrite()
        {
            using (var mem = new MemoryStream())
            using (var streamWriter = new StreamWriter(mem))
            using (var writer = new IniWriter(streamWriter, IniOptions.Default))
            {
                writer.AddProperty("value", 10L);
                writer.Write();
                writer.Flush();

                mem.Position = 0;
                var memReader = new StreamReader(mem);
                var read = memReader.ReadToEnd();
                Assert.Equal("value=10\n", read);
            }
        }

        [Fact]
        public void TestSectionWrite()
        {
            using (var mem = new MemoryStream())
            using (var streamWriter = new StreamWriter(mem))
            using (var writer = new IniWriter(streamWriter, IniOptions.Default))
            {
                writer.StartSection("section");
                writer.AddProperty("value", 10L);
                writer.Write();
                writer.Flush();

                mem.Position = 0;
                var memReader = new StreamReader(mem);
                var read = memReader.ReadToEnd();
                Assert.Equal("[section]\nvalue=10\n", read);
            }
        }

        [Fact]
        public void TestPropertyComment()
        {
            using (var mem = new MemoryStream())
            using (var streamWriter = new StreamWriter(mem))
            using (var writer = new IniWriter(streamWriter, IniOptions.Default))
            {
                writer.AddProperty("value", 10L, "property");
                writer.Write();
                writer.Flush();

                mem.Position = 0;
                var memReader = new StreamReader(mem);
                var read = memReader.ReadToEnd();
                Assert.Equal("#property\nvalue=10\n", read);
            }
        }

        [Fact]
        public void TestSectionComment()
        {
            using (var mem = new MemoryStream())
            using (var streamWriter = new StreamWriter(mem))
            using (var writer = new IniWriter(streamWriter, IniOptions.Default))
            {
                writer.StartSection("section", "section");
                writer.Write();
                writer.Flush();

                mem.Position = 0;
                var memReader = new StreamReader(mem);
                var read = memReader.ReadToEnd();
                Assert.Equal("#section\n[section]\n", read);
            }
        }

        [Fact]
        public void TestLineEndingLF()
        {
            using (var mem = new MemoryStream())
            using (var streamWriter = new StreamWriter(mem))
            using (var writer = new IniWriter(streamWriter, new IniOptions(lineEndings: LineEnding.LF)))
            {
                writer.StartSection("section");
                writer.Write();
                writer.Flush();

                mem.Position = 0;
                var memReader = new StreamReader(mem);
                var read = memReader.ReadToEnd();
                Assert.Equal("[section]\n", read);
            }
        }

        [Fact]
        public void TestLineEndingCR()
        {
            using (var mem = new MemoryStream())
            using (var streamWriter = new StreamWriter(mem))
            using (var writer = new IniWriter(streamWriter, new IniOptions(lineEndings: LineEnding.CR)))
            {
                writer.StartSection("section");
                writer.Write();
                writer.Flush();

                mem.Position = 0;
                var memReader = new StreamReader(mem);
                var read = memReader.ReadToEnd();
                Assert.Equal("[section]\r", read);
            }
        }

        [Fact]
        public void TestLineEndingCRLF()
        {
            using (var mem = new MemoryStream())
            using (var streamWriter = new StreamWriter(mem))
            using (var writer = new IniWriter(streamWriter, new IniOptions(lineEndings: LineEnding.CRLF)))
            {
                writer.StartSection("section");
                writer.Write();
                writer.Flush();

                mem.Position = 0;
                var memReader = new StreamReader(mem);
                var read = memReader.ReadToEnd();
                Assert.Equal("[section]\r\n", read);
            }
        }
    }
}
