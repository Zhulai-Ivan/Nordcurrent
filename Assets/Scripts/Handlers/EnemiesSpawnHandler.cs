using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Enemies;
using Level;
using Level.Data;
using Modules;
using UnityEngine;
using View;
using Zenject;
using Random = UnityEngine.Random;

namespace Handlers
{
    public class EnemiesSpawnHandler : ISpawnHandler
    {
        private IViewPool _viewPool;
        private LevelHandler _levelHandler;
        
        private SpawnPoint[] _spawnPoints;

        [Inject]
        private void InstallBindings(IViewPool viewPool, LevelHandler levelHandler, SpawnPoint[] spawnPoints)
        {
            _viewPool = viewPool;
            _levelHandler = levelHandler;
            _spawnPoints = spawnPoints;
        }

        public async UniTask InitSpawn()
        {
            for (int i = 0; i < 3; i++)
            {
                await Spawn();
            }
        }

        public async UniTask Spawn()
        {
            var point = await FindSpawnPoint();
            var position = point.transform.position;
            var view = await _viewPool.Pop<Enemy>(position, Vector3.one, Vector3.one, null);
            
            view.SetTrack(new FastTrack());
            
            view.ReadyToSpawn += OnReadyToSpawn;
            view.Pushed += OnViewPushed;
            
            SetMoveState(view);
        }

        private BaseTrack GetTrack(ModuleType moduleType)
        {
            switch (moduleType)
            {
                case ModuleType.None:
                    return null;
                case ModuleType.FastTrack:
                    return new FastTrack();
                default:
                    throw new ArgumentOutOfRangeException(nameof(moduleType), moduleType, null);
            }
        }

        public async UniTask Spawn(ViewData tankView)
        {
            var view = await _viewPool.Pop<Enemy>(tankView.position, tankView.rotation,
                tankView.scale, null);
            
            view.SetTrack(GetTrack(tankView.trackType));
            view.ReadyToSpawn += OnReadyToSpawn;
            view.Pushed += OnViewPushed;

            SetMoveState(view);
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

        private void OnViewPushed(Enemy view)
        {
            view.Pushed -= OnViewPushed;
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
    }
}