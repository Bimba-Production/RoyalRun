using UnityEngine;

namespace _Scripts.Chunk
{
    public class ChunkMover : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _speed = 8f;

        public float Speed
        {
            get => _speed;
            set
            {
                if (value >= 0) _speed = value;
            }
        }

        private void Update() => Move();

        private void Move()
        {
            transform.position = new Vector3(transform.position.x, transform.position.y,
                transform.position.z - _speed * Time.deltaTime);
        }
    }
}