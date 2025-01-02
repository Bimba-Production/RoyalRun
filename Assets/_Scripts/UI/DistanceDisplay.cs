using TMPro;
using UnityEngine;

namespace _Scripts.UI
{
    public class DistanceDisplay: MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TMP_Text _label;

        private float _distance = 0;

        public void IncreaseDistance(float amount)
        {
            if (amount <= 0) return;
            
            _distance += amount;
            _label.text = $"{((int)_distance).ToString()} m";
        }

        public void ResetDistance() => _distance = 0;
    }
}