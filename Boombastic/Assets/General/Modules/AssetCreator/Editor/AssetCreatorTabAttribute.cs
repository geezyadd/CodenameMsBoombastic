using System;

namespace AssetCreator {
    public class AssetCreatorTabAttribute : Attribute {
        public readonly string DisplayName;

        public AssetCreatorTabAttribute(string displayName) =>
            DisplayName = displayName;
    }
}