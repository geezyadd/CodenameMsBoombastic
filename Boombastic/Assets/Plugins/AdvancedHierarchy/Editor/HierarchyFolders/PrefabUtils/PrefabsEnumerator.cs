using System.Collections;
using System.Collections.Generic;

namespace AdvancedHierarchy.Editor.HierarchyFolders.PrefabUtils {
    internal struct PrefabsEnumerator : IEnumerator<(string, string)> {
        private readonly ChangedPrefabs _instance;
        private int _index;

        public PrefabsEnumerator(ChangedPrefabs instance) {
            _instance = instance;
            _index = -1;
        }

        public bool MoveNext() =>
            ++_index < _instance.GUIDs.Length;

        public void Reset() =>
            _index = 0;

        public (string, string) Current =>
            _instance[_index];

        object IEnumerator.Current =>
            Current;

        public void Dispose() { }
    }
}