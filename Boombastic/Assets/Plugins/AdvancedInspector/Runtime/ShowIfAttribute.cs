using System;

namespace AdvancedInspector {
    /// <summary>
    /// Shows fields in the Inspector based on a condition.
    /// </summary>
    public class ShowIfAttribute : Attribute {
        public string ConditionName { get; }
        public ConditionOperation Operation { get; }
        public object Comparand { get; }
        public object Reference { get; }
        public bool HardCondition { get; }

        /// <summary>
        /// Find Field or Method to validate
        /// </summary>
        /// <param name="conditionName">Name of Boolean field or method in the class.</param>
        public ShowIfAttribute(string conditionName) =>
            ConditionName = conditionName;
        
        /// <summary>
        /// Validate two values by custom condition
        /// </summary>
        /// <param name="operation">The comparison operation to be used for evaluating the condition (e.g., equals, and, or, less etc.).</param>
        /// <param name="comparand">The value to be compared. This is the object that will be used for comparison with the field.</param>
        /// <param name="reference">The reference value to be used for comparison. This serves as the target for the comparison with the field.</param>
        public ShowIfAttribute(ConditionOperation operation, object comparand, object reference) {
            Operation = operation;
            Comparand = comparand;
            Reference = reference;
        }

        /// <summary>
        /// Validate two values using equals operation
        /// </summary>
        /// <param name="comparand">The value to be compared. This is the object that will be used for comparison with the field.</param>
        /// <param name="reference">The reference value to be used for comparison. This serves as the target for the comparison with the field.</param>
        public ShowIfAttribute(object comparand, object reference) {
            Operation = ConditionOperation.Equals;
            Comparand = comparand;
            Reference = reference;
        }
        
        /// <summary>
        /// Validate two values by custom condition
        /// </summary>
        /// <param name="operation">The comparison operation to be used for evaluating the condition (e.g., equals, and, or, less etc.).</param>
        /// <param name="comparand">The value to be compared. This is the object that will be used for comparison with the field.</param>
        /// <param name="reference">The reference value to be used for comparison. This serves as the target for the comparison with the field.</param>
        /// /// <param name="hardCondition">Disables the search for fields or methods in the class</param>
        public ShowIfAttribute(ConditionOperation operation, object comparand, object reference, bool hardCondition) {
            Operation = operation;
            Comparand = comparand;
            Reference = reference;
            HardCondition = hardCondition;
        }
        
        /// <summary>
        /// Validate two values using equals operation
        /// </summary>
        /// <param name="comparand">The value to be compared. This is the object that will be used for comparison with the field.</param>
        /// <param name="reference">The reference value to be used for comparison. This serves as the target for the comparison with the field.</param>
        /// <param name="hardCondition">Disables the search for fields or methods in the class</param>
        public ShowIfAttribute(object comparand, object reference, bool hardCondition) {
            Operation = ConditionOperation.Equals;
            Comparand = comparand;
            Reference = reference;
            HardCondition = hardCondition;
        }
    }
}