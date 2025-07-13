using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AdvancedHierarchy.Editor.HierarchyFolders.TextureUtils;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using Assembly = System.Reflection.Assembly;
using Object = UnityEngine.Object;

namespace AdvancedHierarchy.Editor.HierarchyFolders {
    public static class HierarchyFolderIcon {
        private const string OPENED_FOLDER_PREFIX = "FolderOpened";
        private const string CLOSED_FOLDER_PREFIX = "Folder";
        private const string PREFAB_MARK_TEXT = "Folder Prefab";

        private static Texture2D _openFolderTexture;
        private static Texture2D _closedFolderTexture;

        private static bool _isInitialized;

        private static PropertyInfo _sceneHierarchyProperty;
        private static PropertyInfo _treeViewProperty;
        private static PropertyInfo _dataProperty;
        private static PropertyInfo _objectProperty;

        private static MethodInfo _getRowsMethodInfo;
        private static MethodInfo _isExpandedMethodInfo;
        private static MethodInfo _getAllSceneHierarchyMethodInfo;

        private static FolderIconsHolder[] _coloredFolderIcons;

        internal static FolderIconsHolder ColoredFolderIcons(int i) =>
            _coloredFolderIcons[i];

        public static int IconColumnCount =>
            IconColors.GetLength(0);
        public static int IconRowCount =>
            IconColors.GetLength(1);

        private static readonly Color[,] IconColors = {
            {
                new(0.09f, 0.57f, 0.82f), new(0.05f, 0.34f, 0.48f)
            }, {
                new(0.09f, 0.67f, 0.67f), new(0.05f, 0.42f, 0.42f)
            }, {
                new(0.23f, 0.73f, 0.36f), new(0.15f, 0.41f, 0.22f)
            }, {
                new(0.55f, 0.35f, 0.71f), new(0.35f, 0.24f, 0.44f)
            }, {
                new(0.78f, 0.27f, 0.55f), new(0.52f, 0.15f, 0.35f)
            }, {
                new(0.80f, 0.66f, 0.10f), new(0.56f, 0.46f, 0.02f)
            }, {
                new(0.91f, 0.49f, 0.13f), new(0.62f, 0.33f, 0.07f)
            }, {
                new(0.91f, 0.30f, 0.24f), new(0.77f, 0.15f, 0.09f)
            }, {
                new(0.35f, 0.49f, 0.63f), new(0.24f, 0.33f, 0.42f)
            }
        };

        [InitializeOnLoadMethod]
        private static void Startup() {
            EditorApplication.update += ResetFolderIcons;
            EditorApplication.hierarchyWindowItemOnGUI += RefreshFolderIcons;
            EditorApplication.hierarchyWindowItemOnGUI += MarkPrefabs;
        }

