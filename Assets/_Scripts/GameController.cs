using _Scripts.Level;
using _Scripts.Obstacles;
using _Scripts.StateMachine;
using _Scripts.UI;
using UnityEngine;

namespace _Scripts
{
    public class GameController: Singleton<GameController>
    {
        [Header("References")]
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private ObstacleSpawner _obstacleSpawner;
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private CapsuleCollider _playerCollider;

        private void OnEnable() => _playerController.OnGameOverEvent += HandleGameOver;
        private void OnDisable() => _playerController.OnGameOverEvent -= HandleGameOver;
        
        private void HandleGameOver()
        {
            _obstacleSpawner.IsPaused = true;
            _rb.useGravity = false;
            _playerCollider.isTrigger = true;
            
            LevelGenerator.Instance.Pause();
            LevelGenerator.Instance.StopChunks();
            
            TimerDisplay.Instance.IsPaused = true;
        }

        private void ResetGame()
        {
            _obstacleSpawner.IsPaused = false;
            
            LevelGenerator.Instance.UnPause();
            LevelGenerator.Instance.ResetChunksSpeed();
            TimerDisplay.Instance.IsPaused = false;
            
            _rb.useGravity = true;
            _playerCollider.isTrigger = false;
        }
    }
}