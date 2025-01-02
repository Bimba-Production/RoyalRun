using System;
using _Scripts.Models;
using TMPro;
using UnityEngine;

namespace _Scripts.UI
{
    public class TimerDisplay: Singleton<TimerDisplay>
    {
        [Header("References")]
        [SerializeField] private TMP_Text _label;

        private int _timer = 0;

        private float _tickCooldown = 1f;
        private float _tickCooldownTimer = 0f;
        
        public bool IsPaused { get; set; }
            
        private void Update()
        {
            if (IsPaused) return;
            
            if (_tickCooldownTimer < _tickCooldown)
            {
                _tickCooldownTimer += Time.deltaTime;
            }
            else
            {
                _tickCooldownTimer = 0f;
                _timer += 1;
                
                ScoreModel.Instance.time = _timer;
                UpdateTimer();
            }
        }

        private void UpdateTimer()
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(_timer);

            string formattedTime = string.Format("{0:D2}:{1:D2}",
                timeSpan.Minutes,
                timeSpan.Seconds);
            
            _label.text = formattedTime;
        }

        public void RestartTimer()
        {
            _timer = 0;
            UpdateTimer();
            IsPaused = false;
            ScoreModel.Instance.time = 0;
        }
    }
}