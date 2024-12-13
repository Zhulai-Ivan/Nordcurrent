using System.Linq;
using Cysharp.Threading.Tasks;
using Enemies;
using Level;
using UnityEngine;
using View;
using Zenject;

namespace Handlers
{
    public class EnemiesSpawnHandler : MonoBehaviour, ISpawnHandler
    {
        [SerializeField] private SpawnPoint[] _spawnPoints;

        private IViewPool _viewPool; 

        [Inject]
        private void InstallBindings(IViewPool viewPool)
        {
            _viewPool = viewPool;
        }

        private async void Awake()
        {
            for (int i = 0; i < 3; i++) 
                await Spawn();
        }

        public async UniTask Spawn()
        {
            var unlockedPoints = _spawnPoints.Where(point => !point.IsLocked).ToArray();

            if (unlockedPoints.Length == 0)
            {
                await UniTask.WaitUntil(() => _spawnPoints.Any(point => !point.IsLocked));
                unlockedPoints = _spawnPoints.Where(point => !point.IsLocked).ToArray();
            }
            
            var index = Random.Range(0, unlockedPoints.Length);
            var position = unlockedPoints[index].transform.position;
            await _viewPool.Pop<Enemy>(position, transform);
        }
    }
}