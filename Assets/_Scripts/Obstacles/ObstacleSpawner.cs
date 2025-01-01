using System.Collections;
using UnityEngine;

namespace Assets._Scripts
{
    public class ObstacleSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] _prefabs;
        [SerializeField] private float _spawnCooldownTime;
        [SerializeField] private Transform _parent;
        [SerializeField] private float _spawnBoundaries = 4f;

        public bool IsPaused { get;  set; }
        public void ResetSpawn() => StartCoroutine(SpawnObstacleRoutin());
        private void Start() => ResetSpawn();
        
        private IEnumerator SpawnObstacleRoutin()
        {
            while (!IsPaused)
            {
                yield return new WaitForSeconds(_spawnCooldownTime);
                GameObject prefab = _prefabs[Random.Range(0, _prefabs.Length)];
                Vector3 spawnPosition = transform.position +
                                        new Vector3(Random.Range(-_spawnBoundaries, _spawnBoundaries), 0, 0);
                Instantiate(prefab, spawnPosition, Random.rotation, _parent);
            }
        }
    }
}
