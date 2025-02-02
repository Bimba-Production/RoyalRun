using System.Collections.Generic;
using UnityEngine;

namespace _Scripts
{
    public sealed class HitEffectSpawner: Singleton<HitEffectSpawner>
    {
        [Header("References")]
        [SerializeField] private ParticleSystem _hitVFX;
        [SerializeField] private Transform _parent;
        
        private static readonly int _initCapacity = 2;
        private List<ParticleSystem> _vfxPool = new List<ParticleSystem>(_initCapacity);

        private void Start()
        {
            for (int i = 0; i < _initCapacity; i+=1)
            {
                ParticleSystem vfx = Instantiate(_hitVFX, _parent);
                _vfxPool.Add(vfx);
                vfx.gameObject.SetActive(false);
            }
        }

        public void Play(Vector3 position, Vector3 rotation)
        {
            ParticleSystem vfx = GetFreeSystem();
            
            vfx.gameObject.SetActive(true);
            vfx.transform.position = position;
            vfx.transform.rotation = Quaternion.Euler(rotation);
            vfx.Play();
        }

        private ParticleSystem GetFreeSystem()
        {
            for (int i = 0; i < _vfxPool.Count; i+=1)
            {
                if (!_vfxPool[i].gameObject.activeSelf) return _vfxPool[i];
            }
            
            ParticleSystem vfx = Instantiate(_hitVFX, _parent);
            _vfxPool.Add(vfx);

            return vfx;
        }
    }
}