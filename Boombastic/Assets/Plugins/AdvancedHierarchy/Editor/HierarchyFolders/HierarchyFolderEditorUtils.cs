using UnityEditor;
using UnityEngine;

namespace AdvancedHierarchy.Editor.HierarchyFolders {
    public static class HierarchyFolderEditorUtils {
        private const string ACTION_NAME = "Create Hierarchy Folder %#&N";
        
        [MenuItem("GameObject/" + ACTION_NAME, false, 0)]
        public static void AddFolderPrefab(MenuCommand command) {
            GameObject folderInstance = new("Folder");
            folderInstance.AddComponent<HierarchyFolder>();

            GameObjectUtility.SetParentAndAlign(folderInstance, (GameObject)command.context);
            Undo.RegisterCreatedObjectUndo(folderInstance, ACTION_NAME);
            Selection.activeObject = folderInstance;
        }
    }
}