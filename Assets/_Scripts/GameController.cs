using _Scripts.Level;
using _Scripts.Obstacles;
using _Scripts.StateMachine;
using UnityEngine;

namespace _Scripts
{
    public class GameController: MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private LevelGenerator _levelGenerator;
        [SerializeField] private ObstacleSpawner _obstacleSpawner;
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private CapsuleCollider _playerCollider;

        private void OnEnable() => _playerController.OnGameOverEvent += HandleGameOver;
        private void OnDisable() => _playerController.OnGameOverEvent -= HandleGameOver;
        
        private void HandleGameOver()
        {
            _obstacleSpawner.IsPaused = true;
            _levelGenerator.IsPaused = true;
            _levelGenerator.StopChunks();
            _rb.useGravity = false;
            _playerCollider.isTrigger = true;
        }

        private void ResetGame()
        {
            _obstacleSpawner.IsPaused = false;
            _levelGenerator.IsPaused = false;
            _levelGenerator.ResetChunksSpeed();
            _rb.useGravity = true;
            _playerCollider.isTrigger = false;
        }
    }
}