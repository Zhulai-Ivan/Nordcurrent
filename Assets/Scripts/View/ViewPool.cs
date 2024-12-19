using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ModestTree;
using Providers;
using UnityEngine;
using Zenject;

namespace View
{
    public class ViewPool : IViewPool, IDisposable
    {
        private readonly Dictionary<string, Stack<BaseView>> _typePool = new Dictionary<string, Stack<BaseView>>();
        
        private readonly AddressablesProvider _addressablesProvider;
        private readonly DiContainer _container;

        [Inject]
        public ViewPool(AddressablesProvider addressablesProvider, DiContainer container)
        {
            _addressablesProvider = addressablesProvider;
            _container = container;
        }
        
        public async UniTask<T> Pop<T>(Vector3 position, Vector3 rotation, Vector3 scale, Transform parent) where T : BaseView
        {
            T view = null;
            if (_typePool.Keys.ContainsItem(typeof(T).Name) &&_typePool[typeof(T).Name].Count > 0)
            {
                view = _typePool[typeof(T).Name].Pop() as T;
                view.gameObject.transform.position = position;
                view.gameObject.SetActive(true);
            }
            else
            {
                var prefab = await _addressablesProvider.LoadAsync<GameObject>(typeof(T).Name);
                var instance = _container.InstantiatePrefabForComponent<T>(prefab, position, Quaternion.Euler(rotation), parent);
                instance.transform.localScale = scale;
                view = instance.GetComponent<T>();
                
                if(view == null)
                    throw new Exception($"Cannot instantiate view {typeof(T).Name}");
                
                
                if (!_typePool.ContainsKey(typeof(T).Name))
                {
                    _typePool.Add(typeof(T).Name, new Stack<BaseView>());
                }
            }
            
            return view != null ? view : null;
        }

        public void Push<T>(T view) where T : BaseView
        {
            view.gameObject.SetActive(false);
            _typePool[typeof(T).Name].Push(view);
        }

        public void Dispose()
        {
            _typePool.Clear();
        }
    }
}