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
                Collider otherCollider = other.GetComponent<Collider>()!;
                
                HitEffectSpawner.Instance.Play(transform.position + new Vector3(0f, 4.0f, -1.0f), Vector3.zero);
                
                if (EffectController.Instance.ShieldEffectIsActive())
                {
                    DestructionObstacleSpawner.Instance.Play(otherCollider.transform.position, Vector3.zero);
                    Destroy(otherCollider.gameObject);
                    // EffectController.Instance.DisableShieldEffect();
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
                    _playerController.OnGameOverEvent?.Invoke();
                }

                otherCollider.enabled = false;
                other.transform.DOPunchPosition(Vector3.forward, 0.2f, 3).OnComplete(() =>
                {
                    otherCollider.enabled = true;
                });
                
                return;
            }

            if (other.CompareTag(nameof(Tags.CriticalObstacle)))
            {
                HitEffectSpawner.Instance.Play(transform.position + new Vector3(0f, 4.0f, -1.0f), Vector3.zero);
                Transform otherTransform = other.transform;
                
                if (EffectController.Instance.ShieldEffectIsActive())
                {
                    DestructionObstacleSpawner.Instance.Play(otherTransform.position, Vector3.zero);
                    Destroy(otherTransform.gameObject);
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