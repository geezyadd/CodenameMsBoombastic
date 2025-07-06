namespace General.Modules.ObjectPools.Runtime {
    public interface IObjectsPool<TObject> {
        public abstract TObject Get();
        public abstract void Release(TObject instance);
    }
}