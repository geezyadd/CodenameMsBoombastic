using System;

namespace AdvancedInspector {
    /// <summary>
    /// Draw buttons in Inspector GUI
    /// </summary>
    public class ButtonAttribute : Attribute {
        public string ButtonLabel { get; }

        /// <summary>
        /// Draws a button with the default label (method name will be used).
        /// </summary>
        public ButtonAttribute() =>
            ButtonLabel = null;

        /// <summary>
        /// Draws a button with a custom label in the Inspector.
        /// </summary>
        /// <param name="buttonLabel">The label to display on the button.</param>
        public ButtonAttribute(string buttonLabel) =>
            ButtonLabel = buttonLabel;
    }
}