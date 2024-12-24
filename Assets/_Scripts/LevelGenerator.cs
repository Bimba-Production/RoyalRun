using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _chunkPrefab;
    [SerializeField] private int _startingChunksAmount = 12;
    [SerializeField] private Transform _chunkParent;
    [SerializeField] private float _minChunkPos;
    [SerializeField] private float _minMoveSpeed = 8f;
    [SerializeField] private float _levelAcceleration = 2f;
    [SerializeField] private float _levelDecceleration = 1.5f;
    [SerializeField] private float _accelerationCooldown = 10f;
    [SerializeField] private CameraController _cameraController;

    private float _chunkLength = 10f;
    private GameObject[] _chunks = new GameObject[12];
    private GameObject _currentDistantChunk;
    private GameObject _current—losestChunk;
    private int _closestChunkIndex;
    private float _accelerationCooldownValue;
    private float _currentSpeed;

    private void Awake()
    {
        _accelerationCooldownValue = _accelerationCooldown;
        _currentSpeed = _minMoveSpeed;
    }

    private void Start()
    {
        InitChunks();
    }

    private void Update()
    {
        _accelerationCooldownValue -= Time.deltaTime;

        if (_accelerationCooldownValue <= 0)
        {
            _accelerationCooldownValue = _accelerationCooldown;
            ChangeChunkMoveSpeed(_levelAcceleration);
        }
    }

    private void LateUpdate()
    {
        UpdateChunksPos();
    }

    public void GameOver()
    {
        foreach (var chunk in _chunks)
        {
            chunk.GetComponent<ChunkMover>().Speed = 0;
        }
    }

    public void SlowDownTheLevel()
    {
        ChangeChunkMoveSpeed(-_levelDecceleration);
    }

    public void ChangeChunkMoveSpeed(float acceleration)
    {
        _cameraController.ChangeCameraFOV(acceleration);

        float speed = _currentSpeed + acceleration;
        float newSpeed = _minMoveSpeed <= speed ? speed : _minMoveSpeed;
        float realAcceleration = newSpeed - _currentSpeed;
        _currentSpeed = newSpeed;

        foreach (var chunk in _chunks)
        {
            chunk.GetComponent<ChunkMover>().Speed = newSpeed;
        }

        Physics.gravity = new Vector3(Physics.gravity.x, Physics.gravity.y, Physics.gravity.z - realAcceleration);
    }

    private void InitChunks()
    {
        for (int i = 0; i < _startingChunksAmount; i += 1)
        {
            Vector3 pos = new Vector3(transform.position.x, transform.position.y, _chunkLength * i);
            GameObject chunk = Instantiate(_chunkPrefab, pos, Quaternion.identity, _chunkParent);
            _chunks[i] = chunk;
        }

        _closestChunkIndex = 0;
        _current—losestChunk = _chunks[0];
        _currentDistantChunk = _chunks[_startingChunksAmount - 1];
    }

    private void UpdateChunksPos()
    {
       if (_current—losestChunk.transform.position.z <= _minChunkPos)
       {
            _current—losestChunk.GetComponent<Chunk>().ResetChunkSlots();
            _current—losestChunk.GetComponent<Chunk>().SpawnFence();
            _current—losestChunk.GetComponent<Chunk>().SpawnPickup();
            _current—losestChunk.GetComponent<Chunk>().SpawnCoin();
            _current—losestChunk.transform.position = new Vector3(transform.position.x, transform.position.y, _currentDistantChunk.transform.position.z + _chunkLength);
            _currentDistantChunk = _current—losestChunk;

            if (_closestChunkIndex < _chunks.Length - 1) {
                _closestChunkIndex += 1;
            } else
            {
                _closestChunkIndex = 0;
            }

            _current—losestChunk = _chunks[_closestChunkIndex];
       }
    }
}
