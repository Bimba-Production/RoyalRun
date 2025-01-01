using Assets._Scripts.StateMachine;
using UnityEngine;

namespace Assets._Scripts
{
    public class GameController: MonoBehaviour
    {
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private LevelGenerator _levelGenerator;
        [SerializeField] private ObstacleSpawner _obstacleSpawner;

        private void OnEnable() => _playerController.OnGameOverEvent += HandleGameOver;
        private void OnDisable() => _playerController.OnGameOverEvent -= HandleGameOver;
        
        private void HandleGameOver()
        {
            _obstacleSpawner.IsPaused = true;
            _levelGenerator.IsPaused = true;
            _levelGenerator.StopChunks();
            
        }

        private void ResetGame()
        {
            _obstacleSpawner.IsPaused = false;
            _levelGenerator.IsPaused = false;
            _levelGenerator.ResetChunksSpeed();
        }
    }
}