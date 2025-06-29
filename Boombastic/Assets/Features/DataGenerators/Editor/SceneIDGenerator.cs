using System.IO;
using CodeGenerator;
using UnityEditor;

namespace DataGenerators.Editor {
    public static class SceneIDGenerator {
        [MenuItem("Generation/Data/Scene IDs")]
        public static void Generate() {
            ClassGenerator classGenerator = new() {
                Name = "SceneID",
                Modifiers = {
                    ModifierKeyword.Public,
                    ModifierKeyword.Static
                }
            };
            
            for (int sceneIndex = 0; sceneIndex < EditorBuildSettings.scenes.Length; sceneIndex++) {
                EditorBuildSettingsScene scene = EditorBuildSettings.scenes[sceneIndex];
                if (scene.enabled is false)
                    continue;

                classGenerator.Children.Add(new FieldGenerator {
                    Name = Path.GetFileNameWithoutExtension(scene.path),
                    Modifiers = {
                        ModifierKeyword.Public,
                        ModifierKeyword.Static
                    },
                    Type = typeof(int),
                    DefaultValue = sceneIndex.ToString()
                });
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
            
            FileWriter.WriteContent("Assets/General/Generations/Data/SceneID.cs", fileGenerator.GenerateContent());
        }
    }
}