using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AdvancedHierarchy.Editor.Settings {
    internal static class SettingsDrawer {
        private static readonly Dictionary<string, string> FieldNames = new() {
            {
                nameof(AdvancedHierarchySettings.MaxComponents), "Max Components"
            }, {
                nameof(AdvancedHierarchySettings.IconsSize), "Icons Size"
            }, {
                nameof(AdvancedHierarchySettings.ShowTransforms), "Show Transforms"
            }, {
                nameof(AdvancedHierarchySettings.ShowTransformsWhenSingle), "Show Transforms When Single"
            }, {
                nameof(AdvancedHierarchySettings.ShowDefaultScriptIcons), "Show Default Script Icons"
            }, {
                nameof(AdvancedHierarchySettings.OpenComponentProperties), "Open Component Properties"
            }, {
                nameof(AdvancedHierarchySettings.PlayMode), "Play Mode Stripping Type"
            }, {
                nameof(AdvancedHierarchySettings.Build), "Build Stripping Type"
            }, {
                nameof(AdvancedHierarchySettings.CapitalizeName), "Capitalize Folder Names"
            }, {
                nameof(AdvancedHierarchySettings.StripFoldersFromPrefabsInPlayMode), "Strip folders from prefabs in Play Mode"
            }, {
                nameof(AdvancedHierarchySettings.StripFoldersFromPrefabsInBuild), "Strip folders from prefabs in build"
            }
        };

        private static readonly GUIContent BuildStrippingName = new(FieldNames[nameof(AdvancedHierarchySettings.Build)]);

        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider() {
            return new SettingsProvider("Preferences/Advanced Hierarchy", SettingsScope.User) {
                guiHandler = OnGUI, 
                keywords = GetKeywords()
            };
        }

        private static void OnGUI(string searchContext) {
            GUILayout.Space(12);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(12);
            EditorGUILayout.BeginVertical();
            
            GUILayout.Label("Component Icons", EditorStyles.boldLabel);

            using (new TemporaryLabelWidth(230f)) {
                AdvancedHierarchySettings.MaxComponents = EditorGUILayout.IntSlider(FieldNames[nameof(AdvancedHierarchySettings.MaxComponents)], AdvancedHierarchySettings.MaxComponents, 0, 5);
                AdvancedHierarchySettings.IconsSize = EditorGUILayout.IntSlider(FieldNames[nameof(AdvancedHierarchySettings.IconsSize)], AdvancedHierarchySettings.IconsSize, 10, 20);
                AdvancedHierarchySettings.ShowTransforms = EditorGUILayout.Toggle(FieldNames[nameof(AdvancedHierarchySettings.ShowTransforms)], AdvancedHierarchySettings.ShowTransforms);
                AdvancedHierarchySettings.ShowTransformsWhenSingle = EditorGUILayout.Toggle(FieldNames[nameof(AdvancedHierarchySettings.ShowTransformsWhenSingle)], AdvancedHierarchySettings.ShowTransformsWhenSingle);
                AdvancedHierarchySettings.ShowDefaultScriptIcons = EditorGUILayout.Toggle(FieldNames[nameof(AdvancedHierarchySettings.ShowDefaultScriptIcons)], AdvancedHierarchySettings.ShowDefaultScriptIcons);
                AdvancedHierarchySettings.OpenComponentProperties = EditorGUILayout.Toggle(FieldNames[nameof(AdvancedHierarchySettings.OpenComponentProperties)], AdvancedHierarchySettings.OpenComponentProperties);
            }

            GUILayout.Space(10);
            GUILayout.Label("Stripping", EditorStyles.boldLabel);
            
            AdvancedHierarchySettings.PlayMode = (StrippingMode)EditorGUILayout.EnumPopup(FieldNames[nameof(AdvancedHierarchySettings.PlayMode)], AdvancedHierarchySettings.PlayMode);
            if (AdvancedHierarchySettings.PlayMode == StrippingMode.ReplaceWithSeparator)
                AdvancedHierarchySettings.CapitalizeName = EditorGUILayout.Toggle(FieldNames[nameof(AdvancedHierarchySettings.CapitalizeName)], AdvancedHierarchySettings.CapitalizeName);
            
            AdvancedHierarchySettings.Build = (StrippingMode)EditorGUILayout.EnumPopup(BuildStrippingName, AdvancedHierarchySettings.Build, TypeCanBeInBuild, true);
            if (AdvancedHierarchySettings.StripFoldersFromPrefabsInPlayMode)
                EditorGUILayout.HelpBox("If you notice that entering play mode takes too long, you can try disabling this option. " + "Folders will not be stripped from prefabs that are instantiated at runtime, but if performance in " + "Play Mode does not matter, you will be fine.", MessageType.Info);

            using (new TemporaryLabelWidth(230f)) {
                AdvancedHierarchySettings.StripFoldersFromPrefabsInPlayMode = EditorGUILayout.Toggle(FieldNames[nameof(AdvancedHierarchySettings.StripFoldersFromPrefabsInPlayMode)], AdvancedHierarchySettings.StripFoldersFromPrefabsInPlayMode);
                AdvancedHierarchySettings.StripFoldersFromPrefabsInBuild = EditorGUILayout.Toggle(FieldNames[nameof(AdvancedHierarchySettings.StripFoldersFromPrefabsInBuild)], AdvancedHierarchySettings.StripFoldersFromPrefabsInBuild);
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

        private static HashSet<string> GetKeywords() {
            HashSet<string> keywords = new();

            foreach (string fieldName in FieldNames.Values)
                foreach (string word in fieldName.Split(' '))
                    keywords.Add(word);

            return keywords;
        }

        private static bool TypeCanBeInBuild(Enum enumValue) =>
            (StrippingMode)enumValue is StrippingMode.PrependWithFolderName or StrippingMode.Delete;

        private readonly struct TemporaryLabelWidth : IDisposable {
            private readonly float _oldWidth;

            public TemporaryLabelWidth(float width) {
                _oldWidth = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = width;
            }

            public void Dispose() {
                EditorGUIUtility.labelWidth = _oldWidth;
            }
        }
    }
}