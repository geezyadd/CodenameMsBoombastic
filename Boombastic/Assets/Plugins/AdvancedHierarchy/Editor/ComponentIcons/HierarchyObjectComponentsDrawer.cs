using System;
using System.Collections.Generic;
using System.Linq;
using AdvancedHierarchy.Editor.Settings;
using UnityEditor;
using UnityEngine;

namespace AdvancedHierarchy.Editor.ComponentIcons {
    [InitializeOnLoad]
    public static class HierarchyObjectComponentsDrawer {
        private const float TEXT_WIDTH_OFFSET = 20f;

        static HierarchyObjectComponentsDrawer() =>
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowItemGUI;

        private static void OnHierarchyWindowItemGUI(int instanceID, Rect selectionRect) {
            if (EditorUtility.InstanceIDToObject(instanceID) is not GameObject gameObject)
                return;
 
            Component[] components = gameObject.GetComponents<Component>();
            if (components.Any(component => component is HierarchyFolder))
                return;
            
            List<Component> filteredComponents = new();
            for (int i = 0; i < components.Length; i++) {
                Component component = components[i];
                if (component is null)
                    continue;
                
                Type type = component.GetType();
                GUIContent content = EditorGUIUtility.ObjectContent(component, type);
                
                if((type == typeof(Transform) || type.IsSubclassOf(typeof(Transform))) && (AdvancedHierarchySettings.ShowTransforms is false || AdvancedHierarchySettings.ShowTransformsWhenSingle))
                    continue;
                
                if(AdvancedHierarchySettings.ShowDefaultScriptIcons is false && content.image == EditorGUIUtility.IconContent("cs Script Icon").image)
                    continue;
                    
                filteredComponents.Add(component);
            }

            if (filteredComponents.Count > AdvancedHierarchySettings.MaxComponents)
                filteredComponents.RemoveRange(AdvancedHierarchySettings.MaxComponents, filteredComponents.Count - AdvancedHierarchySettings.MaxComponents);

            if (filteredComponents.Count == 0 && AdvancedHierarchySettings.ShowTransforms && AdvancedHierarchySettings.ShowTransformsWhenSingle && AdvancedHierarchySettings.MaxComponents > 0)
                filteredComponents.Add(components[0]);
      
            float textWidth = EditorStyles.label.CalcSize(new GUIContent(gameObject.name)).x + TEXT_WIDTH_OFFSET;
            for (int i = 0; i < filteredComponents.Count; i++) {
                if (textWidth >= selectionRect.width - AdvancedHierarchySettings.IconsSize * (i + 1))
                    break;
                
                Component component = filteredComponents[i];
                Type type = component.GetType();
                GUIContent content = EditorGUIUtility.ObjectContent(component, type);
                content.text = null;
                content.tooltip = null;
                
                float x = selectionRect.xMax - AdvancedHierarchySettings.IconsSize * (i + 1) - 2 * i;
                float y = selectionRect.yMin + (selectionRect.height - AdvancedHierarchySettings.IconsSize) / 2;
                Rect iconRect = new(x, y, AdvancedHierarchySettings.IconsSize, AdvancedHierarchySettings.IconsSize);

                if (AdvancedHierarchySettings.OpenComponentProperties) {
                    if (GUI.Button(iconRect, content, EditorStyles.label))
                        EditorUtility.OpenPropertyEditor(component);
                }
                else
                    GUI.Label(iconRect, content, EditorStyles.label);
            }
        }
    }
}