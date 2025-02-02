using System.Collections;
using _Scripts.Audio;
using _Scripts.Camera;
using _Scripts.Chunks;
using _Scripts.UI;
using UnityEngine;

namespace _Scripts.Level
{
    public sealed class LevelGenerator : Singleton<LevelGenerator>
    {
        [Header("References")] 
        [SerializeField] private GameObject[] _chunkPrefabs;
        [SerializeField] private Transform _chunkParent;
        [SerializeField] private CameraController _cameraController;

        [Header("Level Settings")] 
        [SerializeField] private int _startingChunksAmount = 12;
        [SerializeField] private float _minChunkPos;
        [SerializeField] private float _minMoveSpeed = 8f;
        [SerializeField] private float _maxMoveSpeed = 24f;
        [SerializeField] private float _levelAcceleration = 2f;
        [SerializeField] private float _levelDecceleration = 1.5f;
        [SerializeField] private float _accelerationCooldown = 10f;
        [SerializeField] [Range(0, 100)] private int _probabilityOfChunkChange = 40;

        private readonly GameObject[] _chunkObjects = new GameObject[12];
        private readonly Chunk[] _chunks = new Chunk[12];
        private readonly ChunkMover[] _chunkMovers = new ChunkMover[12];
        private Chunk _currentDistantChunk;
        private GameObject _currentClosestChunk;
        private int _closestChunkIndex;
        private float _accelerationCooldownValue;
        private float _currentSpeed;
        
        public float Speed => _currentSpeed;

        private bool IsPaused { get; set; }

        public void ApplyAcceleration(float acceleration) => ChangeChunkMoveSpeed(acceleration);
        
        public void UnPause()
        {
            IsPaused = false;
            StartCoroutine(AccelerationCoroutine(_levelAcceleration));
        }

        public void Pause()
        {
            IsPaused = true;
            StopCoroutine(nameof(AccelerationCoroutine));
        }

        protected override void Awake()
        {
            _accelerationCooldownValue = _accelerationCooldown;
            _currentSpeed = _minMoveSpeed;
            StartCoroutine(AccelerationCoroutine(_levelAcceleration));
            base.Awake();
        }

        private void Start()
        {
            InitChunks();
        }

        private void Update()
        {
            float currSpeed = _chunkMovers[0].Speed;
            DistanceDisplay.Instance.IncreaseDistance(currSpeed * Time.deltaTime);
        }

        private IEnumerator AccelerationCoroutine(float acceleration)
        {
            YieldInstruction waitForSeconds = new WaitForSeconds(_accelerationCooldownValue);
            
            yield return waitForSeconds;
            
            while (true)
            {
                if (IsPaused) break;
                if (_currentSpeed >= _maxMoveSpeed) yield return new WaitForSeconds(_accelerationCooldownValue);;
                
                _accelerationCooldownValue = _accelerationCooldown;
                ChangeChunkMoveSpeed(acceleration);

                yield return waitForSeconds;
            }
        }

        private void LateUpdate() => UpdateChunksPos();

        public void GameOver()
        {
            foreach (var chunk in _chunkMovers) chunk.Speed = 0;
        }

        public void SlowDownTheLevel() => ChangeChunkMoveSpeed(-_levelDecceleration);

        private void ChangeChunkMoveSpeed(float acceleration)
        {
            if (IsPaused) return;
            if (_currentSpeed + acceleration > _maxMoveSpeed) return;

            // float relativeSpeedValue = GetRelativeSpeedValue(_currentSpeed + acceleration);
            
            // AmbienceController.Instance.UpdateAmbience(relativeSpeedValue);
            
            _cameraController.ChangeCameraFOV(acceleration);

            float speed = _currentSpeed + acceleration;
            float newSpeed = _minMoveSpeed <= speed ? speed : _minMoveSpeed;
            _currentSpeed = newSpeed;

            foreach (var mover in _chunkMovers) mover.Speed = newSpeed;
        }

        public void StopChunks()
        {
            _cameraController.SetDefaultFov();
            _currentSpeed = _minMoveSpeed;
            foreach (var mover in _chunkMovers) mover.Speed = 0f;
        }

        public void ResetChunksSpeed()
        {
            foreach (var mover in _chunkMovers) mover.Speed = _minMoveSpeed;
        }

        private void InitChunks()
        {
            float lastChunkSize = 0;
            for (int i = 0; i < _startingChunksAmount; i += 1)
            {
                Vector3 pos = new Vector3(transform.position.x, transform.position.y, lastChunkSize);
                GameObject chunkToSpawn = _chunkPrefabs[Random.Range(0, _chunkPrefabs.Length)];

                if (i < 2) chunkToSpawn = _chunkPrefabs[0];

                GameObject chunk = Instantiate(chunkToSpawn, pos, Quaternion.identity, _chunkParent);
                _chunkObjects[i] = chunk;
                _chunks[i] = chunk.GetComponent<Chunk>();
                _chunkMovers[i] = chunk.GetComponent<ChunkMover>();

                if (i > 2) _chunks[i].Init();
                lastChunkSize += _chunks[i].Size;
            }

            _closestChunkIndex = 0;
            _currentClosestChunk = _chunkObjects[0];
            _currentDistantChunk = _chunks[_startingChunksAmount - 1];
        }

        private void UpdateChunksPos()
        {
            if (_currentClosestChunk.transform.position.z > _minChunkPos) return;

            Chunks.Chunk closestChunk = _chunks[_closestChunkIndex];
            
            if (Random.Range(0, 100) < _probabilityOfChunkChange)
            {
                Chunks.Chunk oldChunk = _chunks[_closestChunkIndex];
                oldChunk.ResetChunkSlots(0);

                Vector3 pos = new Vector3(oldChunk.transform.position.x, oldChunk.transform.position.y, oldChunk.transform.position.z);

                GameObject chunkToSpawn = _chunkPrefabs[Random.Range(0, _chunkPrefabs.Length)];
                GameObject chunk = Instantiate(chunkToSpawn, pos, Quaternion.identity, _chunkParent);
                _chunkObjects[_closestChunkIndex] = chunk;
                _chunks[_closestChunkIndex] = chunk.GetComponent<Chunk>();
                _chunkMovers[_closestChunkIndex] = chunk.GetComponent<ChunkMover>();
                _chunkMovers[_closestChunkIndex].Speed = oldChunk.GetComponent<ChunkMover>().Speed;

                Destroy(oldChunk.gameObject);

                closestChunk = _chunks[_closestChunkIndex];
                closestChunk.Init();
            } else
            {
                closestChunk.ResetChunkSlots(1);
                closestChunk.SpawnFence();
                closestChunk.SpawnPickup();
                closestChunk.SpawnCoin();
            }

            _currentClosestChunk = closestChunk.gameObject;
            _currentClosestChunk.transform.position = new Vector3(transform.position.x, transform.position.y,
                _currentDistantChunk.transform.position.z + _currentDistantChunk.Size);
            _currentDistantChunk = _chunks[_closestChunkIndex];

            if (_closestChunkIndex < _chunks.Length - 1) _closestChunkIndex += 1;
            else _closestChunkIndex = 0;

            _currentClosestChunk = _chunkObjects[_closestChunkIndex];
        }

        private float GetRelativeSpeedValue(float currentSpeed)
        {
            float speedBoundaries =  _maxMoveSpeed - _minMoveSpeed;
            float currentValue = currentSpeed - _minMoveSpeed;
            
            return currentValue / speedBoundaries;
        }
    }
}