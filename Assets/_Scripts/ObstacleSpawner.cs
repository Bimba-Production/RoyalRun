using System.Collections;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _prefabs;
    [SerializeField] private float _spawnCooldownTime;
    [SerializeField] private Transform _parent;
    [SerializeField] private float _spawnBoundaries = 4f;

    private void Start()
    {
        StartCoroutine(SpawnObstacleRoutin());
    }

    private IEnumerator SpawnObstacleRoutin()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnCooldownTime);
            GameObject prefab = _prefabs[Random.Range(0, _prefabs.Length)];
            Vector3 spawnPosition = transform.position + new Vector3(Random.Range(-_spawnBoundaries, _spawnBoundaries), 0, 0);
            Instantiate(prefab, spawnPosition, Random.rotation, _parent);
        }
    }
}
