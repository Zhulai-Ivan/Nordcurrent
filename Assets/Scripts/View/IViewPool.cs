using Cysharp.Threading.Tasks;
using UnityEngine;

namespace View
{
    public interface IViewPool
    {
        UniTask<T> Pop<T>(Vector3 position, Vector3 rotation, Vector3 scale, Transform parent) where T : BaseView;
        void Push<T>(T view) where T : BaseView;
    }
}