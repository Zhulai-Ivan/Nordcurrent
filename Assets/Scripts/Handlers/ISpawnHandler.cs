using Cysharp.Threading.Tasks;
using Level;
using Level.Data;
using View;

namespace Handlers
{
    public interface ISpawnHandler
    {
        UniTask InitSpawn();
        UniTask Spawn();
        UniTask Spawn(ViewData view);
        UniTask<SpawnPoint> FindSpawnPoint();
    }
}