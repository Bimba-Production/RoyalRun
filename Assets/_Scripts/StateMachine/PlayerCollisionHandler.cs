using Assets._Scripts.StateMachine;
using UnityEngine;

namespace Assets._Scripts
{
    public class PlayerCollisionHandler : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private LevelGenerator _levelGenerator;
        [SerializeField] private ScoreDisplay _scoreDisplay;

        private void OnTriggerEnter(Collider other)
        {
            switch (other.tag)
            {
                case nameof(Tags.Obstacle):
                    if (!_playerController.IsCriticalCondition) _playerController.IsStumble = true;
                    else
                    {
                        _playerController.IsFall = true;
                        _playerController.OnGameOverEvent.Invoke();
                    }
                    break;

                case nameof(Tags.Coin):
                    other.GetComponent<Pickup>().OnPickup();
                    _scoreDisplay.IncreaseScore(1);
                    break;

                case nameof(Tags.Crown):
                    other.GetComponent<Pickup>().OnPickup();
                    _levelGenerator.SlowDownTheLevel();
                    break;

                case nameof(Tags.Star):
                    other.GetComponent<Pickup>().OnPickup();
                    break;

                case nameof(Tags.Electric):
                    other.GetComponent<Pickup>().OnPickup();
                    break;

                case nameof(Tags.Explosion):
                    other.GetComponent<Pickup>().OnPickup();
                    break;

                case nameof(Tags.Shield):
                    other.GetComponent<Pickup>().OnPickup();
                    break;

                default:
                    print("Unknown tag: " + other.gameObject.tag);
                    break;
            }
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