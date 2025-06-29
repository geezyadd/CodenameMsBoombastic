using UnityEngine;

namespace AdvancedHierarchy.Editor.HierarchyFolders {
    internal class FolderIconsHolder {
        internal Texture2D OpenFolder { get; }
        internal Texture2D ClosedFolder { get; }

        internal FolderIconsHolder(Texture2D openFolder, Texture2D closedFolder) {
            OpenFolder = openFolder;
            ClosedFolder = closedFolder;
        }
    }
}