        private static void Initialize() {
            _openFolderTexture = (Texture2D)EditorGUIUtility.IconContent($"{OPENED_FOLDER_PREFIX} Icon").image;
            _closedFolderTexture = (Texture2D)EditorGUIUtility.IconContent($"{CLOSED_FOLDER_PREFIX} Icon").image;
            
            _openFolderTexture = TextureHelper.GetWhiteTexture(_openFolderTexture, $"{OPENED_FOLDER_PREFIX} Icon White");
            _closedFolderTexture = TextureHelper.GetWhiteTexture(_closedFolderTexture, $"{CLOSED_FOLDER_PREFIX} Icon White");

            _coloredFolderIcons = new FolderIconsHolder[] { new (_openFolderTexture, _closedFolderTexture) };

            for (int row = 0; row < IconRowCount; row++) {
                for (int column = 0; column < IconColumnCount; column++) {
                    int index = 1 + column + row * IconColumnCount;
                    Color color = IconColors[column, row];

                    Texture2D openFolderIcon = TextureHelper.GetTintedTexture(_openFolderTexture, color, $"{_openFolderTexture.name} {index}");
                    Texture2D closedFolderIcon = TextureHelper.GetTintedTexture(_closedFolderTexture, color, $"{_closedFolderTexture.name} {index}");

                    ArrayUtility.Add(ref _coloredFolderIcons, new FolderIconsHolder(openFolderIcon, closedFolderIcon));
                }
            }

            const BindingFlags bindingAll = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

            Assembly assembly = typeof(SceneView).Assembly;

            Type sceneHierarchyWindowType = assembly.GetType("UnityEditor.SceneHierarchyWindow");
            _getAllSceneHierarchyMethodInfo = sceneHierarchyWindowType.GetMethod("GetAllSceneHierarchyWindows", bindingAll);
            _sceneHierarchyProperty = sceneHierarchyWindowType.GetProperty("sceneHierarchy");

            Type sceneHierarchyType = assembly.GetType("UnityEditor.SceneHierarchy");
            _treeViewProperty = sceneHierarchyType.GetProperty("treeView", bindingAll);

            Type treeViewControllerType = assembly.GetType("UnityEditor.IMGUI.Controls.TreeViewController");
            _dataProperty = treeViewControllerType.GetProperty("data", bindingAll);

            Type iTreeViewDataSourceType = assembly.GetType("UnityEditor.IMGUI.Controls.ITreeViewDataSource");
            _getRowsMethodInfo = iTreeViewDataSourceType.GetMethod("GetRows");
            _isExpandedMethodInfo = iTreeViewDataSourceType.GetMethod("IsExpanded", new[] { typeof(TreeViewItem) });

            Type gameObjectTreeViewItemType = assembly.GetType("UnityEditor.GameObjectTreeViewItem");
            _objectProperty = gameObjectTreeViewItemType.GetProperty("objectPPTR", bindingAll);

            _isInitialized = true;
        }

        private static void ResetFolderIcons() {
            if(_isInitialized is false)
                Initialize();
        }

        private static void RefreshFolderIcons(int instanceID, Rect selectionRect) {
            if (_getAllSceneHierarchyMethodInfo is null)
                return;
            
            IEnumerable<EditorWindow> windows = ((IEnumerable)_getAllSceneHierarchyMethodInfo.Invoke(null, Array.Empty<object>())).Cast<EditorWindow>();
            foreach (EditorWindow window in windows) {
                object sceneHierarchy = _sceneHierarchyProperty.GetValue(window);
                object treeView = _treeViewProperty.GetValue(sceneHierarchy);
                object data = _dataProperty.GetValue(treeView);

                IList<TreeViewItem> rows = (IList<TreeViewItem>)_getRowsMethodInfo.Invoke(data, Array.Empty<object>());
                foreach (TreeViewItem item in rows) { 
                    Object itemObject = (Object)_objectProperty.GetValue(item);
                    if (!HierarchyFolder.TryGetIconIndex(itemObject, out int colorIndex))
                        continue;

                    if (IsPrefabRoot((GameObject)itemObject))
                        continue;

                    bool isExpanded = (bool)_isExpandedMethodInfo.Invoke(data, new object[] { item });
                    FolderIconsHolder folderIconsHolder = ColoredFolderIcons(Mathf.Clamp(colorIndex, 0, _coloredFolderIcons.Length - 1));
                    item.icon = isExpanded ? folderIconsHolder.OpenFolder : folderIconsHolder.ClosedFolder;
                }
            }
        }

        private static void MarkPrefabs(int instanceID, Rect selectionRect) {
            if (EditorUtility.InstanceIDToObject(instanceID) is not GameObject gameObject)
                return;
            
            if (gameObject.TryGetComponent(out HierarchyFolder _) is false)
                return;
            
            if (IsPrefabRoot(gameObject) is false)
                return;
            
            float textWidth = EditorStyles.label.CalcSize(new GUIContent(PREFAB_MARK_TEXT)).x;
            Rect rect = new(selectionRect.xMax - textWidth - 2f, selectionRect.yMin, textWidth, selectionRect.height);
            GUI.Label(rect, PREFAB_MARK_TEXT, EditorStyles.label);
        }

        private static bool IsPrefabRoot(GameObject gameObject) =>
            PrefabUtility.GetPrefabAssetType(gameObject) != PrefabAssetType.NotAPrefab && PrefabUtility.IsAnyPrefabInstanceRoot(gameObject);
    }
}