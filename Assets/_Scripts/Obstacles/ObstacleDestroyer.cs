using UnityEngine;

namespace Assets._Scripts
{
    public class ObstacleDestroyer : MonoBehaviour
    {
        [SerializeField] float _minZ = -30f;

        private void FixedUpdate()
        {
            if (transform.position.z <= _minZ)
            {
                Destroy(transform.gameObject);
            }
        }
    }
}