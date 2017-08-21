using System;
using System.Linq;
using System.Text;
using INITool.Parser.Units;

namespace INITool.Structure.Properties
{
    internal class Property : StructurePiece, ISerialisableToString
    {
        private static readonly Type[] AcceptedTypes = { typeof(string), typeof(long), typeof(double), typeof(bool) };

        public static Property FromParsedAssignmentUnit(AssignmentUnit assignmentUnit, IniOptions options) => new Property(
            assignmentUnit.Name, assignmentUnit.Value, options) {
            Comment = assignmentUnit.Comment
        };

        public string Name { get; }

        private readonly object value;
        
        public Property(string name, object value, IniOptions options) : base(options)
        {
            Name = CaseSensitiviseString(name);

            if (!AcceptedTypes.Contains(value.GetType()))
                throw new ArgumentException($"value is of invalid type ({value.GetType().Name})");

            this.value = value;
        }

        public string SerialiseToString()
        {
            var sb = new StringBuilder();

            AppendCommentToStringBuilder(sb);

            sb.Append($"{Name}=");

            switch (value)
            {
                case string s:
                    var includeAt = s.Contains("\r") || s.Contains("\n");
                    if (Options.StringSerialisationPolicy == StringSerialisationPolicy.ToEscapeSequences)
                    {
                        s = s
                            .Replace("\r", @"\r")
                            .Replace("\n", @"\n");
                        includeAt = false;
                    }

                    sb.Append($"{(includeAt ? "@" : "")}\"{s}\"");
                    break;

                case bool b:
                    sb.Append(b.ToString().ToLower());
                    break;

                default:
                    sb.Append(value);
                    break;
            }

            return sb.ToString();
        }

        public T GetValueAs<T>() => (T) value;
    }
}
