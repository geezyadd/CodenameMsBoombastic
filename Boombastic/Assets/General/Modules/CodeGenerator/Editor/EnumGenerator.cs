using System.Collections.Generic;
using System.Text;

namespace CodeGenerator {
    public class EnumGenerator : IContentGenerator {
        public string Name { get; set; }
        public List<ModifierKeyword> Modifiers { get; } = new();
        public List<string> Values { get; } = new();
        public int StartIndex { get; set; } = 0;
        public int Increment { get; set; } = 1;
        public List<int> Indexes { get; } = new();

        public string GenerateContent() {
            StringBuilder stringBuilder = new();
            
            stringBuilder.AppendLine($"{GenerationHelper.GetModifiers(Modifiers)} enum {Name} {{");
            
            for (int elementIndex = 0; elementIndex < Values.Count; elementIndex++)
                stringBuilder.AppendLine(GenerationHelper.AddIndent(Values[elementIndex] + " = " + GetIndex(elementIndex) + ","));

            stringBuilder.Append("}");
            
            return stringBuilder.ToString();
        }

        private int GetIndex(int elementIndex) {
            if (Indexes == null || Indexes != null && elementIndex >= Indexes.Count)
                return elementIndex + StartIndex + elementIndex * Increment;

            return Indexes[elementIndex];
        }
    }
}