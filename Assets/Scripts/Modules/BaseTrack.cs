namespace Modules
{
    public abstract class BaseTrack
    {
        public abstract ModuleType Type { get; }
        public abstract float Speed { get; protected set; }
    }
}