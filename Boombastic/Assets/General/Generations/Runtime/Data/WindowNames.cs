// Generated File
// Date: 2025-07-20 15:34:07
// Code Generator version: 1.0.0

namespace Generations.Data {
    public enum WindowNames {
        Test2Window = 0,
        Test3Window = 1,
        TestWindow = 2,
    }
    public static class WindowsTypeMapper { 
        private static System.Collections.Generic.IReadOnlyDictionary<string, string> WindowTypes => new System.Collections.Generic.Dictionary<string, string> {
            {"Test2Window", "UIWorkflow.Windows.Test2Window, UIWorkflow.Editor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" },
            {"Test3Window", "UIWorkflow.Windows.Test3Window, UIWorkflow.Editor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" },
            {"TestWindow", "UIWorkflow.Windows.TestWindow, UIWorkflow.Editor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" },
        };
        public static System.Type GetType(string typeName) {
            if (WindowTypes.TryGetValue(typeName, out string value))
                return System.Type.GetType(value);
            return default;
        }
    }
}