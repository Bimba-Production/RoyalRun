using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets._Scripts
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Volume _damagEffect;
        [SerializeField] private ParticleSystem _speedUpParticleSystem;
        
        [Header("Settings")]
        [SerializeField] private float _minFov = 60f;
        [SerializeField] private float _maxFov = 100f;
        [SerializeField] private float _zoomDuration = 1f;
        [SerializeField] private float _damageDuration = 1f;

        private Camera _camera;

        private void Awake() => _camera = GetComponent<Camera>();

        public void ChangeCameraFOV(float acceleration)
        {
            StopAllCoroutines();
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

        public void ApplyDamageEffect()
        {
            StopAllCoroutines();
            StartCoroutine(ApplyDamageEffectRoutine(0.7f));
        }

        public void DisableyDamageEffect()
        {
            StopAllCoroutines();
            StartCoroutine(ApplyDamageEffectRoutine(0f));
        }

        IEnumerator ApplyDamageEffectRoutine(float intensity)
        {
            float startWeight = _damagEffect.weight;
            float targetWeight = intensity;

            float elapsedTime = 0f;
            while (elapsedTime < _damageDuration)
            {
                float t = elapsedTime / _damageDuration;
                elapsedTime += Time.deltaTime;
                _damagEffect.weight = Mathf.Lerp(startWeight, targetWeight, t);
                yield return null;
            }

            _damagEffect.weight = targetWeight;
        }

        public void SetDefaultFov() => _camera.fieldOfView = _minFov;
    }
}