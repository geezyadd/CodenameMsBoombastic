using System;
using System.Collections.Generic;

namespace DependenciesProvider {
    public static class ZenjectDependenciesProvider {
        private static readonly Dictionary<Type, object> Dependencies = new();

        public static void Register<TObject>(TObject instance) {
            Dependencies[typeof(TObject)] = instance;
        }

        public static void Unregister<TObject>() {
            Type type = typeof(TObject);
            if (Dependencies.ContainsKey(type))
                Dependencies.Remove(type);
        }

        public static TObject Get<TObject>() {
            if (Dependencies.TryGetValue(typeof(TObject), out object dependency))
                return (TObject)dependency;

            return default;
        }

        public static void Cleanup() =>
            Dependencies.Clear();
    }
}
