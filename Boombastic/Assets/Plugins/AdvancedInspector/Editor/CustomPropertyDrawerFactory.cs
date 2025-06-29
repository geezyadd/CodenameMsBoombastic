using System;
using System.Reflection;
using UnityEditor;
using Object = UnityEngine.Object;

namespace AdvancedInspector.Editor {
    internal class CustomPropertyDrawerFactory : ICustomPropertyDrawerFactory {
        private readonly Object _target;
        private readonly SerializedObject _serializedObject;

        internal CustomPropertyDrawerFactory(Object target, SerializedObject serializedObject) {
            _target = target;
            _serializedObject = serializedObject;
        }
        
        public ICustomPropertyDrawer Create(FieldInfo fieldInfo) {
            ShowIfAttribute showIfAttribute = (ShowIfAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(ShowIfAttribute));
            if (showIfAttribute is not null)
                return new ShowIfAttributePropertyDrawer(new ShowHideAttributeComparator(_target), _serializedObject, showIfAttribute, fieldInfo);
            
            HideIfAttribute hideIfAttribute = (HideIfAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(HideIfAttribute));
            if (hideIfAttribute is not null)
                return new HideIfAttributePropertyDrawer(new ShowHideAttributeComparator(_target), _serializedObject, hideIfAttribute, fieldInfo);
            
            return new DefaultPropertyDrawer(_serializedObject, fieldInfo.Name);
        }
    }
}