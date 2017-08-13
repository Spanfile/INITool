using System;
using System.Reflection;
using System.Runtime.Serialization;
using INITool.Parser.Units;

namespace INITool.Parser
{
    [Serializable]
    internal class UnexpectedUnitException : ParserException
    {
        public UnexpectedUnitException(Unit got, MemberInfo expected) : base($"unexpected unit: expected {expected.Name}, got {got.GetType().Name}")
        {
        }

        public UnexpectedUnitException(Unit got, MemberInfo expected, Exception inner) : base($"unexpected unit: expected {expected.Name}, got {got.GetType().Name}", inner)
        {
        }

        protected UnexpectedUnitException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}