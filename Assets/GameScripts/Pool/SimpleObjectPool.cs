using System.Collections.Generic;
using UnityEngine;

namespace GameScripts.Pool
{
    public class SimpleObjectPool : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private int _initialObjectCount;

        private Stack<GameObject> _activeObjects;
        private Stack<GameObject> _disabledObjects;
        private Transform _poolObjectsContainer;

        private void Awake()
        {
            var container = new GameObject($"[Pool] {_prefab.name}");
            _poolObjectsContainer = container.transform;
            
            _activeObjects = new Stack<GameObject>(_initialObjectCount);
            _disabledObjects = new Stack<GameObject>(_initialObjectCount);
            for (var i = 0; i < _initialObjectCount; i++)
            {
                InstantiateNewObject();
            }
        }

        public GameObject Spawn()
        {
            if (_disabledObjects.Count == 0)
                InstantiateNewObject();
            var gameObject = _disabledObjects.Pop();
            _activeObjects.Push(gameObject);
            gameObject.SetActive(true);
            return gameObject;
        }

        public void DespawnLastActive()
        {
            if (_activeObjects.Count == 0)
                return;
            var go = _activeObjects.Pop();
            go.GetComponent<IPoolable>()?.ResetState();
            go.transform.SetParent(_poolObjectsContainer);
            go.SetActive(false);
            _disabledObjects.Push(go);
        }

        public void DespawnAll()
        {
            while (_activeObjects.Count > 0)
            {
                DespawnLastActive();
            }
        }

        private void InstantiateNewObject()
        {
            var gameObj = Instantiate(_prefab, _poolObjectsContainer);
            _disabledObjects.Push(gameObj);
            gameObj.SetActive(false);
        }
    }
}