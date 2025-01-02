using UnityEngine;

namespace _Scripts.Obstacles
{
    public class ObstaclesMover: MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _cooldown = 0.25f;
        
        private float _currentCooldown = 0f;
        private Rigidbody _rb;

        private void Awake() => _rb = GetComponent<Rigidbody>();

        private void Update()
        {
            if (_currentCooldown > 0) _currentCooldown -= Time.deltaTime;          
            else
            {
                _currentCooldown = _cooldown;
                ApplyForce();
            }
        }

        private void ApplyForce() => _rb.AddForce(Vector3.back * 10 , ForceMode.Impulse);
    }
}