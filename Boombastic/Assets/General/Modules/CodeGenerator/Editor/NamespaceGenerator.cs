using System.Collections.Generic;
using System.Text;

namespace CodeGenerator {
    public class NamespaceGenerator : IContentHierarchyGenerator {
        public List<IContentGenerator> Children { get; } = new();
        public string Namespace { get; set; }

        public NamespaceGenerator(string @namespace) =>
            Namespace = @namespace;

        public string GenerateContent() {
            StringBuilder stringBuilder = new();
            
            stringBuilder.AppendLine("namespace " + Namespace + " {");
            foreach (IContentGenerator child in Children)
                stringBuilder.AppendLine(GenerationHelper.AddIndent(child.GenerateContent()));
            stringBuilder.Append("}");
            
            return stringBuilder.ToString();
        }
    }
}