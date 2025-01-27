using System;
using TMPro;
using UnityEngine;

namespace _Scripts.UI
{
    public sealed class BestScoreDisplay : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private TMP_Text _coinLabel;
        [SerializeField] private TMP_Text _distanceLabel;
        [SerializeField] private TMP_Text _timerLabel;

        public void UpdateScore(int coin, float distance, int seconds)
        {
            _coinLabel.text = coin.ToString();
            _distanceLabel.text = $"{(int)distance} m";

            TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);

            string formattedTime = $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";

            _timerLabel.text = formattedTime;
        }
    }
}