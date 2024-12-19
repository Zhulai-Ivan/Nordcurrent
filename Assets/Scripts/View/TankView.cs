using Modules;

namespace View
{
    public abstract class TankView : BaseView
    {
        public abstract string Name { get; }
        public abstract override void Push();
        public abstract ModuleType TrackType { get; }
        public abstract void SetTrack(BaseTrack track);
    }
}