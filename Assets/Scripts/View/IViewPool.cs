using Cysharp.Threading.Tasks;
using UnityEngine;

namespace View
{
    public interface IViewPool
    {
        UniTask<T> Pop<T>(Vector3 position, Transform parent) where T : BaseView;
        void Push<T>(T view) where T : BaseView;
    }
}