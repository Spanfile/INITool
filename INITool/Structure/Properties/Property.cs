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

        public object GetValueAs<T>()
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (Type.GetTypeCode(typeof(T)))
            {
                case TypeCode.String:
                    if (Options.AllowValueConversion)
                        return value.ToString();
                    break;

                case TypeCode.Int64:
                    if (value is string s2 && Options.AllowValueConversion)
                        return long.Parse(s2);
                    break;

                case TypeCode.Double:
                    if (value is string s3 && Options.AllowValueConversion)
                        return double.Parse(s3);
                    break;

                case TypeCode.Boolean:
                    if (value is string s4 && Options.AllowValueConversion)
                        return bool.Parse(s4);
                    break;
            }

            return value;
        }
    }
}
