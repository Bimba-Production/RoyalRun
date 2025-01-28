using UnityEngine;

namespace _Scripts.Obstacles
{
    public sealed class ObstacleDestroyer : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float _minZ = -30f;

        private void FixedUpdate()
        {
            if (transform.position.z <= _minZ) Destroy(gameObject);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag(nameof(Tags.Obstacle)) || other.gameObject.CompareTag(nameof(Tags.Player)) || other.gameObject.CompareTag(nameof(Tags.CriticalObstacle)))
            {
                DestructionEffectSpawner.Instance.Play(transform.position, new Vector3(0.7f, 0.7f, 0.7f));
                Destroy(gameObject);
            }
        }
    }
}