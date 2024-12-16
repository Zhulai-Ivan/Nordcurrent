using Handlers;
using Level;
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
        [SerializeField] private EnemiesSpawnHandler _spawnHandler;

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
                .FromInstance(_spawnHandler)
                .AsSingle();

            Container.Bind<LevelHandler>()
                .FromInstance(_levelHandler)
                .AsSingle();
        }
    }
}