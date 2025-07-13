using System.Reflection;
using UnityEditor;

namespace AdvancedInspector.Editor {
    internal class HideIfAttributePropertyDrawer : ICustomPropertyDrawer {
        private readonly ShowHideAttributeComparator _comparator;
        private readonly SerializedObject _serializedObject;
        private readonly HideIfAttribute _hideIfAttribute;
        private readonly FieldInfo _fieldInfo;

        internal HideIfAttributePropertyDrawer(ShowHideAttributeComparator comparator, SerializedObject serializedObject, HideIfAttribute hideIfAttribute, FieldInfo fieldInfo) {
            _comparator = comparator;
            _serializedObject = serializedObject;
            _hideIfAttribute = hideIfAttribute;
            _fieldInfo = fieldInfo;
        }

        public void Draw() {
            bool shouldHide = string.IsNullOrEmpty(_hideIfAttribute.ConditionName) 
                ? _comparator.EvaluateCondition(_hideIfAttribute.Operation, _hideIfAttribute.Comparand, _hideIfAttribute.Reference, _hideIfAttribute.HardCondition) 
                : _comparator.EvaluateCondition(_hideIfAttribute.ConditionName);
                
            if (shouldHide is false) {
                SerializedProperty serializedProperty = _serializedObject.FindProperty(_fieldInfo.Name);
                if (serializedProperty is not null)
                    EditorGUILayout.PropertyField(serializedProperty, true);
            }
        }
    }
}