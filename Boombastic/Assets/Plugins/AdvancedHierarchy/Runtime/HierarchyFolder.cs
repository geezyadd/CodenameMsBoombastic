using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AdvancedHierarchy {
    [DisallowMultipleComponent, ExecuteAlways]
    public class HierarchyFolder : MonoBehaviour {
        #if UNITY_EDITOR
        private static bool _addedSelectionResetCallback;
        private static Tool _lastTool;
        private static HierarchyFolder _toolLock;
        private static readonly Dictionary<int, int> Folders = new();
        [SerializeField] private int _colorIndex;

        private HierarchyFolder() {
            if (!_addedSelectionResetCallback) {
                Selection.selectionChanged += () => Tools.hidden = false;
                _addedSelectionResetCallback = true;
            }

            Selection.selectionChanged += HandleSelection;
        }
        
        private void Start() =>
            AddFolderData();

        private void OnValidate() =>
            AddFolderData();

        private void OnDestroy() =>
            RemoveFolderData();

        public static bool TryGetIconIndex(Object obj, out int index) {
            index = -1;
            return obj && Folders.TryGetValue(obj.GetInstanceID(), out index);
        }
        
        private void AddFolderData() =>
            Folders[gameObject.GetInstanceID()] = _colorIndex;

        private void RemoveFolderData() =>
            Folders.Remove(gameObject.GetInstanceID());

        private void HandleSelection() {
            if (_toolLock != null && _toolLock != this)
                return;

            if (this != null && Selection.Contains(gameObject)) {
                _lastTool = Tools.current;
                _toolLock = this;
                Tools.current = Tool.None;
            }
            else if (_toolLock != null) {
                Tools.current = _lastTool;
                _toolLock = null;
            }
        }

        private bool AskDelete() =>
            EditorUtility.DisplayDialog("Can't add script", "Folders shouldn't be used with other components. Which component should be kept?", "Folder", "Component");

        private void DeleteComponents(IEnumerable<Component> comps) {
            IEnumerable<Component> destroyable = comps.Where(c => c != null && c.CanDestroy()).ToArray();

            while (destroyable.Any())
                foreach (Component c in destroyable)
                    DestroyImmediate(c);
        }

        private void EnsureExclusiveComponent() {
            if (Application.isPlaying || this == null)
                return;

            IEnumerable<Component> existingComponents = GetComponents<Component>().Where(component => component != this && component is not Transform).ToArray();
            if (!existingComponents.Any())
                return;

            if (AskDelete())
                DeleteComponents(existingComponents);
            else
                DestroyImmediate(this);
        }
        #endif
        
        private void OnEnable() =>
            transform.hideFlags = HideFlags.HideInInspector;
        
        private void Update() {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;

            #if UNITY_EDITOR
            if (!Application.IsPlaying(gameObject))
                AddFolderData();

            EnsureExclusiveComponent();
            #endif
        }
        
        public void Flatten(StrippingMode strippingMode, bool capitalizeFolderName) {
            if (strippingMode == StrippingMode.DoNothing)
                return;

            MoveChildrenOut(strippingMode);
            HandleSelf(strippingMode, capitalizeFolderName);
        }

        private void MoveChildrenOut(StrippingMode strippingMode) {
            int index = transform.GetSiblingIndex(); 

            foreach (Transform child in GetComponentsInChildren<Transform>(true)) {
                if (child.parent != transform)
                    continue;

                if (strippingMode == StrippingMode.PrependWithFolderName)
                    child.name = $"{name}/{child.name}";

                child.SetParent(transform.parent, true);
                child.SetSiblingIndex(++index);
            }
        }

        private void HandleSelf(StrippingMode strippingMode, bool capitalizeFolderName) {
            if (strippingMode == StrippingMode.ReplaceWithSeparator) {
                if (!name.StartsWith("--- "))
                    name = $"--- {(capitalizeFolderName ? name.ToUpper() : name)} ---";

                return;
            }

            if (Application.isPlaying)
                Destroy(gameObject);
            else
                DestroyImmediate(gameObject);
        }
    }
    
    #if UNITY_EDITOR
    internal static class CanDestroyExtension {
        internal static bool CanDestroy(this Component currentComponent) =>
            !currentComponent.gameObject.GetComponents<Component>().Any(component => Requires(component.GetType(), currentComponent.GetType()));

        private static bool Requires(Type obj, Type req) =>
            Attribute.IsDefined(obj, typeof(RequireComponent)) &&
            Attribute.GetCustomAttributes(obj, typeof(RequireComponent))
                     .OfType<RequireComponent>()
                     .SelectMany(rc => new[] {
                         rc.m_Type0, rc.m_Type1, rc.m_Type2
                     })
                     .Any(t => t != null && t.IsAssignableFrom(req));
    }
    #endif
}