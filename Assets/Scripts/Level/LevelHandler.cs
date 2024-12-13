using UnityEngine;

namespace Level
{
    public class LevelHandler : MonoBehaviour
    {
        [SerializeField] private Vector2 _levelLimits;
        
        public Vector2 XLimits => new Vector2(-_levelLimits.x, _levelLimits.x);
        public Vector2 YLimits => new Vector2(-_levelLimits.y, _levelLimits.y);
    }
}