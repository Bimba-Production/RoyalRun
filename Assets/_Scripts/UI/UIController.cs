using UnityEngine;

namespace _Scripts.UI
{
    public class UIController: Singleton<UIController>
    {
        [Header("References")]
        [SerializeField] private GameOverDisplay _gameOverDisplay;
        
        public GameOverDisplay GameOverDisplay => _gameOverDisplay;
    }
}