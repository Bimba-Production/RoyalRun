using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public sealed class GameOverDisplay: Singleton<GameOverDisplay>
    {
        [Header("References")]
        [SerializeField] private Animator _anim;
        [SerializeField] private Button _button;

        public UnityEvent OnRestartClicked;
        
        private void OnEnable() => _button.onClick.AddListener(OnRestartClicked.Invoke);

        private void OnDisable() => _button.onClick.RemoveListener(OnRestartClicked.Invoke);

        public void Show() =>_anim.Play(GameOverKeys.Show.ToString());

        public void Hide() => _anim.Play(GameOverKeys.Hide.ToString());
    }

    public enum GameOverKeys
    {
        Show = 0,
        Hide = 1,
    }
}