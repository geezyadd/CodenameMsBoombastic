using System;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AdvancedInspector.Editor {
    internal class ShowHideAttributeComparator {
        private readonly Object _target;

        internal ShowHideAttributeComparator(Object target) =>
            _target = target;

        internal bool EvaluateCondition(string conditionName) {
            object objectByConditionName = GetValueFromFieldOrMethod(conditionName);
            if (objectByConditionName is bool value)
                return value;
            
            Debug.LogWarning($"Field or Method with name '{conditionName}' not found or does not return a boolean value in {_target.GetType()}.");
            return false; 
        }

        internal bool EvaluateCondition(ConditionOperation operation, object comparand, object reference, bool hardCondition) {
            if (comparand == null || reference == null) {
                Debug.LogWarning("Comparand or reference is null.");
                return false;
            }
            
            Type comparandType = comparand.GetType();
            Type referenceType = reference.GetType();
            
            if (hardCondition && comparandType != referenceType) {
                Debug.LogWarning($"Types do not match: {comparandType} vs {referenceType}.");
                return false;
            }

            if (hardCondition is false) {
                object comparandObject = comparand is string comparandValue ? GetValueFromFieldOrMethod(comparandValue) : comparand;
                object referenceObject = reference is string referenceValue ? GetValueFromFieldOrMethod(referenceValue) : reference;
                return Compare(operation, comparandObject, referenceObject);
            }

            return Compare(operation, comparand, reference);
        }

        private object GetValueFromFieldOrMethod(string searchName) {
            FieldInfo conditionField = _target.GetType().GetField(searchName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (conditionField != null)
                return conditionField.GetValue(_target);
            
            MethodInfo conditionMethod = _target.GetType().GetMethod(searchName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (conditionMethod != null)
                return conditionMethod.Invoke(_target, null);

            return null;
        }

        private bool Compare(ConditionOperation operation, object comparand, object reference) =>
            operation switch {
                ConditionOperation.Equals => EqualsOperation(comparand, reference),
                ConditionOperation.NotEquals => NotEqualsOperation(comparand, reference),
                ConditionOperation.And => AndOperation(comparand, reference),
                ConditionOperation.Or => OrOperation(comparand, reference),
                ConditionOperation.Less => LessOperation(comparand, reference),
                ConditionOperation.LessEquals => LessEqualsOperation(comparand, reference),
                ConditionOperation.Greater => GreaterOperation(comparand, reference),
                ConditionOperation.GreaterEquals => GreaterEqualsOperation(comparand, reference),
                _ => throw new ArgumentOutOfRangeException(nameof(operation), operation, null)
            };

        private bool EqualsOperation(object comparand, object reference) =>
            comparand.Equals(reference);

        private bool NotEqualsOperation(object comparand, object reference) =>
            comparand.Equals(reference) is false;

        private bool AndOperation(object comparand, object reference) {
            if (comparand is bool comparandValue && reference is bool referenceValue)
                return comparandValue && referenceValue;

            Debug.LogWarning("Comparand or reference is not boolean.");
            return false;
        }

        private bool OrOperation(object comparand, object reference) {
            if (comparand is bool comparandValue && reference is bool referenceValue)
                return comparandValue || referenceValue;

            Debug.LogWarning("Comparand or reference is not boolean.");
            return false;
        }

        private bool LessOperation(object comparand, object reference) {
            if (comparand is IComparable && reference is IComparable)
                return ((IComparable)comparand).CompareTo(reference) < 0;
            
            Debug.LogWarning("Comparand or reference is not comparable.");
            return false;
        }

        private bool LessEqualsOperation(object comparand, object reference) {
            if (comparand is IComparable && reference is IComparable)
                return ((IComparable)comparand).CompareTo(reference) <= 0;
            
            Debug.LogWarning("Comparand or reference is not comparable.");
            return false;
        }

        private bool GreaterOperation(object comparand, object reference) {
            if (comparand is IComparable && reference is IComparable)
                return ((IComparable)comparand).CompareTo(reference) > 0;
            
            Debug.LogWarning("Comparand or reference is not comparable.");
            return false;
        }

        private bool GreaterEqualsOperation(object comparand, object reference) {
            if (comparand is IComparable && reference is IComparable)
                return ((IComparable)comparand).CompareTo(reference) >= 0;
            
            Debug.LogWarning("Comparand or reference is not comparable.");
            return false;
        }
    }
}