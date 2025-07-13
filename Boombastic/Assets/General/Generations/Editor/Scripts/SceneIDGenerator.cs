using System.Collections.Generic;
using System.IO;
using CodeGenerator;
using UnityEditor;

namespace DataGenerators.Editor {
    public static class SceneIDGenerator {
        [MenuItem("Generation/Data/Scenes In Build")]
        public static void Generate() {
            ClassGenerator idsClassGenerator = new() {
                Name = "SceneID",
                Modifiers = {
                    ModifierKeyword.Public,
                    ModifierKeyword.Static
                }
            };
            
            ClassGenerator namesClassGenerator = new() {
                Name = "SceneName",
                Modifiers = {
                    ModifierKeyword.Public,
                    ModifierKeyword.Static
                }
            };

            EnumGenerator scenesEnumGenerator = new() {
                Name = "SceneInBuild",
                Modifiers = {
                    ModifierKeyword.Public
                }
            };

            List<string> sceneNames = new();
            for (int sceneIndex = 0; sceneIndex < EditorBuildSettings.scenes.Length; sceneIndex++) {
                EditorBuildSettingsScene scene = EditorBuildSettings.scenes[sceneIndex];
                if (scene.enabled is false)
                    continue;

                string sceneName = Path.GetFileNameWithoutExtension(scene.path);
                sceneNames.Add(sceneName);
                
                idsClassGenerator.Children.Add(new FieldGenerator {
                    Name = sceneName,
                    Modifiers = {
                        ModifierKeyword.Public,
                        ModifierKeyword.Static
                    },
                    Type = typeof(int),
                    DefaultValue = sceneIndex.ToString()
                });
                
                namesClassGenerator.Children.Add(new FieldGenerator {
                    Name = sceneName,
                    Modifiers = {
                        ModifierKeyword.Public,
                        ModifierKeyword.Static
                    },
                    Type = typeof(string),
                    DefaultValue = $"\"{sceneName}\""
                });
            }
            scenesEnumGenerator.Values.AddRange(sceneNames);

            FileGenerator fileGenerator = new() {
                Children = {
                    new NamespaceGenerator("Generations.Data") {
                        Children = {
                            scenesEnumGenerator,
                            idsClassGenerator,
                            namesClassGenerator
                        }
                    }
                }
            };
            
            FileWriter.WriteContent("Assets/General/Generations/Runtime/Data/ScenesInBuild.cs", fileGenerator.GenerateContent());
        }
    }
}