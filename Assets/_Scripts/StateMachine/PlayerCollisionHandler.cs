using _Scripts.Camera;
using _Scripts.Pickups;
using UnityEngine;

namespace _Scripts.StateMachine
{
    public class PlayerCollisionHandler : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private PlayerController _playerController;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(nameof(Tags.Obstacle)))
            {
                if (!_playerController.IsCriticalCondition)
                {
                    CameraController.Instance.ApplyDamageEffect();
                    _playerController.IsStumble = true;
                }
                else
                {
                    _playerController.IsFall = true;
                    _playerController.OnGameOverEvent.Invoke();
                }

                return;
            }

            other.GetComponent<Pickup>()?.OnPickup();
        }
    }

    public enum Tags
    {
        Obstacle = 0,
        Coin = 1,
        Apple = 2,
        Crown = 3,
        Electric = 4,
        Explosion = 5,
        Shield = 6,
        Star = 7,
    }
}