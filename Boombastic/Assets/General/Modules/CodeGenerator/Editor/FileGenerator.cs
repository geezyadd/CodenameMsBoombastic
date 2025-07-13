using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenerator {
    public class FileGenerator : IContentHierarchyGenerator {
        public List<IContentGenerator> Children { get; } = new();

        public string GenerateContent() {
            StringBuilder stringBuilder = new();
            stringBuilder.Append($"// Generated File\n");
            stringBuilder.Append($"// Date: {DateTime.Now:yyyy-MM-dd HH:mm:ss}\n");
            stringBuilder.Append($"// Code Generator version: 1.0.0\n\n");

            foreach (IContentGenerator child in Children)
                stringBuilder.Append(child.GenerateContent());

            return stringBuilder.ToString();
        }
    }
}