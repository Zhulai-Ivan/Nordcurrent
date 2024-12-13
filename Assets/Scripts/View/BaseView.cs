using UnityEngine;
using Zenject;

namespace View
{
    public abstract class BaseView : MonoBehaviour
    {
        protected IViewPool ViewPool;

        [Inject]
        private void InstallBindings(IViewPool viewPool)
        {
            ViewPool = viewPool;
        }

        public abstract void Push();
    }
}