using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeGenerator {
    internal static class GenerationHelper {
        public static string AddIndent(string content) =>
            "    " + content.Replace("\n", "\n    ").TrimEnd();

        public static string GetModifiers(List<ModifierKeyword> modifiers) =>
            string.Join(" ", modifiers.Where(modifier => modifier != ModifierKeyword.None).Select(modifier => modifier.ToString().ToLower()));

        public static string GetTypeName(Type type) {
            if (type == typeof(void)) return "void";
            if (type == typeof(bool)) return "bool";
            if (type == typeof(byte)) return "byte";
            if (type == typeof(sbyte)) return "sbyte";
            if (type == typeof(char)) return "char";
            if (type == typeof(decimal)) return "decimal";
            if (type == typeof(double)) return "double";
            if (type == typeof(float)) return "float";
            if (type == typeof(int)) return "int";
            if (type == typeof(uint)) return "uint";
            if (type == typeof(long)) return "long";
            if (type == typeof(ulong)) return "ulong";
            if (type == typeof(object)) return "object";
            if (type == typeof(short)) return "short";
            if (type == typeof(ushort)) return "ushort";
            if (type == typeof(string)) return "string";

            if (Nullable.GetUnderlyingType(type) is { } innerNullable)
                return GetTypeName(innerNullable) + "?";

            return type.FullName ?? type.Name;
        }
    }
}