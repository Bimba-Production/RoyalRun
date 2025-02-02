using System;
using _Scripts.Audio;
using _Scripts.Level;
using _Scripts.Models;
using _Scripts.Obstacles;
using _Scripts.Save;
using _Scripts.StateMachine;
using _Scripts.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace _Scripts
{
    public sealed class GameController : Singleton<GameController>
    {
        [Header("References")] [SerializeField]
        private Rigidbody _rb;

        [FormerlySerializedAs("_scoreDisplay")] [SerializeField] private CurrentScoreDisplay currentScoreDisplay;
        [SerializeField] private BestScoreDisplay _bestScoreDisplay;
        [SerializeField] private CapsuleCollider _playerCollider;
        [SerializeField] private ObstacleSpawner _obstacleSpawner;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private GameObject _rock;

        private RockMover _rockMover;
        protected override void Awake()
        {
            var savedBestScore = SaveManager.Load();
            _rockMover = _rock.GetComponent<RockMover>();
            // BestScore.Instance.SetScore(savedBestScore.Coin , savedBestScore.Distance, savedBestScore.Time); 
            _bestScoreDisplay.UpdateScore(savedBestScore.Coin , savedBestScore.Distance, savedBestScore.Time);
            
            base.Awake();
        }

        public bool IsGameOver { get; private set; } = false;

        private void OnEnable()
        {
            UIController.Instance.GameOverDisplay.OnRestartClicked.AddListener(ResetGame);
            _playerController.OnGameOverEvent.AddListener(HandleGameOver);
        }

        private void OnDisable()
        {
            UIController.Instance.GameOverDisplay.OnRestartClicked.RemoveListener(ResetGame);
            _playerController.OnGameOverEvent.RemoveListener(HandleGameOver);
        }

        private void HandleGameOver()
        {
            AudioEffectController.Instance.Play(AudioEffectNames.onGameOver, PlayerMover.Instance.transform.position);

            IsGameOver = true;
            _rockMover.IsGameOver = true;
            //Метод ресетящий позицию игрока и сбрасывающий его текущее ускорение.

            StopAllCoroutines();

            _obstacleSpawner.IsPaused = true;
            
            LevelGenerator.Instance.Pause();
            LevelGenerator.Instance.StopChunks();

            TimerDisplay.Instance.IsPaused = true;

            currentScoreDisplay.UpdateScore(CurrentScore.Instance.Coin, CurrentScore.Instance.Distance, CurrentScore.Instance.Time);

            var savedBestScore = SaveManager.Load();
            var updatedBestScore = new GameData(0, 
                Math.Max(CurrentScore.Instance.Coin, savedBestScore.Coin), 
                Math.Max(CurrentScore.Instance.Time, savedBestScore.Time), 
                Math.Max(CurrentScore.Instance.Distance, savedBestScore.Distance));

            SaveManager.Save(updatedBestScore);
            
            _bestScoreDisplay.UpdateScore(updatedBestScore.Coin , updatedBestScore.Distance, updatedBestScore.Time);
            
            UIController.Instance.GameOverDisplay.Show();
        }

        private void ResetGame()
        {
            IsGameOver = false;
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }

        private void ContinueGame()
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

        public void ActivateRock()
        {
            _rock.SetActive(true);
        }

        public void DeactivateRock()
        {
            if (!IsGameOver) _rock.SetActive(false);
            else Destroy(_rock.gameObject, 15f);
        }
    }
}