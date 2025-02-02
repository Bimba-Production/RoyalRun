using UnityEngine;
using System.Collections;

namespace _Scripts.Audio
{
    public sealed class AmbienceController: Singleton<AmbienceController>
    {
        [SerializeField] private AudioSource _source;
        [SerializeField] private float _minVolumeSpeed = 0.7f;
        [SerializeField] private float _maxVolumeSpeed = 1.25f;

        private float _volume = 0.05f;
        
        private readonly float _speedUpDuration = 4f;

        private void Start() => _source.volume = _volume;
        
        public void UpdateAmbience(float relativeSpeedValue)
        {
            float extraSpeed = (_maxVolumeSpeed - _minVolumeSpeed) * relativeSpeedValue;

            ChangeAmbienceSpeed(extraSpeed);
        }
        
        public void ChangeAmbienceSpeed(float acceleration)
        {
            StopCoroutine(nameof(ChangeAmbienceSpeedRoutine));
            StartCoroutine(ChangeAmbienceSpeedRoutine(acceleration));
        }

        IEnumerator ChangeAmbienceSpeedRoutine(float acceleration)
        {
            float startSpeed = _source.pitch;
            float targetSpeed = Mathf.Clamp(_minVolumeSpeed + acceleration, _minVolumeSpeed, _maxVolumeSpeed);

            float elapsedTime = 0f;
            while (elapsedTime < _speedUpDuration)
            {
                float t = elapsedTime / _speedUpDuration;
                elapsedTime += Time.deltaTime;
                _source.pitch = Mathf.Lerp(startSpeed, targetSpeed, t);
                yield return null;
            }

            _source.pitch = targetSpeed;
        }

        public void SetSound(float volume) => _source.volume = volume;
    }
}