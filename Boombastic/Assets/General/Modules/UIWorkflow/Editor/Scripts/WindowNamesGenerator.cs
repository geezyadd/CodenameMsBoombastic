using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CodeGenerator;
using UIWorkflow.Core;
using UnityEditor;

namespace UIWorkflow.Editor {
    public static class WindowNamesGenerator {
        [MenuItem("Generation/Data/Window Names")]
        public static void Generate() {
            EnumGenerator windowsEnumGenerator = new() {
                Name = "WindowNames",
                Modifiers = {
                    ModifierKeyword.Public
                }
            };

            List<Type> windowTypes = GetWindowTypes();
            windowsEnumGenerator.Values.AddRange(windowTypes.Select(type => type.Name));

            string dictionaryContent = "private static System.Collections.Generic.IReadOnlyDictionary<string, string> WindowTypes => new System.Collections.Generic.Dictionary<string, string> {";
            foreach (Type windowType in windowTypes)
                dictionaryContent += "\n    {\"" + windowType.Name + "\", \"" + windowType.FullName + ", " + windowType.Assembly + "\" },";
            dictionaryContent += "\n};";

            string getTypeMethod = "public static System.Type GetType(string typeName) {\n    if (WindowTypes.TryGetValue(typeName, out string value))\n        return System.Type.GetType(value);\n    return default;\n}";
            
            ClassGenerator typeMapper = new() {
                Name = "WindowsTypeMapper",
                Modifiers = {
                    ModifierKeyword.Public,
                    ModifierKeyword.Static,
                },
                Children = {
                    new TextBlockGenerator(dictionaryContent),
                    new TextBlockGenerator(getTypeMethod),
                }
            };
            
            FileGenerator fileGenerator = new() {
                Children = {
                    new NamespaceGenerator("Generations.Data") {
                        Children = {
                            windowsEnumGenerator,
                            typeMapper
                        }
                    }
                }
            };
            
            FileWriter.WriteContent("Assets/General/Generations/Runtime/Data/WindowNames.cs", fileGenerator.GenerateContent());
        }
        
        private static List<Type> GetWindowTypes() {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => {
                    Type[] types;
                    try {
                        types = assembly.GetTypes();
                    }
                    catch (ReflectionTypeLoadException e) {
                        types = e.Types.Where(t => t != null).ToArray();
                    }

                    return types;
                })
                .Where(type => type is { IsAbstract: false } && typeof(WindowBehaviour).IsAssignableFrom(type))
                .ToList();
        }
    }
}