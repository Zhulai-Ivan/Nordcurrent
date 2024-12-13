using Handlers;
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
                .AsSingle();
        }
    }
}