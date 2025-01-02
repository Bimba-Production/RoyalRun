using UnityEngine;

namespace _Scripts.Obstacles
{
    public class ObstacleDestroyer : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float _minZ = -30f;

        private void FixedUpdate()
        {
            if (transform.position.z <= _minZ) Destroy(transform.gameObject);
        }
    }
}