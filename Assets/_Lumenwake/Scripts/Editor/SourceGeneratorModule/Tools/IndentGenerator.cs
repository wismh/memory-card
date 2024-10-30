using System.Linq;

namespace Project.Core.SourceGeneratorModule.Editor
{
    public class IndentGenerator
    {
        public static string GetIndent(int indentCount) =>
            string.Concat(Enumerable.Repeat(ToolConstants.DefaultTabString, indentCount));
    }
}