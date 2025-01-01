using System.Collections.Generic;
using UnityEngine;

namespace Assets._Scripts
{
    public class Chunk : MonoBehaviour
    {
        [SerializeField] private GameObject _fence;
        [SerializeField] private GameObject[] _pickups;
        [SerializeField] private GameObject _coin;
        [SerializeField] private float _pickupSpawnChance = 0.15f;
        [SerializeField] private float _coinSpawnChance = 0.5f;
        [SerializeField] private float _coinSeperationLength = 2f;
        [SerializeField] private int _maxCoinToSpawn = 5;
        [SerializeField] private float[] _lanes = { -2.5f, 0f, 2.5f };
        [SerializeField] private List<GameObject> _laneObjects;
        [SerializeField] private List<int> _availableLanesIndexes = new List<int> { 0, 1, 2 };
        [SerializeField] private int _maxFenceCount;
        [SerializeField] private int _minFenceCount;

        private void Start()
        {
            SpawnFence();
            SpawnPickup();
            SpawnCoin();
        }

        private void InstantiateObjOnLine(GameObject prefab)
        {
            int randIndex = Random.Range(0, _availableLanesIndexes.Count);
            Vector3 pos = new Vector3(_lanes[_availableLanesIndexes[randIndex]], transform.position.y,
                transform.position.z);

            GameObject obj = Instantiate(prefab, pos, Quaternion.identity, transform);
            _laneObjects.Add(obj);
            _availableLanesIndexes.RemoveAt(randIndex);
        }

        public void SpawnPickup()
        {
            if (Random.value > _pickupSpawnChance) return;
            if (_availableLanesIndexes.Count == 0) return;

            int randIndex = Random.Range(0, _pickups.Length);

            InstantiateObjOnLine(_pickups[randIndex]);
        }

        public void SpawnCoin()
        {
            if (Random.value > _coinSpawnChance) return;
            if (_availableLanesIndexes.Count == 0) return;

            int randIndex = Random.Range(0, _availableLanesIndexes.Count);
            int coinsToSpawn = Random.Range(3, _maxCoinToSpawn + 1);

            float topOfChunkZpos = transform.position.z + (_coinSeperationLength * 2f);

            for (int i = 0; i < coinsToSpawn; i += 1)
            {
                Vector3 pos = new Vector3(_lanes[_availableLanesIndexes[randIndex]], transform.position.y,
                    (topOfChunkZpos - (i * _coinSeperationLength)));
                GameObject obj = Instantiate(_coin, pos, Quaternion.identity, transform);
                _laneObjects.Add(obj);
            }

            _availableLanesIndexes.RemoveAt(randIndex);
        }

        public void SpawnFence()
        {
            int fencesToSpawn = Random.Range(_minFenceCount, _maxFenceCount + 1);

            for (int i = 0; i < fencesToSpawn; i += 1)
            {
                if (_availableLanesIndexes.Count == 0) break;

                InstantiateObjOnLine(_fence);
            }
        }

        public void ResetChunkSlots()
        {
            _availableLanesIndexes = new List<int> { 0, 1, 2 };

            foreach (var item in _laneObjects)
            {
                Destroy(item, 1);
            }

            _laneObjects.Clear();
        }
    }
}