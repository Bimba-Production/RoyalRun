using _Scripts.Models;
using TMPro;
using UnityEngine;

namespace _Scripts.UI
{
    public sealed class DistanceDisplay: Singleton<DistanceDisplay>
    {
        [Header("References")]
        [SerializeField] private TMP_Text _label;

        private float _distance = 0;

        public void IncreaseDistance(float amount)
        {
            if (amount <= 0) return;
            
            _distance += amount;
            ScoreModel.Instance.distance = _distance;
            _label.text = $"{((int)_distance).ToString()} m";
        }

        public void ResetDistance()
        {
            _distance = 0;
            ScoreModel.Instance.distance = 0;
        }
    }
}