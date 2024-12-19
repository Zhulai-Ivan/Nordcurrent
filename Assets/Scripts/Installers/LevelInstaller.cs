using Handlers;
using Level;
using Player;
using Player.Input;
using Providers;
using UnityEngine;
using View;
using Zenject;

namespace Installers
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField] private InputManager _inputManager;
        [SerializeField] private LevelHandler _levelHandler;
        [SerializeField] private SpawnPoint[] _spawnPoints;
        [SerializeField] private SceneDataCollector _sceneDataCollector;

        public override void InstallBindings()
        {
            Container.Bind<InputManager>()
                .FromInstance(_inputManager)
                .AsSingle()
                .NonLazy();

            Container.Bind<AddressablesProvider>()
                .AsSingle();
            
            Container.BindInterfacesTo<ViewPool>()
                .AsSingle()
                .NonLazy();
            
            Container.BindInterfacesAndSelfTo<EnemiesSpawnHandler>()
                .AsSingle()
                .WithArguments(_spawnPoints);
            
            Container.BindInterfacesAndSelfTo<PlayerSpawnHandler>()
                .AsSingle()
                .WithArguments(_spawnPoints);

            Container.Bind<LevelHandler>()
                .FromInstance(_levelHandler)
                .AsSingle();
            
            Container.BindInterfacesAndSelfTo<LevelLoader>()
                .AsSingle()
                .NonLazy();

            Container.Bind<SceneDataCollector>()
                .FromInstance(_sceneDataCollector);

            Container.BindInterfacesAndSelfTo<LevelSaver>()
                .AsSingle();
        }
    }
}