using UnityEngine;

namespace Assets._Scripts
{
    public class LevelGenerator : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _chunkPrefab;
        [SerializeField] private Transform _chunkParent;
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private DistanceDisplay _distanceDisplay;
        
        [Header("Level Settings")]
        [SerializeField] private int _startingChunksAmount = 12;
        [SerializeField] private float _minChunkPos;
        [SerializeField] private float _minMoveSpeed = 8f;
        [SerializeField] private float _levelAcceleration = 2f;
        [SerializeField] private float _levelDecceleration = 1.5f;
        [SerializeField] private float _accelerationCooldown = 10f;

        private readonly float _chunkLength = 10f;
        private readonly GameObject[] _chunkObjects = new GameObject[12];
        private readonly Chunk[] _chunks = new Chunk[12];
        private readonly ChunkMover[] _chunkMovers = new ChunkMover[12];
        private GameObject _currentDistantChunk;
        private GameObject _currentClosestChunk;
        private int _closestChunkIndex;
        private float _accelerationCooldownValue;
        private float _currentSpeed;

        public bool IsPaused { get; set; }
        
        private void Awake()
        {
            _accelerationCooldownValue = _accelerationCooldown;
            _currentSpeed = _minMoveSpeed;
        }

        private void Start() => InitChunks();

        private void Update()
        {
            float currSpeed = _chunkMovers[0].Speed;
            _distanceDisplay.IncreaseDistance(currSpeed * Time.deltaTime);
            
            if (!IsPaused) UpdateSpeedUp();
        }

        private void UpdateSpeedUp()
        {
            _accelerationCooldownValue -= Time.deltaTime;

            if (_accelerationCooldownValue <= 0)
            {
                _accelerationCooldownValue = _accelerationCooldown;
                ChangeChunkMoveSpeed(_levelAcceleration);
            }
        }

        private void LateUpdate() => UpdateChunksPos();

        public void GameOver()
        {
            foreach (var chunk in _chunks) chunk.GetComponent<ChunkMover>().Speed = 0;
        }

        public void SlowDownTheLevel() => ChangeChunkMoveSpeed(-_levelDecceleration);

        private void ChangeChunkMoveSpeed(float acceleration)
        {
            _cameraController.ChangeCameraFOV(acceleration);

            float speed = _currentSpeed + acceleration;
            float newSpeed = _minMoveSpeed <= speed ? speed : _minMoveSpeed;
            _currentSpeed = newSpeed;

            foreach (var mover in _chunkMovers) mover.Speed = newSpeed;
        }
        
        public void StopChunks()
        {
            _cameraController.SetDefaultFov();

            foreach (var mover in _chunkMovers) mover.Speed = 0f;
        }

        public void ResetChunksSpeed()
        {
            foreach (var mover in _chunkMovers) mover.Speed = _minMoveSpeed;
        }

        private void InitChunks()
        {
            for (int i = 0; i < _startingChunksAmount; i += 1)
            {
                Vector3 pos = new Vector3(transform.position.x, transform.position.y, _chunkLength * i);
                GameObject chunk = Instantiate(_chunkPrefab, pos, Quaternion.identity, _chunkParent);
                _chunkObjects[i] = chunk;
                _chunks[i] = chunk.GetComponent<Chunk>();
                _chunkMovers[i] = chunk.GetComponent<ChunkMover>();
            }

            _closestChunkIndex = 0;
            _currentClosestChunk = _chunkObjects[0];
            _currentDistantChunk = _chunkObjects[_startingChunksAmount - 1];
        }

        private void UpdateChunksPos()
        {
            if (_currentClosestChunk.transform.position.z <= _minChunkPos)
            {
                Chunk closestChunk = _chunks[_closestChunkIndex];

                closestChunk.ResetChunkSlots();
                closestChunk.SpawnFence();
                closestChunk.SpawnPickup();
                closestChunk.SpawnCoin();

                _currentClosestChunk.transform.position = new Vector3(transform.position.x, transform.position.y,
                    _currentDistantChunk.transform.position.z + _chunkLength);
                _currentDistantChunk = _currentClosestChunk;

                if (_closestChunkIndex < _chunks.Length - 1) _closestChunkIndex += 1;
                else _closestChunkIndex = 0;

                _currentClosestChunk = _chunkObjects[_closestChunkIndex];
            }
        }
    }
}