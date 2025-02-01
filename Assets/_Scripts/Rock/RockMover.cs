using UnityEngine;

namespace _Scripts
{
    public class RockMover: MonoBehaviour
    {
        [SerializeField] private float _speed;

        public bool IsGameOver { get; set; } = false;
        private void Update()
        {
            transform.Rotate(new Vector3(_speed, 0f, 0f));
            if (IsGameOver) transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + 8f * Time.deltaTime);
        }
    }
}