using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Level;
using Player;
using Zenject;
using Random = UnityEngine.Random;

namespace Handlers
{
    public class PlayerSpawnHandler : ISpawnHandler, IDisposable
    {
        private PlayerController _player;
        private SpawnPoint[] _spawnPoints;

        [Inject]
        private void InstallBindings(PlayerController player, SpawnPoint[] spawnPoints)
        {
            _player = player;
            _spawnPoints = spawnPoints;

            _player.Dead += StartSpawn;
        }

        private void StartSpawn()
        {
            Spawn().Forget();
        }
        
        public async UniTask Spawn()
        {
            _player.gameObject.SetActive(false); 
            
            await UniTask.Delay(1000);
            
            var point = await FindSpawnPoint();
            _player.gameObject.transform.position = point.transform.position;
            _player.gameObject.SetActive(true);
        }

        public async UniTask<SpawnPoint> FindSpawnPoint()
        {
            var unlockedPoints = _spawnPoints.Where(point => !point.IsLocked).ToArray();;

            if (unlockedPoints.Length <= 0)
            {
                await UniTask.WaitUntil(() => _spawnPoints.Any(point => !point.IsLocked));
                unlockedPoints = _spawnPoints.Where(point => !point.IsLocked).ToArray();;;
            }
            
            var index = Random.Range(0, unlockedPoints.Length);
            return unlockedPoints[index];
        }

        public void Dispose()
        {
            _player.Dead -= StartSpawn;
        }
    }
}