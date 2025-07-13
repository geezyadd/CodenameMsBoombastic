using System;
using System.IO;
using System.Linq;
using AdvancedHierarchy.Editor.Settings;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AdvancedHierarchy.Editor.HierarchyFolders.PrefabUtils {
    [InitializeOnLoad]
    public class PrefabFolderStripper : IPreprocessBuildWithReport, IPostprocessBuildWithReport {
        public int callbackOrder { get; }

        static PrefabFolderStripper() =>
            EditorApplication.playModeStateChanged += HandlePrefabsOnPlayMode;

        private static void HandlePrefabsOnPlayMode(PlayModeStateChange state) {
            if (!AdvancedHierarchySettings.StripFoldersFromPrefabsInPlayMode || AdvancedHierarchySettings.PlayMode == StrippingMode.DoNothing)
                return;

            if (state == PlayModeStateChange.ExitingEditMode)
                using (AssetImportGrouper.Initialize())
                    StripFoldersFromAllPrefabs();
            else if (state == PlayModeStateChange.ExitingPlayMode)
                RevertChanges();
        }

        private static void StripFoldersFromDependentPrefabs() {
            string[] scenePaths = EditorBuildSettings.scenes.Select(scene => scene.path).ToArray();
            string[] dependentAssetsPaths = AssetDatabase.GetDependencies(scenePaths, true);
            string[] prefabsWithLabel = dependentAssetsPaths.Where(path => AssetDatabase.GetLabels(GetAssetForLabel(path)).Contains(LabelHandler.FOLDER_PREFAB_LABEL)).ToArray();

            ChangedPrefabs.Initialize(prefabsWithLabel.Length);

            for (int i = 0; i < prefabsWithLabel.Length; i++) {
                string path = prefabsWithLabel[i];
                ChangedPrefabs.Instance[i] = (AssetDatabase.AssetPathToGUID(path), File.ReadAllText(path));
                StripFoldersFromPrefab(path, AdvancedHierarchySettings.Build);
            }
        }

        private static GUID GetAssetForLabel(string path) =>
            AssetDatabase.GUIDFromAssetPath(path);

        private static void StripFoldersFromAllPrefabs() {
            string[] prefabGUIDs = AssetDatabase.FindAssets($"l: {LabelHandler.FOLDER_PREFAB_LABEL}");
            ChangedPrefabs.Initialize(prefabGUIDs.Length);

            for (int i = 0; i < prefabGUIDs.Length; i++) {
                string guid = prefabGUIDs[i];
                string path = AssetDatabase.GUIDToAssetPath(guid);

                ChangedPrefabs.Instance[i] = (guid, File.ReadAllText(path));
                StripFoldersFromPrefab(path, AdvancedHierarchySettings.PlayMode);
            }

            ChangedPrefabs.SerializeIfNeeded();
        }

        private static void StripFoldersFromPrefab(string prefabPath, StrippingMode strippingMode) {
            using EditPrefabContentsScope temp = new(prefabPath);
            HierarchyFolder[] folders = temp.PrefabContentsRoot.GetComponentsInChildren<HierarchyFolder>();

            foreach (HierarchyFolder folder in folders) {
                if (folder.gameObject == temp.PrefabContentsRoot) {
                    Debug.LogWarning($"Hierarchy will not flatten for {prefabPath} because its root is a folder. " + "It's advised to make the root an empty game object.");
                    Object.DestroyImmediate(folder);
                }
                else {
                    folder.Flatten(strippingMode, AdvancedHierarchySettings.CapitalizeName);
                }
            }
        }

        private static void RevertChanges() {
            foreach ((string guid, string content) in ChangedPrefabs.Instance) {
                string path = AssetDatabase.GUIDToAssetPath(guid);

                if (string.IsNullOrEmpty(path) || !File.Exists(path))
                    continue; 

                File.WriteAllText(path, content);
            }

            AssetDatabase.Refresh();
        }

        public void OnPostprocessBuild(BuildReport report) {
            if (AdvancedHierarchySettings.StripFoldersFromPrefabsInBuild is false)
                return;

            RevertChanges();
        }

        public void OnPreprocessBuild(BuildReport report) {
            if (AdvancedHierarchySettings.StripFoldersFromPrefabsInBuild is false)
                return;

            using (AssetImportGrouper.Initialize())
                StripFoldersFromDependentPrefabs();
        }

        private readonly struct EditPrefabContentsScope : IDisposable {
            public readonly GameObject PrefabContentsRoot;
            private readonly string _assetPath;

            public EditPrefabContentsScope(string assetPath) {
                PrefabContentsRoot = PrefabUtility.LoadPrefabContents(assetPath);
                _assetPath = assetPath;
            }

            public void Dispose() {
                PrefabUtility.SaveAsPrefabAsset(PrefabContentsRoot, _assetPath);
                PrefabUtility.UnloadPrefabContents(PrefabContentsRoot);
            }
        }
    }
}