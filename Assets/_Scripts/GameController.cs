using System;
using _Scripts.Level;
using _Scripts.Models;
using _Scripts.Obstacles;
using _Scripts.Save;
using _Scripts.StateMachine;
using _Scripts.UI;
using Unity.VisualScripting;
using UnityEngine;

namespace _Scripts
{
    public class GameController : Singleton<GameController>
    {
        [Header("References")] [SerializeField]
        private Rigidbody _rb;

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
            _obstacleSpawner.IsPaused = true;
            _rb.useGravity = false;
            _playerCollider.isTrigger = true;

            LevelGenerator.Instance.Pause();
            LevelGenerator.Instance.StopChunks();

            TimerDisplay.Instance.IsPaused = true;

            _scoreDisplay.UpdateScore(CurrentScore.Instance.coin, CurrentScore.Instance.distance, CurrentScore.Instance.time);

            var savedBestScore = SaveManager.Load();
            var updatedBestScore = new GameData(0, 
                Math.Max(CurrentScore.Instance.coin, savedBestScore.Coin), 
                Math.Max(CurrentScore.Instance.time, savedBestScore.Time), 
                Math.Max(CurrentScore.Instance.distance, savedBestScore.Distance));

            SaveManager.Save(updatedBestScore);
            UIController.Instance.GameOverDisplay.Show();
        }

        private void ResetGame()
        {
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

            _rb.useGravity = true;
            _playerCollider.isTrigger = false;
        }
    }
}