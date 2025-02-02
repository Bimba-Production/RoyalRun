using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.UI
{
    public sealed class SettingsWindowController: MonoBehaviour
    {
        [SerializeField] private CanvasGroup _optionsWindow;
        [SerializeField] private CanvasGroup _settingsWindow;

        public void OnClose()
        {
            GameController.Instance.ResumeGame();
            _settingsWindow.DOFade(0f, 0.25f).OnComplete(()=>_settingsWindow.gameObject.SetActive(false));
        }
        
        public void OnRestart()
        {
            SceneManager.LoadScene(1);
        }
        
        public void OnOptions()
        {
            _settingsWindow.DOFade(0f, 0.25f).OnComplete(()=>_settingsWindow.gameObject.SetActive(false));
            _optionsWindow.gameObject.SetActive(true);
            _optionsWindow.DOFade(1f, 0.25f)
                .OnComplete(()=> _optionsWindow.transform.DOScale(1.1f, 0.5f).SetEase(Ease.OutBounce));
        }
        
        public void OnExit()
        {
            SceneManager.LoadScene(0);
        }
    }
}