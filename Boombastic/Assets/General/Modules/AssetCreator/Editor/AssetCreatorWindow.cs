using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace AssetCreator {
    public class AssetCreatorWindow : EditorWindow {
        private readonly List<TabInfo> _tabs = new();
        private int _selectedTab;
        private Vector2 _tabsScrollPosition;
        private Vector2 _editorScrollPosition;

        private Editor _currentEditor;
        private UnityEngine.Object _targetObject;

        [MenuItem("Tools/Asset Creator")]
        public static void Open() =>
            GetWindow<AssetCreatorWindow>("Asset Creator");

        private void OnEnable() {
            UpdateTabsInfo();
            CreateDummyTarget();
            CreateEditorInstance();
        }

        private void OnDisable() {
            if (_currentEditor != null)
                DestroyImmediate(_currentEditor);

            if (_targetObject != null)
                DestroyImmediate(_targetObject);
        }

        private void UpdateTabsInfo() {
            _tabs.Clear();

            IEnumerable<Type> editorTypes = AppDomain.CurrentDomain.GetAssemblies()
                                                     .SelectMany(a => a.GetTypes())
                                                     .Where(t => typeof(Editor).IsAssignableFrom(t) && !t.IsAbstract);

            foreach (Type type in editorTypes) {
                AssetCreatorTabAttribute attribute = type.GetCustomAttribute<AssetCreatorTabAttribute>();
                if (attribute != null) {
                    _tabs.Add(new TabInfo {
                        Name = attribute.DisplayName, 
                        EditorType = type
                    });
                }
            }
        }

        private void CreateDummyTarget() {
            if (_targetObject == null) {
                _targetObject = CreateInstance<ScriptableObject>();
                _targetObject.hideFlags = HideFlags.HideAndDontSave;
            }
        }

        private void CreateEditorInstance() {
            if (_currentEditor != null) {
                DestroyImmediate(_currentEditor);
                _currentEditor = null;
            }

            if (_tabs.Count == 0)
                return;

            Type editorType = _tabs[_selectedTab].EditorType;
            _currentEditor = Editor.CreateEditor(_targetObject, editorType);
        }

        private void OnGUI() {
            EditorGUILayout.BeginHorizontal();

            _tabsScrollPosition = EditorGUILayout.BeginScrollView(_tabsScrollPosition, GUILayout.Width(150));
            GUIStyle buttonStyle = new("ObjectPickerTab") {
                alignment = TextAnchor.MiddleLeft, 
                margin = new RectOffset(0, 0, 0, 0)
            };

            EditorGUILayout.BeginVertical();
            for (int i = 0; i < _tabs.Count; i++) {
                bool selected = (i == _selectedTab);
                if (GUILayout.Toggle(selected, _tabs[i].Name, buttonStyle, GUILayout.ExpandWidth(true))) {
                    if (_selectedTab != i) {
                        _selectedTab = i;
                        CreateEditorInstance();
                        Repaint();
                    }
                }
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
            
            _editorScrollPosition = EditorGUILayout.BeginScrollView(_editorScrollPosition);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(12);

            EditorGUILayout.BeginVertical();
            GUIStyle labelStyle = new(EditorStyles.label) {
                fontSize = 16, 
                fontStyle = FontStyle.Bold
            };

            GUILayout.Label(_tabs[_selectedTab].Name, labelStyle);
            GUILayout.Space(12);
            _currentEditor.OnInspectorGUI();
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();

            EditorGUILayout.EndHorizontal();
        }

        private struct TabInfo {
            public string Name { get; set; }
            public Type EditorType { get; set; }
        }
    }
}