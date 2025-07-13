using CodeGenerator;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace DataGenerators.Editor {
    public static class AddressableAssetsGenerator {
        [MenuItem("Generation/Data/Addressable Assets")]
        public static void Generate() {
            ClassGenerator classGenerator = new() {
                Name = "AddressableAssets",
                Modifiers = {
                    ModifierKeyword.Public,
                    ModifierKeyword.Static
                }
            };
            
            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
            
            ClassGenerator groupsClass = new() {
                Name = "Groups",
                Modifiers = {
                    ModifierKeyword.Public,
                    ModifierKeyword.Static
                }
            };
            
            foreach (AddressableAssetGroup group in settings.groups) {
                groupsClass.Children.Add(new FieldGenerator {
                    Name = group.Name,
                    Modifiers = {
                        ModifierKeyword.Public,
                        ModifierKeyword.Static
                    },
                    Type = typeof(string),
                    DefaultValue = $"\"{group.Name}\""
                });
            }
            
            classGenerator.Children.Add(groupsClass);
            
            foreach (AddressableAssetGroup group in settings.groups) {
                if (string.IsNullOrEmpty(group.Name))
                    continue;

                ClassGenerator groupClass = new() {
                    Name = group.Name,
                    Modifiers = {
                        ModifierKeyword.Public,
                        ModifierKeyword.Static
                    }
                };
            
                foreach (AddressableAssetEntry entry in group.entries) {
                    if (string.IsNullOrEmpty(entry.address)) {
                        Debug.LogWarning("Detected empty address in Group: " + group.name + ". Asset will be skipped.");
                        continue;
                    }

                    groupClass.Children.Add(new FieldGenerator {
                        Name = entry.address,
                        Modifiers = {
                            ModifierKeyword.Public,
                            ModifierKeyword.Static
                        },
                        Type = typeof(string),
                        DefaultValue = $"\"{entry.address}\""
                    });
                }

                classGenerator.Children.Add(groupClass);
            }
            
            FileGenerator fileGenerator = new() {
                Children = {
                    new NamespaceGenerator("Generations.Data") {
                        Children = {
                            classGenerator
                        }
                    }
                }
            };

            FileWriter.WriteContent("Assets/General/Generations/Data/AddressableAssets.cs", fileGenerator.GenerateContent());
        }
    }
}
