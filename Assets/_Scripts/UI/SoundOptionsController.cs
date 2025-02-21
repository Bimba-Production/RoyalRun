using DG.Tweening;
using UnityEngine;

namespace _Scripts.UI
{
    public sealed class SoundOptionsController: MonoBehaviour
    {
        [SerializeField] private CanvasGroup _optionsWindow;
        
        public void OnClose()
        {
            GameController.Instance.ResumeGame();
            _optionsWindow.DOFade(0f, 0.25f).OnComplete(()=>_optionsWindow.gameObject.SetActive(false));
        }
    }
}