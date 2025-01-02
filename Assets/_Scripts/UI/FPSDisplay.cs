using TMPro;
using UnityEngine;

namespace Assets._Scripts
{
    public class FPSDisplay : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TMP_Text _fpsLabel;
        
        private float _deltaTime = 0.0f;

        void Update()
        {
            _deltaTime += (Time.deltaTime - _deltaTime) * 0.1f;
        }

        void OnGUI()
        {
            float fps = 1.0f / _deltaTime;
            _fpsLabel.text = $"{fps:0.} FPS";
        }
    }
}