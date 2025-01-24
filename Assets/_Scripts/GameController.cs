using _Scripts.Level;
using _Scripts.Models;
using _Scripts.Obstacles;
using _Scripts.StateMachine;
using _Scripts.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts
{
    public sealed class GameController: Singleton<GameController>
    {
        [Header("References")]
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private ScoreDisplay _scoreDisplay;
        [SerializeField] private CapsuleCollider _playerCollider;
        [SerializeField] private ObstacleSpawner _obstacleSpawner;
        [SerializeField] private PlayerController _playerController;

        private void OnEnable()
        {
            UIController.Instance.GameOverDisplay.OnRestartClicked.AddListener(ResetGame);
            _playerController.OnGameOverEvent += HandleGameOver;
        }

        private void OnDisable()
        {
            UIController.Instance.GameOverDisplay.OnRestartClicked.RemoveListener(ResetGame);
            _playerController.OnGameOverEvent -= HandleGameOver;
        }

        private void HandleGameOver()
        {
            //Метод ресетящий позицию игрока и сбрасывающий его текущее ускорение.

            StopAllCoroutines();

            _obstacleSpawner.IsPaused = true;
            // _rb.useGravity = false;
            // _playerCollider.isTrigger = true;
            
            LevelGenerator.Instance.Pause();
            LevelGenerator.Instance.StopChunks();
            
            TimerDisplay.Instance.IsPaused = true;

            _scoreDisplay.UpdateScore(ScoreModel.Instance.coin, ScoreModel.Instance.distance, ScoreModel.Instance.time);
            UIController.Instance.GameOverDisplay.Show();
        }

        private void ResetGame()
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }

        private void ContinueGame()
        {
            // Метод который очищает несколько ближайших чанков от перепятствий.

            UIController.Instance.GameOverDisplay.Hide();

            _playerController.Reset();
            
            _obstacleSpawner.IsPaused = false;
            _obstacleSpawner.ResetSpawn();
            
            LevelGenerator.Instance.UnPause();
            LevelGenerator.Instance.ResetChunksSpeed();
            TimerDisplay.Instance.IsPaused = false;

            CoinDisplay.Instance.ResetScore();
            DistanceDisplay.Instance.ResetDistance();
            TimerDisplay.Instance.RestartTimer();
            
            // _rb.useGravity = true;
            // _playerCollider.isTrigger = false;
        }
    }
}