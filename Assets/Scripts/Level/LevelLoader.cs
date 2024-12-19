using System;
using System.IO;
using System.Linq;
using Cysharp.Threading.Tasks;
using Enemies;
using Handlers;
using Level.Data;
using Modules;
using UnityEngine;
using Zenject;

namespace Level
{
    public class LevelLoader : ILoader
    {
        private ISpawnHandler _playerSpawnHandler;
        private ISpawnHandler _enemiesSpawnHandler;

        [Inject]
        private async void InstallBindings(ISpawnHandler[] spawnHandlers)
        {
            _playerSpawnHandler = spawnHandlers.First(handler => handler is PlayerSpawnHandler);
            _enemiesSpawnHandler = spawnHandlers.First(handler => handler is EnemiesSpawnHandler);
            
            var isLoaded = await Load();
            
            if (!isLoaded)
            {
                foreach (var spawnHandler in spawnHandlers)
                {
                    await spawnHandler.InitSpawn();
                }
            }
        }

        public async UniTask<bool> Load()
        {
            if (!File.Exists(Const.LoadLevelDataPath))
                return false;

            var json = File.ReadAllText(Const.LoadLevelDataPath);
            var levelData = JsonUtility.FromJson<LevelData>(json);

            if (levelData.views == null || levelData.views.Length == 0)
                return false;

            foreach (var view in levelData.views)
            {
                if (view.name == "Player")
                    await _playerSpawnHandler.Spawn(view);
                else
                    await _enemiesSpawnHandler.Spawn(view);
            }

            return true;
        }

        private BaseTrack GetTrack(ModuleType trackType)
        {
            switch (trackType)
            {
                case ModuleType.FastTrack:
                    return new FastTrack();
                default:
                    throw new ArgumentOutOfRangeException(nameof(trackType), trackType, null);
            }
        }
    }

    public interface ILoader
    {
        UniTask<bool> Load();
    }
}