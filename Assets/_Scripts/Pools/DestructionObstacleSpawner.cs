using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Pools
{
    //Нужна фабрика для этого дерьма
    public class DestructionObstacleSpawner: Singleton<DestructionObstacleSpawner>
    {
        [Header("References")]
        [SerializeField] private ParticleSystem _obstacleDestroyVFX;
        [SerializeField] private Transform _parent;

        private const int _initCapacity = 2;
        private readonly List<ParticleSystem> _vfxPool = new(_initCapacity);

        private void Start()
        {
            for (int i = 0; i < _initCapacity; i+=1)
            {
                ParticleSystem vfx = Instantiate(_obstacleDestroyVFX, _parent);
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
            
            ParticleSystem vfx = Instantiate(_obstacleDestroyVFX, _parent);
            _vfxPool.Add(vfx);

            return vfx;
        }
    }
}