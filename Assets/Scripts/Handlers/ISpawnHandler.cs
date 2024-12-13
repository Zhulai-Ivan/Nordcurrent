using Cysharp.Threading.Tasks;
using UnityEngine;
using View;

namespace Handlers
{
    public interface ISpawnHandler
    {
        UniTask Spawn();
    }
}