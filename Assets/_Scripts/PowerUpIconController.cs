using DG.Tweening;
using UnityEngine;

namespace _Scripts
{
    public class PowerUpIconController : Singleton<PowerUpIconController>
    {
        [Header("References")] 
        [SerializeField] private GameObject _doubleCoin;
        [SerializeField] private GameObject _shield;
        [SerializeField] private GameObject _evasion;
        [SerializeField] private GameObject _magnet;

        public void ShowDoubleCoin() => Show(_doubleCoin);

        public void HideDoubleCoin() => _doubleCoin.gameObject.SetActive(false);

        public void ShowShield() => Show(_shield);

        public void ShowMagnet() => Show(_magnet);

        public void HideShield() => _shield.gameObject.SetActive(false);

        public void ShowEvasion() => Show(_evasion);

        public void HideEvasion() => _evasion.gameObject.SetActive(false);

        private void Show(GameObject target)
        {
            CanvasGroup group = target.GetComponent<CanvasGroup>();

            group.alpha = 0;
            target.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);

            group.DOFade(1, 0.2f);
            target.transform.DOScale(Vector3.one, 0.4f).SetEase(Ease.OutBounce);

            target.gameObject.SetActive(true);
        }
    }
}