using System;
using UnityEditor;

namespace AdvancedHierarchy.Editor.HierarchyFolders.PrefabUtils {
    internal class AssetImportGrouper : IDisposable {
        private static AssetImportGrouper _instance;

        public static AssetImportGrouper Initialize() {
            AssetDatabase.StartAssetEditing();
            return _instance ??= new AssetImportGrouper();
        }

        public void Dispose() =>
            AssetDatabase.StopAssetEditing();
    }
}