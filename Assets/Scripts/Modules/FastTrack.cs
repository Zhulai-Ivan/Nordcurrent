namespace Modules
{
    public class FastTrack : BaseTrack
    {
        private float _speed = 10f;
        
        public override float Speed
        {
            get => _speed;
            protected set // in future can be set from json config in future
            {
                if(_speed != value)
                    _speed = value;
            }
        }
    }
}