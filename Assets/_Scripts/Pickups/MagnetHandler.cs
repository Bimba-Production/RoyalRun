using UnityEngine;

namespace _Scripts.Pickups
{
    public sealed class MagnetHandler : StaticInstance<MagnetHandler>
    {
        [Header("References")] 
        [SerializeField] private Transform _playerTransform;

        [Header("Settings")]
        [SerializeField] private float _magnetingTime = 5f;
        [SerializeField] private float _magnetRadius = 5f; 
        [SerializeField] private float _attractionSpeed = 10f;
        [SerializeField] private float _rotationSpeed = 200f; 
        [SerializeField] private float _cooldown = 0.5f;

        private readonly Collider[] _results = new Collider[10]; 
        private float _nextCheck; 
        private bool _isMagneting;
        private float _magnetingEndTime;

        public void EnableMagneting()
        {
            _isMagneting = true;
            _magnetingEndTime = Time.time + _magnetingTime;
        }

        public void DisableMagneting()
        {
            _isMagneting = false;
        }

        private void FixedUpdate()
        {
            if (!_isMagneting || Time.time > _magnetingEndTime)
            {
                DisableMagneting();
                return;
            }

            if (GameController.Instance.IsGameOver) return;

            if (Time.time > _nextCheck)
            {
                CheckForPickups();
                _nextCheck = Time.time + _cooldown; 
            }
        }

        private void CheckForPickups()
        {
            int numColliders = Physics.OverlapSphereNonAlloc(_playerTransform.position, _magnetRadius, _results);

            for (int i = 0; i < numColliders; i++)
            {
                Collider pickup = _results[i];

                if (!pickup || pickup.CompareTag(nameof(Tags.Obstacle)) 
                            || pickup.CompareTag(nameof(Tags.Player)) 
                            || pickup.CompareTag(nameof(Tags.CriticalObstacle)))
                {
                    continue;
                }

                MovePickupTowardPlayer(pickup.transform);
            }
        }

        private void MovePickupTowardPlayer(Transform pickup)
        {
            pickup.position = Vector3.Lerp(
                pickup.position, 
                _playerTransform.position, 
                _attractionSpeed * Time.deltaTime
            );

            pickup.Rotate(Vector3.up * (_rotationSpeed * Time.deltaTime), Space.Self);
        }
    }
}