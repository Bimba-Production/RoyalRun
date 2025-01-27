using _Scripts.Camera;
using _Scripts.Obstacles;
using _Scripts.Pickups;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.StateMachine
{
    public sealed class PlayerCollisionHandler : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private PlayerController _playerController;

        private void OnTriggerEnter(Collider other)
        {
            if (GameController.Instance.IsGameOver) return;
            
            if (other.CompareTag(nameof(Tags.Obstacle)))
            {
                Collider collider = other.GetComponent<Collider>()!;
                
                if (EffectController.Instance.ShieldEffectIsActive())
                {
                    DestructionObstacleSpawner.Instance.Play(collider.transform.position, Vector3.zero);
                    Destroy(collider.gameObject);
                    EffectController.Instance.DisableShieldEffect();
                    return;
                }
                
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

                collider.enabled = false;
                other.transform.DOPunchPosition(Vector3.forward, 0.2f, 3).OnComplete(() =>
                {
                    collider.enabled = true;
                });
                
                return;
            }

            if (other.CompareTag(nameof(Tags.CriticalObstacle)))
            {
                Transform transform = other.transform.GetComponentInParent<Transform>();

                if (EffectController.Instance.ShieldEffectIsActive())
                {
                    DestructionObstacleSpawner.Instance.Play(transform.transform.position, Vector3.zero);
                    Destroy(transform.gameObject);
                    EffectController.Instance.DisableShieldEffect();
                    return;
                }
                
                CameraController.Instance.Shake();

                if (PlayerMover.Instance.CanCancelMove && !_playerController.IsCriticalCondition)
                {
                    PlayerMover.Instance.CancelMove();
                    CameraController.Instance.ApplyDamageEffect();
                    _playerController.IsStumble = true;

                }
                else
                {
                    _playerController.IsFall = true;
                    _playerController.OnGameOverEvent.Invoke();
                }
            }

            other.GetComponent<Pickup>()?.OnPickup();
        }
    }
}