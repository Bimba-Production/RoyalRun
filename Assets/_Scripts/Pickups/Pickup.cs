using UnityEngine;

namespace _Scripts.Pickups
{
    public abstract class Pickup : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed;

        private void Update()
        {
            float rotationAngle = _rotationSpeed * Time.deltaTime;
            transform.Rotate(0, rotationAngle, 0);
        }

        public abstract void OnPickup();
    }
}