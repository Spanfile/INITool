using System;

namespace INITool.Parser.Tokeniser
{
    internal struct Position : IEquatable<Position>
    {
        public readonly int Line;
        public readonly int Column;

        public Position(int line, int column)
        {
            Line = line;
            Column = column;
        }

        public override string ToString() => $"ln {Line} col {Column}";

        public bool Equals(Position other) => Line == other.Line && Column == other.Column;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            return obj is Position && Equals((Position) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Line * 397) ^ Column;
            }
        }
    }
}
