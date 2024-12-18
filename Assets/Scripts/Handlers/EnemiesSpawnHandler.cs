using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Enemies;
using Level;
using Modules;
using View;
using Zenject;
using Random = UnityEngine.Random;

namespace Handlers
{
    public class EnemiesSpawnHandler : ISpawnHandler, IDisposable
    {
        private IViewPool _viewPool;
        private LevelHandler _levelHandler;

        private List<Enemy> _enemies = new();
        private SpawnPoint[] _spawnPoints;

        [Inject]
        private async void InstallBindings(IViewPool viewPool, LevelHandler levelHandler, SpawnPoint[] spawnPoints)
        {
            _viewPool = viewPool;
            _levelHandler = levelHandler;
            _spawnPoints = spawnPoints;
            
            for (int i = 0; i < 3; i++)
                await Spawn();
        }

        public async UniTask Spawn()
        {
            var point = await FindSpawnPoint();
            var position = point.transform.position;
            var view = await _viewPool.Pop<Enemy>(position, null);
            
            view.ReadyToSpawn += OnReadyToSpawn;
            view.Pushed += OnViewPushed;
            
            SetModules(view);
            
            _enemies.Add(view);

            SetMoveState(view);
        }

        public async UniTask<SpawnPoint> FindSpawnPoint()
        {
            var unlockedPoints = _spawnPoints.Where(point => !point.IsLocked).ToArray();;

            if (unlockedPoints.Length <= 0)
            {
                await UniTask.WaitUntil(() => _spawnPoints.Any(point => !point.IsLocked));
                unlockedPoints = _spawnPoints.Where(point => !point.IsLocked).ToArray();
            }
            
            var index = Random.Range(0, unlockedPoints.Length);
            return unlockedPoints[index];
        }

        private void SetModules(Enemy view)
        {
            view.SetTrack(new FastTrack());
        }

        private void OnViewPushed(Enemy view)
        {
            view.Pushed -= OnViewPushed;
            _enemies.Remove(view);
        }

        private void OnReadyToSpawn(Enemy view)
        {
            view.ReadyToSpawn -= OnReadyToSpawn;
            Spawn().Forget();
        }

        private void SetMoveState(Enemy view)
        {
            view.SetPositionLimits(_levelHandler.XLimits, _levelHandler.YLimits);
            view.Move();
        }


        public void Dispose()
        {
            foreach (var enemy in _enemies)
            {
                enemy.ReadyToSpawn -= OnReadyToSpawn;
                enemy.Pushed -= OnViewPushed;
                
                if(enemy.gameObject != null)
                    enemy.Push();
            }

            _enemies.Clear();
        }
    }
}