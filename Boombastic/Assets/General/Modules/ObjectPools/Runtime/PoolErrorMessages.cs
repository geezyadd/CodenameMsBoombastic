namespace General.Modules.ObjectPools.Runtime {
    internal static class PoolErrorMessages {
        public static readonly string EmptyPrefabError = "Pool prefab can`t be null";
        public static readonly string EmptyInstance = "Instance can`t be null to Release";
        public static readonly string DoubleRelease = "Attempt to release an object already in the pool";
        public static readonly string ReachedMaxCount = "Max limit reached and cyclic disabled";
    }
}