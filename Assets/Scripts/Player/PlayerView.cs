using Modules;
using UnityEngine;

namespace Player
{
    public class PlayerView : MonoBehaviour
    {
        private BaseTrack _track;

        public float Speed => _track.Speed;

        public void SetTrack(BaseTrack track)
        {
            _track = track;
        }
    }
}