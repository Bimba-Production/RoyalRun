using _Scripts.Models;
using TMPro;
using UnityEngine;

namespace _Scripts.UI
{
    public class CoinDisplay: Singleton<CoinDisplay>
    {
        [Header("References")]
        [SerializeField] private TMP_Text _label;

        private int _score = 0;

        public void IncreaseScore(int amount)
        {
            if (amount <= 0) return;
            
            _score += amount;
            
            ScoreModel.Instance.coin += amount;
            _label.text = _score.ToString();
        }

        public void ResetScore()
        {
            _score = 0;
            ScoreModel.Instance.coin = 0;
        }
    }
}