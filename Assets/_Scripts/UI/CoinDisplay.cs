using _Scripts.Models;
using TMPro;
using UnityEngine;

namespace _Scripts.UI
{
    public sealed class CoinDisplay: Singleton<CoinDisplay>
    {
        [Header("References")]
        [SerializeField] private TMP_Text _label;

        private int _score = 0;

        public void IncreaseScore(int amount)
        {
            if (amount <= 0) return;
            
            _score += amount;
            
            CurrentScore.Instance.Coin += amount;
            _label.text = _score.ToString();
        }

        public void ResetScore()
        {
            _score = 0;
            CurrentScore.Instance.Coin = 0;
        }
    }
}