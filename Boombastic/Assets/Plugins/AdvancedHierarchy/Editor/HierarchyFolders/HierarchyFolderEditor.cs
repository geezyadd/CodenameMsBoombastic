using System;
using UnityEditor;
using UnityEngine;

namespace AdvancedHierarchy.Editor.HierarchyFolders {
    [CanEditMultipleObjects]
    [CustomEditor(typeof(HierarchyFolder), true)]
    public class HierarchyFolderEditor : UnityEditor.Editor {
        private const float COLOR_PICKER_BUTTON_SIZE = 25f;
        private readonly Color _backgroundColor = new(1f, 1f, 1f, 0.3f);
        private readonly Color _backgroundHoverColor = new(1f, 1f, 1f, 0.1f);
        private SerializedProperty _colorIndexProperty;

        public override void OnInspectorGUI() {
            RenderColorPicker();
        }

        private void RenderColorPicker() {
            EditorGUILayout.BeginHorizontal();

            _colorIndexProperty ??= serializedObject.FindProperty("_colorIndex");
            int currentIndex = _colorIndexProperty.intValue;
            Rect gridRect = EditorGUILayout.GetControlRect(false, COLOR_PICKER_BUTTON_SIZE * HierarchyFolderIcon.IconRowCount, GUILayout.Width(COLOR_PICKER_BUTTON_SIZE * HierarchyFolderIcon.IconColumnCount));
            float height = gridRect.height / HierarchyFolderIcon.IconRowCount;
            float width = gridRect.width / HierarchyFolderIcon.IconColumnCount;
            GUIStyle backgroundGUIStyle = "WhiteBackground";

            for (int row = 0; row < HierarchyFolderIcon.IconRowCount; row++) {
                for (int column = 0; column < HierarchyFolderIcon.IconColumnCount; column++) {
                    int index = 1 + column + row * HierarchyFolderIcon.IconColumnCount;
                    Rect rect = new(gridRect.xMin + width * column, gridRect.yMin + height * row, width, height);
                    FolderIconsHolder folderIconsHolder = HierarchyFolderIcon.ColoredFolderIcons(index);

                    if (Event.current.type == EventType.Repaint) {
                        if (index == currentIndex) {
                            GUI.backgroundColor = _backgroundHoverColor;
                            backgroundGUIStyle.Draw(rect, false, false, false, false);
                        }
                        else if (rect.Contains(Event.current.mousePosition)) {
                            GUI.backgroundColor = _backgroundColor;
                            backgroundGUIStyle.Draw(rect, false, false, true, false);
                        }
                    }

                    if (GUI.Button(rect, currentIndex == index ? folderIconsHolder.OpenFolder : folderIconsHolder.ClosedFolder, EditorStyles.label)) {
                        Undo.RecordObject(target, "Set Hierarchy Folder Color");
                        _colorIndexProperty.intValue = currentIndex == index ? 0 : index;

                        serializedObject.ApplyModifiedProperties();
                        EditorApplication.RepaintHierarchyWindow();
                        GUIUtility.ExitGUI();
                    }
                }
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}