using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenerator {
    public class FieldGenerator : IContentGenerator {
        public List<ModifierKeyword> Modifiers { get; } = new();
        public Type Type { get; set; } = typeof(string);
        public string Name { get; set; }
        public string DefaultValue { get; set; }

        public string GenerateContent() {
            StringBuilder stringBuilder = new();

            stringBuilder.Append($"{GenerationHelper.GetModifiers(Modifiers)} {GenerationHelper.GetTypeName(Type)} {Name}");

            if (string.IsNullOrEmpty(DefaultValue) is false)
                stringBuilder.Append(" = " + DefaultValue);

            stringBuilder.AppendLine(";");

            return stringBuilder.ToString();
        }
    }
}