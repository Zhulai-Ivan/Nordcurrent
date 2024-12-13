using System;
using UnityEngine;

namespace Level
{
    public class SpawnPoint : MonoBehaviour
    {
        public bool IsLocked =>
            Physics2D.OverlapCircle(transform.position, 2f, LayerMask.GetMask("Spawnable")) != null;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, 2f);
        }
#endif
    }
}