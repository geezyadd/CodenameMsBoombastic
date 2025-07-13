using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AdvancedHierarchy.Editor.HierarchyFolders.PrefabUtils {
    [Serializable]
    internal class ChangedPrefabs : IEnumerable<ValueTuple<string, string>> {
        private const string KEY_NAME = nameof(ChangedPrefabs);
        private static ChangedPrefabs _instance;
        public static ChangedPrefabs Instance =>
            _instance ??= FromDeserialized();
        
        [SerializeField] private string[] _guids;
        [SerializeField] private string[] _contents;
        
        public string[] GUIDs =>
            _guids;
        public string[] Contents =>
            _contents;

        public (string guid, string content) this[int index] {
            get =>
                (_guids[index], _contents[index]);
            set {
                _guids[index] = value.guid;
                _contents[index] = value.content;
            }
        }

        public static void Initialize(int length) {
            _instance = new ChangedPrefabs {
                _guids = new string[length], _contents = new string[length]
            };
        }

        public static void SerializeIfNeeded() {
            if (EditorSettings.enterPlayModeOptionsEnabled && EditorSettings.enterPlayModeOptions.HasFlag(EnterPlayModeOptions.DisableDomainReload))
                return;

            string serializedObject = EditorJsonUtility.ToJson(Instance);
            PlayerPrefs.SetString(KEY_NAME, serializedObject);
        }

        private static ChangedPrefabs FromDeserialized() {
            string serializedObject = PlayerPrefs.GetString(KEY_NAME);
            PlayerPrefs.DeleteKey(KEY_NAME);
            ChangedPrefabs instance = new();
            EditorJsonUtility.FromJsonOverwrite(serializedObject, instance);
            return instance;
        }
        
        IEnumerator<(string, string)> IEnumerable<ValueTuple<string, string>>.GetEnumerator() =>
            GetEnumerator(); 

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();

        public PrefabsEnumerator GetEnumerator() =>
            new(this);
    }
}