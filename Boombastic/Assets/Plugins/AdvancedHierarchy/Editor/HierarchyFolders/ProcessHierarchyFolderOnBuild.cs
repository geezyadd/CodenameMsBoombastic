using AdvancedHierarchy.Editor.Settings;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AdvancedHierarchy.Editor.HierarchyFolders {
    public class ProcessHierarchyFolderOnBuild : IProcessSceneWithReport {
        public int callbackOrder { get; }
        
        public void OnProcessScene(Scene scene, BuildReport report) {
            StrippingMode strippingMode = report is null ? AdvancedHierarchySettings.PlayMode : AdvancedHierarchySettings.Build;

            foreach (HierarchyFolder folder in Object.FindObjectsByType<HierarchyFolder>(FindObjectsSortMode.None))
                folder.Flatten(strippingMode, AdvancedHierarchySettings.CapitalizeName);
        }
    }
}