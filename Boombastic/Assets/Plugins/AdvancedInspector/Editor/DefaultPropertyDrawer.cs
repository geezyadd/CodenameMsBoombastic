using UnityEditor;

namespace AdvancedInspector.Editor {
    internal class DefaultPropertyDrawer : ICustomPropertyDrawer {
        private readonly SerializedObject _serializedObject;
        private readonly string _propertyName;

        internal DefaultPropertyDrawer(SerializedObject serializedObject, string propertyName) {
            _serializedObject = serializedObject;
            _propertyName = propertyName;
        }

        public void Draw() {
            SerializedProperty serializedProperty = _serializedObject.FindProperty(_propertyName);
            if (serializedProperty is not null)
                EditorGUILayout.PropertyField(serializedProperty, true);
        }
    }
}