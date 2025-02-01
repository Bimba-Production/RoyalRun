using UnityEngine;

namespace _Scripts
{
    public class RockCollisionHandler: MonoBehaviour
    {
        private void OnCollision(Collider other)
        {
            if (other.CompareTag(nameof(Tags.Obstacle)) || other.CompareTag(nameof(Tags.CriticalObstacle)))
            {
                Collider otherCollider = other.GetComponent<Collider>()!;
                
                DestructionObstacleSpawner.Instance.Play(otherCollider.transform.position, Vector3.zero);
                Destroy(otherCollider.gameObject);
            }
        }
        
        private void OnCollision(Collision other)
        {
            if (other.transform.CompareTag(nameof(Tags.Obstacle)) || other.transform.CompareTag(nameof(Tags.CriticalObstacle)))
            {
                Collider otherCollider = other.transform.GetComponent<Collider>()!;
                
                DestructionObstacleSpawner.Instance.Play(otherCollider.transform.position, Vector3.zero);
                Destroy(otherCollider.gameObject);
            }
        }
        
        private void OnCollisionEnter(Collision other) => OnCollision(other);
        private void OnTriggerEnter(Collider other) => OnCollision(other);
    }
}