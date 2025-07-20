using System.Reflection;
using AssetCreator;
using UnityEditor;
using UnityEngine;

namespace UIWorkflow.Editor {
    [AssetCreatorTab("UI Windows")]
    public class WindowCreatorTab : UnityEditor.Editor {
        private GameObject _prefabTemplate;
        private string _windowName;
        private string _prefabSavePath;
        private string _classNamespace;
        private string _classSavePath;

        private void OnEnable() {
            _prefabTemplate = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/General/Modules/UIWorkflow/Editor/Prefabs/DefaultWindowTemplate.prefab");
            _classNamespace = "UIWorkflow.Windows";
            
            if (TryGetActiveFolderPath(out string currentFolderPath)) {
                _prefabSavePath = currentFolderPath;
                _classSavePath = currentFolderPath;
                return;
            }
            
            if (string.IsNullOrEmpty(_prefabSavePath))
                _prefabSavePath = "Assets/";
            if (string.IsNullOrEmpty(_classSavePath))
                _classSavePath = "Assets/";
        }

        public override void OnInspectorGUI() {
            _prefabTemplate = (GameObject)EditorGUILayout.ObjectField("Template", _prefabTemplate, typeof(GameObject), false);
            _windowName = EditorGUILayout.TextField("Window Name", _windowName);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Prefab Save Path", _prefabSavePath);
            if (GUILayout.Button("Browse", GUILayout.MaxWidth(60))) {
                string selectedPath = EditorUtility.OpenFolderPanel("Select Save Folder", _prefabSavePath, "");
                if (!string.IsNullOrEmpty(selectedPath)) {
                    if (selectedPath.StartsWith(Application.dataPath))
                        _prefabSavePath = "Assets" + selectedPath.Substring(Application.dataPath.Length);
                    else
                        Debug.LogError("Selected path must be inside Assets folder.");
                }
            }
            EditorGUILayout.EndHorizontal();
            
            _classNamespace = EditorGUILayout.TextField("Class Namespace", _classNamespace);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Class Save Path", _classSavePath);
            if (GUILayout.Button("Browse", GUILayout.MaxWidth(60))) {
                string selectedPath = EditorUtility.OpenFolderPanel("Select Save Folder", _classSavePath, "");
                if (!string.IsNullOrEmpty(selectedPath)) {
                    if (selectedPath.StartsWith(Application.dataPath))
                        _classSavePath = "Assets" + selectedPath.Substring(Application.dataPath.Length);
                    else
                        Debug.LogError("Selected path must be inside Assets folder.");
                }
            }
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Create Window")) {
                CreateWindowPrefab();
                CreateWindowClass();
            }

            if (GUILayout.Button("Generate Window Names"))
                GenerateWindowsNames();
        }

        private void CreateWindowPrefab() {
            if (string.IsNullOrEmpty(_windowName)) {
                Debug.LogError("Prefab name is empty.");
                return;
            }

            if (_prefabTemplate == null) {
                Debug.LogError("Template not assigned.");
                return;
            }

            GameObject instance = Instantiate(_prefabTemplate);
            instance.name = _windowName;

            string fullPath = $"{_prefabSavePath}/{_windowName}.prefab";
            fullPath = AssetDatabase.GenerateUniqueAssetPath(fullPath);

            GameObject prefabInstance = PrefabUtility.SaveAsPrefabAsset(instance, fullPath);
            Selection.activeObject = prefabInstance;
            DestroyImmediate(instance);
        }

        private void CreateWindowClass() {
            WindowClassTemplateGenerator.SetClassName(_windowName);
            WindowClassTemplateGenerator.SetNamespace(_classNamespace);
            WindowClassTemplateGenerator.SetPath(_classSavePath);
            WindowClassTemplateGenerator.Generate();
        }

        private void GenerateWindowsNames() =>
            WindowEnumGenerator.Generate();

        private static bool TryGetActiveFolderPath(out string path) {
            path = string.Empty;
            MethodInfo tryGetActiveFolderPathMethod = typeof(ProjectWindowUtil).GetMethod( "TryGetActiveFolderPath", BindingFlags.Static | BindingFlags.NonPublic );
            if (tryGetActiveFolderPathMethod == null)
                return false;
            
            object[] args = { null };
            bool result = (bool)tryGetActiveFolderPathMethod.Invoke(null, args);
            path = (string)args[0];

            return result;
        }
    }
}