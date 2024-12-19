using System;
using Modules;
using View;
using Zenject;

namespace Player
{
    public class PlayerView : TankView
    {
        private BaseTrack _track;
        private PlayerController _controller;
        private IViewPool _viewPool;

        public float Speed => _track.Speed;
        public override ModuleType TrackType => _track.Type;
        public override string Name => "Player";

        public event Action Pushed;
        
        public PlayerController Controller => _controller;

        public void Setup(PlayerController controller)
        {
            _controller = controller;
        }

        [Inject]
        private void InstallBindings(IViewPool viewPool)
        {
            _viewPool = viewPool;
        }
        
        public override void SetTrack(BaseTrack track)
        {
            _track = track;
        }
        public override void Push()
        {
            _viewPool.Push(this);
            Pushed?.Invoke();
        }
    }
}