using _Scripts.Models;
using TMPro;
using UnityEngine;

namespace _Scripts.UI
{
    public sealed class CoinDisplay: Singleton<CoinDisplay>
    {
        [Header("References")]
        [SerializeField] private TMP_Text _label;
        private readonly int _multiplier = 2;

        private int _score = 0;

        public void IncreaseScore(int amount)
        {
            if (amount <= 0) return;
            
            int realAmount = EffectController.Instance.SpeedUpEffectIsActive() ? amount * _multiplier : amount;
            _score += realAmount;
            
            CurrentScore.Instance.Coin += realAmount;
            _label.text = _score.ToString();
        }

        public void ResetScore()
        {
            _score = 0;
            CurrentScore.Instance.Coin = 0;
        }
    }
}