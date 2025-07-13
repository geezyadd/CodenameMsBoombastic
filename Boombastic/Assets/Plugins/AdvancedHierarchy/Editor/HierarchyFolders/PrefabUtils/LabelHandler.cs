using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AdvancedHierarchy.Editor.HierarchyFolders.PrefabUtils {
    public class LabelHandler : AssetPostprocessor {
        public const string FOLDER_PREFAB_LABEL = "FolderUser";

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] _, string[] __, string[] ___) {
            using (AssetImportGrouper.Initialize())
                foreach (string assetPath in importedAssets)
                    if (assetPath.EndsWith(".prefab"))
                        HandlePrefabLabels(assetPath);
        }

        private static void HandlePrefabLabels(string assetPath) {
            GameObject asset = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

            if (asset.GetComponentsInChildren<HierarchyFolder>().Length == 0)
                RemoveFolderLabel(asset, assetPath);
            else
                AddFolderLabel(asset, assetPath);
        }

        private static void RemoveFolderLabel(GameObject assetObject, string assetPath) {
            string[] labels = AssetDatabase.GetLabels(assetObject);

            if (!labels.Contains(FOLDER_PREFAB_LABEL))
                return;

            ArrayUtility.Remove(ref labels, FOLDER_PREFAB_LABEL);
            AssetDatabase.SetLabels(assetObject, labels);
            AssetDatabase.ImportAsset(assetPath);
        }

        private static void AddFolderLabel(GameObject assetObject, string assetPath) {
            string[] labels = AssetDatabase.GetLabels(assetObject);

            if (labels.Contains(FOLDER_PREFAB_LABEL))
                return;

            ArrayUtility.Add(ref labels, FOLDER_PREFAB_LABEL);
            AssetDatabase.SetLabels(assetObject, labels);
            AssetDatabase.ImportAsset(assetPath);
        }
    }
}