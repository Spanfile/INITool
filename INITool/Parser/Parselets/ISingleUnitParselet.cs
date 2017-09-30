using INITool.Parser.Tokeniser;
using INITool.Parser.Units;

namespace INITool.Parser.Parselets
{
    internal interface ISingleUnitParselet
    {
        Unit TransformUnit(Unit left);
    }
}
