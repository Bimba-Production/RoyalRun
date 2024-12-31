using UnityEngine;

namespace Assets._Scripts
{
    public class ChunkMover : MonoBehaviour
    {
        [SerializeField] private float _speed = 8f;

        public float Speed
        {
            get { return _speed; }
            set
            {
                if (value >= 0)
                {
                    _speed = value;
                }
            }

        }

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            transform.position = new Vector3(transform.position.x, transform.position.y,
                transform.position.z - _speed * Time.deltaTime);
        }
    }
}