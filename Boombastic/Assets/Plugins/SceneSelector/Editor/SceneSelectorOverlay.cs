using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.SceneManagement;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Plugins.SceneSelector.Editor {
    [Overlay(typeof(SceneView), "Scene Selector"), Icon(ICON_PATH)]
    public class SceneSelectorOverlay : ToolbarOverlay {
        private const string ICON_PATH = "SceneAsset Icon";
        private const string TOOLBAR_ID = "SceneSelectorOverlay/SceneDropDownToggle";

        public SceneSelectorOverlay() : base(TOOLBAR_ID) { }
        
        [EditorToolbarElement(TOOLBAR_ID, typeof(SceneView))]
        private class SceneDropDownToggle : EditorToolbarDropdown, IAccessContainerWindow {
            public EditorWindow containerWindow { get; set; }

            public SceneDropDownToggle() {
                icon = EditorGUIUtility.IconContent("SceneAsset Icon").image as Texture2D;
                clicked += ShowSceneMenu;
            }

            private void ShowSceneMenu() {
                GenericMenu menu = new();

                List<Scene> scenes = GetLoadedScenes();
                
                foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes) {
                    string sceneName = Path.GetFileNameWithoutExtension(scene.path);
                    bool loaded = scenes.Any(loadedScene => loadedScene.name == sceneName);
                    bool isActive = SceneManager.GetActiveScene().name == sceneName;
                    menu.AddItem(new GUIContent(sceneName + "/Single"), loaded && isActive, () => LoadSingleScene(scene.path));
                    menu.AddItem(new GUIContent(sceneName + "/Additive"), loaded && !isActive, () => LoadAdditiveScene(scene.path));
                }
                
                menu.AddSeparator("");
                menu.AddItem(new GUIContent("Scene List"), false, OpenSceneList);

                menu.ShowAsContext();
            }
            
            private void LoadSingleScene(string scenePath) {
                List<Scene> scenes = GetLoadedScenes();

                if (scenes.Any(scene => scene.isDirty))
                    EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                
                EditorSceneManager.OpenScene(scenePath);
            }

            private void LoadAdditiveScene(string scenePath) =>
                EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);

            private void OpenSceneList() =>
                EditorApplication.ExecuteMenuItem("File/Build Profiles");

            private List<Scene> GetLoadedScenes() {
                List<Scene> scenes = new();
                for (int i = 0; i < SceneManager.sceneCount; i++)
                    scenes.Add(SceneManager.GetSceneAt(i));

                return scenes;
            }
        }
    }
}
