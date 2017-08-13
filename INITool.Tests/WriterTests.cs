using System.IO;
using Xunit;

namespace INITool.Tests
{
    public class WriterTests
    {
        [Fact]
        public void TestWriteToStream()
        {
            using (var mem = new MemoryStream())
            using (var streamWriter = new StreamWriter(mem))
            {
                var writer = new IniWriter("");

                writer.StartSection("section");
                writer.AddProperty("value", 10L);

                writer.WriteToStream(streamWriter);
                streamWriter.Flush();

                mem.Position = 0;
                var memReader = new StreamReader(mem);
                var read = memReader.ReadToEnd();
                Assert.Equal("[section]\nvalue=10\n", read);
            }
        }
    }
}
