using UnityEngine;
using View;
using Zenject;

namespace Enemy
{
    public abstract class BaseView : MonoBehaviour
    {
        private ViewPool _viewPool;

        [Inject]
        private void InstallBindings(ViewPool viewPool)
        {
            _viewPool = viewPool;
        }

        public virtual void Push()
        {
            _viewPool.Push(this);
        }
    }
}