using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace AdvancedInspector.Editor {
    [CustomEditor(typeof(MonoBehaviour), true), CanEditMultipleObjects]
    public class MonoBehaviourEditor : UnityEditor.Editor {
        private ICustomPropertyDrawerFactory _propertyDrawerFactory;
        private FieldInfo[] _fields;
        private MethodInfo[] _methods;

        private void OnEnable() =>
            Initialize();

        public override void OnInspectorGUI() {
            DrawFields();
            DrawButtons();
        }

        private void Initialize() {
            _propertyDrawerFactory = new CustomPropertyDrawerFactory(target, serializedObject);
            _fields = target.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            _methods = target.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        }

        private void DrawFields() {
            serializedObject.Update();
            
            foreach (FieldInfo fieldInfo in _fields) {
                ICustomPropertyDrawer propertyDrawer = _propertyDrawerFactory.Create(fieldInfo); 
                propertyDrawer.Draw();
            }
            
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawButtons() {
            foreach (MethodInfo method in _methods) {
                ButtonAttribute buttonAttribute = (ButtonAttribute)Attribute.GetCustomAttribute(method, typeof(ButtonAttribute));

                if (buttonAttribute == null)
                    continue;

                string buttonName = string.IsNullOrEmpty(buttonAttribute.ButtonLabel) ? GetFilteredName(method.Name) : buttonAttribute.ButtonLabel;
                if (GUILayout.Button(buttonName)) {
                    if (method.GetParameters().Length == 0)
                        method.Invoke(target, null);
                    else
                        Debug.LogWarning($"Method '{method.Name}' cannot be invoked because it has parameters.");
                }
            }
        }

        private string GetFilteredName(string buttonLabel) {
            string filteredLabel = string.Empty;
            foreach (char c in buttonLabel)
                if (char.IsLetterOrDigit(c))
                    filteredLabel += c;
            
            string result = filteredLabel.Length > 0 ? $"{filteredLabel[0]}" : string.Empty;

            for (int charIndex = 1; charIndex < filteredLabel.Length; charIndex++) {
                char c = filteredLabel[charIndex];
                result += char.IsUpper(c) ? " " + c : c;
            }

            return result;
        }
    }
}