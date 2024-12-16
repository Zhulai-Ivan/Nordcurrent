using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Enemies;
using Level;
using States;
using UnityEngine;
using View;
using Zenject;
using Random = UnityEngine.Random;

namespace Handlers
{
    public class EnemiesSpawnHandler : MonoBehaviour, ISpawnHandler, IDisposable
    {
        [SerializeField] private SpawnPoint[] _spawnPoints;

        private IViewPool _viewPool;
        private LevelHandler _levelHandler;

        private List<Enemy> _enemies = new();

        [Inject]
        private void InstallBindings(IViewPool viewPool, LevelHandler levelHandler)
        {
            _viewPool = viewPool;
            _levelHandler = levelHandler;
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
            var view = await _viewPool.Pop<Enemy>(position, transform);
            _enemies.Add(view);

            SetMoveState(view);
        }

        private void SetMoveState(Enemy view)
        {
            view.Move(_levelHandler.XLimits, _levelHandler.YLimits);
        }


        public void Dispose()
        {
            foreach (var enemy in _enemies)
            {
                enemy.Push();
            }

            _enemies.Clear();
        }
    }
}