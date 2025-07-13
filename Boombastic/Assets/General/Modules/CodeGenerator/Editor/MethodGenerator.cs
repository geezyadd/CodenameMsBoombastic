using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGenerator {
    public class MethodGenerator : IContentHierarchyGenerator {
        public List<IContentGenerator> Children { get; } = new();
        public string Name { get; set; }
        public List<ModifierKeyword> Modifiers { get; } = new();
        public string ReturnType { get; set; } = "void";
        public List<ParameterData> Parameters { get; } = new();

        public string GenerateContent() {
            StringBuilder stringBuilder = new();

            string paramList = string.Join(", ", Parameters.Select(p => $"{GenerationHelper.GetTypeName(p.Type)} {p.Name}"));
            stringBuilder.AppendLine($"{GenerationHelper.GetModifiers(Modifiers)} {ReturnType} {Name}({paramList}) {{");

            foreach (IContentGenerator child in Children)
                stringBuilder.AppendLine(GenerationHelper.AddIndent(child.GenerateContent()));

            stringBuilder.Append("}");

            return stringBuilder.ToString();
        }
    }
}