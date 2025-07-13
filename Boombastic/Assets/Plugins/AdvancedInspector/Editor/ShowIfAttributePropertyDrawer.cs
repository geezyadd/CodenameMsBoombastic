using System.Reflection;
using UnityEditor;

namespace AdvancedInspector.Editor {
    internal class ShowIfAttributePropertyDrawer : ICustomPropertyDrawer {
        private readonly ShowHideAttributeComparator _comparator;
        private readonly SerializedObject _serializedObject;
        private readonly ShowIfAttribute _showIfAttribute;
        private readonly FieldInfo _fieldInfo;

        internal ShowIfAttributePropertyDrawer(ShowHideAttributeComparator comparator, SerializedObject serializedObject, ShowIfAttribute showIfAttribute, FieldInfo fieldInfo) {
            _comparator = comparator;
            _serializedObject = serializedObject;
            _showIfAttribute = showIfAttribute;
            _fieldInfo = fieldInfo;
        }

        public void Draw() {
            bool shouldShow = string.IsNullOrEmpty(_showIfAttribute.ConditionName) 
                ? _comparator.EvaluateCondition(_showIfAttribute.Operation, _showIfAttribute.Comparand, _showIfAttribute.Reference, _showIfAttribute.HardCondition) 
                : _comparator.EvaluateCondition(_showIfAttribute.ConditionName);
                
            if (shouldShow) {
                SerializedProperty serializedProperty = _serializedObject.FindProperty(_fieldInfo.Name);
                if (serializedProperty is not null)
                    EditorGUILayout.PropertyField(serializedProperty, true);
            }
        }
    }
}