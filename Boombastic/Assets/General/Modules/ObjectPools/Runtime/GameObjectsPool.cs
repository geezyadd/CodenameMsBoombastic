using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace General.Modules.ObjectPools.Runtime {
    [CreateAssetMenu(fileName = nameof(GameObjectsPool) + "_Default", menuName = "Game Object Pools/Default Game Object Pool")]
    public class GameObjectsPool : ScriptableObject, IObjectsPool<GameObject> {
        private readonly Stack<GameObject> _pool = new();
        private readonly LinkedList<GameObject> _active = new();
        private readonly Dictionary<GameObject, LinkedListNode<GameObject>> _activeMap = new();
        
        [SerializeField] private GameObject _prefab;
        [SerializeField, Min(0)] private int _min;
        [SerializeField] private int _max;
        [SerializeField] private bool _cyclic;

        private void OnValidate() {
            int minValue = _min == 0 ? 1 : _min;
            if (_max < minValue)
                _max = minValue;
        }

        private GameObject Create() {
            Assert.IsNotNull(_prefab, PoolErrorMessages.EmptyPrefabError);

            if (_active.Count < _max)
                return Instantiate(_prefab);

            if (_cyclic && _active.First != null) {
                GameObject oldest = _active.First.Value;
                _active.RemoveFirst();
                _activeMap.Remove(oldest);
                OnRelease(oldest);
                return oldest;
            }

            Debug.LogWarning(PoolErrorMessages.ReachedMaxCount);
            return null;
        }

        public void Prewarm() {
            int count = Mathf.Min(_min, _max);
            for (int i = 0; i < count; i++) {
                GameObject instance = Create();
                if (instance == null) break;
                OnRelease(instance);
                _pool.Push(instance);
            }
        }

        public GameObject Get() {
            GameObject instance = _pool.Count > 0 ? _pool.Pop() : Create();
            if (instance == null)
                return null;

            OnReset(instance);

            LinkedListNode<GameObject> node = _active.AddLast(instance);
            _activeMap[instance] = node;

            return instance;
        }

        public void Release(GameObject instance) {
            Assert.IsNotNull(instance, PoolErrorMessages.EmptyInstance);

            if (_pool.Contains(instance)) {
                Debug.LogWarning(PoolErrorMessages.DoubleRelease);
                return;
            }

            if (_activeMap.TryGetValue(instance, out LinkedListNode<GameObject> node)) {
                _active.Remove(node);
                _activeMap.Remove(instance);
            }

            OnRelease(instance);
            _pool.Push(instance);
        }

        protected virtual void OnReset(GameObject instance) =>
            instance.SetActive(true);

        protected virtual void OnRelease(GameObject instance) =>
            instance.SetActive(false);
    }
}