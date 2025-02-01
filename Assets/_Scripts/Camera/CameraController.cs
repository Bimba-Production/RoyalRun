using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

namespace _Scripts.Camera
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    public sealed class CameraController : Singleton<CameraController>
    {
        [Header("Damage Effect")]
        [SerializeField] private float _damageDuration = 1f;
        [SerializeField] private Volume _damageEffect;
        
        [Header("SpeedUp Effect")]
        [SerializeField] private float _speedUpDuration = 1f;
        [SerializeField] private Volume _speedUpEffect;
        
        [Header("Shield Effect")]
        [SerializeField] private float _shieldDuration = 1f;
        [SerializeField] private Volume _shieldEffect;

        [Header("SpeedUp Particle")]
        [SerializeField] private ParticleSystem _speedUpParticleSystem;
        [SerializeField] private ParticleSystem _electricalSpeedUpParticleSystem;
        
        [Header("Settings")]
        [SerializeField] private float _minFov = 60f;
        [SerializeField] private float _maxFov = 100f;
        [SerializeField] private float _zoomDuration = 1f;

        private UnityEngine.Camera _camera;

        protected override void Awake()
        {
            _camera = GetComponent<UnityEngine.Camera>();
            base.Awake();
        }

        public void Shake() => transform.DOShakePosition(0.2f);

        public void ChangeCameraFOV(float acceleration)
        {
            StopCoroutine(ChangeFOVRoutine(acceleration));
            StartCoroutine(ChangeFOVRoutine(acceleration));

            if (acceleration > 0f) _speedUpParticleSystem.Play();
        }

        IEnumerator ChangeFOVRoutine(float acceleration)
        {
            float startFOV = _camera.fieldOfView;
            float targetFOV = Mathf.Clamp(startFOV + acceleration * 3, _minFov, _maxFov);

            float elapsedTime = 0f;
            while (elapsedTime < _zoomDuration)
            {
                float t = elapsedTime / _zoomDuration;
                elapsedTime += Time.deltaTime;
                _camera.fieldOfView = Mathf.Lerp(startFOV, targetFOV, t);
                yield return null;
            }

            _camera.fieldOfView = targetFOV;
        }
        
        public void ApplyDamageEffect() => VolumeEffectsController.Instance.ApplyEffect(0.7f, _damageEffect, _damageDuration);
        public void DisableDamageEffect() => VolumeEffectsController.Instance.DisableEffect(_damageEffect, _damageDuration);
        
        public void ApplySpeedUpEffect() => VolumeEffectsController.Instance.ApplyEffect(1f, _speedUpEffect, _speedUpDuration);
        public void DisableSpeedUpEffect() => VolumeEffectsController.Instance.DisableEffect(_speedUpEffect, _speedUpDuration);
        
        public void ApplyShieldEffect() => VolumeEffectsController.Instance.ApplyEffect(1f, _shieldEffect, _shieldDuration);
        public void DisableShieldEffect() => VolumeEffectsController.Instance.DisableEffect(_shieldEffect, _shieldDuration);
        
        public void ApplyElectricEffect() => _electricalSpeedUpParticleSystem.Play();
        public void DisableElectricEffect() => _electricalSpeedUpParticleSystem.Stop();

        public void OnCriticalStateMove()
        {
            transform.DOLocalMove(new Vector3(-1.645f, 9.81f, -13.6f), 0.5f);
            transform.DORotate(new Vector3(33.6f, 0f, 0f), 0.5f);
        }

        public void ResetPosition()
        {
            transform.DOLocalMove(new Vector3(-1.645f, 3.6f, -7.5f), 0.5f);
            transform.DORotate(new Vector3(13.387f, 0f, 0f), 0.5f);
        }
        public void SetDefaultFov() => _camera.fieldOfView = _minFov;
    }
}