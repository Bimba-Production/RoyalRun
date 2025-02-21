using DG.Tweening;
using UnityEngine;

namespace _Scripts.UI
{
    public sealed class SettingController: MonoBehaviour
    {
        [SerializeField] private CanvasGroup _settingsWindow;

        public void OnClick()
        {
            GameController.Instance.PauseGame();
            
            _settingsWindow.gameObject.SetActive(true);
            _settingsWindow.DOFade(1f, 0.25f)
                .OnComplete(()=> _settingsWindow.transform.DOScale(1.1f, 0.5f).SetEase(Ease.OutBounce));
        }
    }
}