using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace _Scripts.Audio
{
    public sealed class AudioEffectController: Singleton<AudioEffectController>
    {
        [Header("References")]
        [SerializeField] private AudioSource _sourcePrefab;
        [SerializeField] private Transform _parent;
        [SerializeField] private AudioResource[] _resources;
        [SerializeField] private float _volume = 1.0f;
        
        private static readonly int _initCapacity = 20;
        private List<AudioSource> _sourcePool = new List<AudioSource>(_initCapacity);
        private Dictionary<string, AudioResource> _resourceDictionary = new Dictionary<string, AudioResource>();

        private void Start()
        {
            foreach (var resource in _resources)
            {
                if (resource != null) _resourceDictionary.TryAdd(resource.name, resource);
            }
            
            for (int i = 0; i < _initCapacity; i+=1)
            {
                AudioSource source = Instantiate(_sourcePrefab, _parent);
                _sourcePool.Add(source);
            }

            SetSound(_volume);
        }

        public void Play(AudioEffectNames effectName, Vector3 position)
        {
            AudioSource source = GetFreeSource();

            source.transform.position = position;
            source.resource = GetAudioResource(effectName);
            source.Play();
        }

        private AudioResource GetAudioResource(AudioEffectNames effectName)
        {
            _resourceDictionary.TryGetValue(effectName.ToString(), out var resource);
            
            return resource;
        }

        private AudioSource GetFreeSource()
        {
            for (int i = 0; i < _sourcePool.Count; i+=1)
            {
                if (!_sourcePool[i].isPlaying) return _sourcePool[i];
            }
            
            AudioSource source = Instantiate(_sourcePrefab, _parent);
            source.volume = _volume;
            _sourcePool.Add(source);

            return source;
        }
        
        public void SetSound(float volume)
        {
            foreach (var source in _sourcePool)
            {
                source.volume = volume;
            }
        }
    }

    public enum AudioEffectNames
    {
        hit = 0,
        jump = 1,
        roll = 2,
        onGameOver = 3,
        slide = 4,
        explosion1 = 5,
    }
}