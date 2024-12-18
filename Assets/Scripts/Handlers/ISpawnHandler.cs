using Cysharp.Threading.Tasks;
using Level;

namespace Handlers
{
    public interface ISpawnHandler
    {
        UniTask Spawn();
        UniTask<SpawnPoint> FindSpawnPoint();
    }
}