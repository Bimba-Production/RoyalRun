using TMPro;
using UnityEngine;

namespace _Scripts.UI
{
    public sealed class FPSDisplay : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TMP_Text _fpsLabel;
        
        private float _deltaTime = 0.0f;

        private void Update()
        {
            _deltaTime += (Time.deltaTime - _deltaTime) * 0.1f;
        }

        private void OnGUI()
        {
            float fps = 1.0f / _deltaTime;
            _fpsLabel.text = $"{fps:0.} FPS";
        }
    }
}