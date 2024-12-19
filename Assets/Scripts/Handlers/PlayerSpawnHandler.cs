using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Level;
using Level.Data;
using Modules;
using Player;
using UnityEngine;
using View;
using Zenject;
using Random = UnityEngine.Random;

namespace Handlers
{
    public class PlayerSpawnHandler : ISpawnHandler
    {
        private PlayerView _player;
        private SpawnPoint[] _spawnPoints;
        private IViewPool _viewPool;

        [Inject]
        private void InstallBindings(SpawnPoint[] spawnPoints, IViewPool viewPool)
        {
            _spawnPoints = spawnPoints;
            _viewPool = viewPool;
        }

        private async void StartRespawn()
        {
            await UniTask.Delay(1000);
            
            if (_player != null) 
                _player.Controller.Dead -= StartRespawn;

            Spawn().Forget();
        }

        public async UniTask Spawn(ViewData view)
        {
            _player = await _viewPool.Pop<PlayerView>(view.position, view.rotation,
                view.scale, null);
            
            _player.SetTrack(GetTrack(view.trackType));
            
            _player.Controller.Dead += StartRespawn;
        }

        private BaseTrack GetTrack(ModuleType type)
        {
            switch (type)
            {
                case ModuleType.FastTrack:
                    return new FastTrack();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
        
        public async UniTask InitSpawn() => 
            await Spawn();

        public async UniTask Spawn()
        {
            var point = await FindSpawnPoint();
            var position = point.transform.position;
            
            _player = await _viewPool.Pop<PlayerView>(position, Vector3.one, Vector3.one, null);
            _player.Controller.Dead += StartRespawn;
        }

        public async UniTask<SpawnPoint> FindSpawnPoint()
        {
            var unlockedPoints = _spawnPoints.Where(point => !point.IsLocked).ToArray();

            if (unlockedPoints.Length <= 0)
            {
                await UniTask.WaitUntil(() => _spawnPoints.Any(point => !point.IsLocked));
                unlockedPoints = _spawnPoints.Where(point => !point.IsLocked).ToArray();
            }

            var index = Random.Range(0, unlockedPoints.Length);
            return unlockedPoints[index];
        }
    }
}