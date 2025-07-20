using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CodeGenerator;
using UIWorkflow.Core;
using UnityEditor;

namespace UIWorkflow.Editor {
    public static class WindowEnumGenerator {
        [MenuItem("Generation/Data/Window Names")]
        public static void Generate() {
            EnumGenerator windowsEnumGenerator = new() {
                Name = "WindowNames",
                Modifiers = {
                    ModifierKeyword.Public
                }
            };
            windowsEnumGenerator.Values.AddRange(GetWindowNames());

            FileGenerator fileGenerator = new() {
                Children = {
                    new NamespaceGenerator("Generations.Data") {
                        Children = {
                            windowsEnumGenerator
                        }
                    }
                }
            };
            
            FileWriter.WriteContent("Assets/General/Generations/Runtime/Data/WindowNames.cs", fileGenerator.GenerateContent());
        }
        
        private static List<string> GetWindowNames() {
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
                .Select(type => type.Name)
                .ToList();
        }
    }
}