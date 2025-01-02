using System;
using TMPro;
using UnityEngine;

namespace _Scripts.UI
{
    public class ScoreDisplay: MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TMP_Text _coinLabel;
        [SerializeField] private TMP_Text _distanceLabel;
        [SerializeField] private TMP_Text _timerLabel;

        public void UpdateScore(int coin, float distance, int timer)
        {
            _coinLabel.text = coin.ToString();
            _distanceLabel.text = $"{((int)distance).ToString()} m";
            
            TimeSpan timeSpan = TimeSpan.FromSeconds(timer);

            string formattedTime = string.Format("{0:D2}:{1:D2}",
                timeSpan.Minutes,
                timeSpan.Seconds);
            
            _timerLabel.text = formattedTime;
        }
    }
}