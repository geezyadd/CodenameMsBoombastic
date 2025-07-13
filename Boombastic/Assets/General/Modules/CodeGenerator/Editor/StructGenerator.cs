using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGenerator {
    public class StructGenerator : IContentHierarchyGenerator {
        public List<IContentGenerator> Children { get; } = new();
        public string Name { get; set; }
        public List<ModifierKeyword> Modifiers { get; } = new();
        public List<Type> Extends { get; } = new();

        public string GenerateContent() {
            StringBuilder stringBuilder = new();
            
            stringBuilder.Append($"{GenerationHelper.GetModifiers(Modifiers)} struct {Name}");

            if (Extends.Count > 0) {
                string extends = string.Join(", ", Extends.Select(GenerationHelper.GetTypeName));
                stringBuilder.Append($" : {extends}");
            }
            
            stringBuilder.Append(" { ");

            if (Children.Count > 0)
                stringBuilder.AppendLine();
            
            foreach (IContentGenerator child in Children)
                stringBuilder.AppendLine(GenerationHelper.AddIndent(child.GenerateContent()));

            stringBuilder.Append("}");
            
            return stringBuilder.ToString();
        }
    }
}