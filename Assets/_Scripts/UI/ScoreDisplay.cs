using TMPro;
using UnityEngine;

namespace _Scripts.UI
{
    public class ScoreDisplay: Singleton<ScoreDisplay>
    {
        [Header("References")]
        [SerializeField] private TMP_Text _label;

        private int _score = 0;

        public void IncreaseScore(int amount)
        {
            if (amount <= 0) return;
            
            _score += amount;
            _label.text = _score.ToString();
        }

        public void ResetScore() => _score = 0;
    }
}