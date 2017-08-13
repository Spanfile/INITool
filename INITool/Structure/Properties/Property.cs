using INITool.Parser.Units;

namespace INITool.Structure.Properties
{
    internal class Property : ISerialisableToString
    {
        public static Property FromParsedAssignmentUnit(AssignmentUnit assignmentUnit) => new Property(assignmentUnit.Name, assignmentUnit.Value);

        public string Name { get; }

        private readonly object value;

        public Property(string name, object value)
        {
            Name = name;
            this.value = value;
        }

        public string SerialiseToString() => $"{Name}={value}";

        public T GetValueAs<T>() => (T) value;
    }
}
