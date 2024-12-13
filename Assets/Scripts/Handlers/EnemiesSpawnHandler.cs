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

        private void Awake()
        {
            for (int i = 0; i < 4; i++) 
                Spawn();
        }

        public async void Spawn()
        {
            var unlockedPoints = _spawnPoints.Where(point => !point.IsLocked).ToArray();

            if (unlockedPoints.Length == 0)
            {
                await UniTask.WaitUntil(() => _spawnPoints.Any(point => !point.IsLocked));
                unlockedPoints = _spawnPoints.Where(point => !point.IsLocked).ToArray();
            }
            
            Debug.Log($"Unlocked {unlockedPoints.Length} points");
            var index = Random.Range(0, unlockedPoints.Length);
            var position = unlockedPoints[index].transform.position;
            await _viewPool.Pop<Enemy>(position, transform);
            
            Debug.Log($"Spawned Enemy at index {index}");
        }
    }
}