using UnityEditor;
using UnityEngine;

namespace DependenciesProvider.Editor {
    internal static class ZenjectDependenciesProviderCleanup {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Initialize() =>
            EditorApplication.playModeStateChanged += OnExitPlayMode;

        private static void OnExitPlayMode(PlayModeStateChange playModeStateChange) {
            if (playModeStateChange is not PlayModeStateChange.ExitingPlayMode)
                return;

            ZenjectDependenciesProvider.Cleanup();
            EditorApplication.playModeStateChanged -= OnExitPlayMode;
        }
    }
}
