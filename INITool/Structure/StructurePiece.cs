using System.Text;

namespace INITool.Structure
{
    internal abstract class StructurePiece
    {
        public string Comment { get; set; }
        protected IniOptions Options { get; }

        protected StructurePiece(IniOptions options)
        {
            Options = options;
        }

        protected void AppendCommentToStringBuilder(StringBuilder sb)
        {
            if (Comment != null)
                sb.Append("#").Append(Comment).Append("\n");
        }

        protected string CaseSensitiviseString(string str) => Options.CaseSensitive ? str : str.ToLowerInvariant();
    }
}
