// Generated File
// Date: 2025-07-20 16:10:01
// Code Generator version: 1.0.0

namespace Generations.Data {
    public enum WindowNames {
    }
    public static class WindowsTypeMapper { 
        private static System.Collections.Generic.IReadOnlyDictionary<string, string> WindowTypes => new System.Collections.Generic.Dictionary<string, string> {
        };
        public static System.Type GetType(string typeName) {
            if (WindowTypes.TryGetValue(typeName, out string value))
                return System.Type.GetType(value);
            return default;
        }
    }
}