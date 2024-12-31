using System;
using UnityEngine;

namespace Assets._Scripts
{
    public class ObstaclesMover: MonoBehaviour
    {
        [SerializeField] private float _cooldown = 0.25f;
        private float _currentCooldown = 0f;
        
        private Rigidbody _rb;

        private void Awake() => _rb = GetComponent<Rigidbody>();

        private void Update()
        {
            if (_currentCooldown > 0)
            {
                _currentCooldown -= Time.deltaTime;          
            }
            else
            {
                _currentCooldown = _cooldown;
                AplyForce();
            }
        }

        private void AplyForce()
        {
            _rb.AddForce(new Vector3(0,0,-10) , ForceMode.Impulse);
        }
    }
}