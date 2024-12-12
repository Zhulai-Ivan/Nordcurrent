using UnityEngine;
using View;

namespace Handlers
{
    public class EnemiesSpawnHandler : MonoBehaviour
    {
        [SerializeField] private Enemies.Enemy _enemyPrefab;
        [SerializeField] private Transform _parent;

        private ViewPool _viewPool;
        
        
    }
}