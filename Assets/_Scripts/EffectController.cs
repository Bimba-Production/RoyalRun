using _Scripts.Camera;
using UnityEngine;

namespace _Scripts
{
    public sealed class EffectController: Singleton<EffectController>
    {
        [Header("SpeedUp Effect Activity")] 
        [SerializeField] private float _sUEffectActivityTime = 8f;
        [SerializeField] private float _sUEffectEndTime = 0f;
        [SerializeField] private bool _sUEffectIsActive = false;

        [Header("Shield Effect Activity")] 
        [SerializeField] private float _sEffectActivityTime = 8f;
        [SerializeField] private float _sEffectEndTime = 0f;
        [SerializeField] private bool _sEffectIsActive = false;
        
        [Header("Electrical Effect Activity")] 
        [SerializeField] private float _eEffectActivityTime = 8f;
        [SerializeField] private float _eEffectEndTime = 0f;
        [SerializeField] private bool _eEffectIsActive = false;
        
        public bool SpeedUpEffectIsActive() => _sUEffectIsActive;
        public bool ShieldEffectIsActive() => _sEffectIsActive;
        public bool ElectricalEffectIsActive() => _eEffectIsActive;
        
        public void ApplySpeedUpEffect()
        {
            if (!_sUEffectIsActive)
            {
                _sUEffectIsActive = true;
                _sUEffectEndTime = Time.realtimeSinceStartup + _sUEffectActivityTime;
                CameraController.Instance.ApplySpeedUpEffect();
            }
            else
            {
                _sUEffectEndTime += _sUEffectActivityTime;
            }
        }
        
        public void ApplyShieldEffect()
        {
            if (!_sEffectIsActive)
            {
                _sEffectIsActive = true;
                _sEffectEndTime = Time.realtimeSinceStartup + _sEffectActivityTime;
                CameraController.Instance.ApplyShieldEffect();
            }
            else
            {
                _sUEffectEndTime += _sEffectActivityTime;
            }
        }
        
        public void ApplyElectricalEffect()
        {
            if (!_eEffectIsActive)
            {
                _eEffectIsActive = true;
                _eEffectEndTime = Time.realtimeSinceStartup + _eEffectActivityTime;
                CameraController.Instance.ApplyElectricEffect();
            }
            else
            {
                _eEffectEndTime += _eEffectActivityTime;
            }
        }
        
        private void Update()
        {
            if (_sUEffectIsActive && Time.realtimeSinceStartup >= _sUEffectEndTime) DisableSpeedUpEffect();
            if (_sEffectIsActive && Time.realtimeSinceStartup >= _sEffectEndTime) DisableShieldEffect();
            if (_eEffectIsActive && Time.realtimeSinceStartup >= _eEffectEndTime) DisableElectricEffect();
        }

        public void DisableSpeedUpEffect()
        {
            _sUEffectIsActive = false;
            _sUEffectEndTime = 0f;
            CameraController.Instance.DisableSpeedUpEffect();
        }

        public void DisableShieldEffect()
        {
            _sEffectIsActive = false;
            _sEffectEndTime = 0f;
            CameraController.Instance.DisableShieldEffect();
        }

        public void DisableElectricEffect()
        {
            _eEffectIsActive = false;
            _eEffectEndTime = 0f;
            CameraController.Instance.DisableElectricEffect();
        }
    }
}