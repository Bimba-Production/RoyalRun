using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.StateMachine
{
    public class PlayerMover : Singleton<PlayerMover>
    {
        [Header("References")]
        [SerializeField] private Rigidbody _rb;
        
        [Header("Settings")]
        [SerializeField] private float _groundedRayLength = 0.2f;

        private Vector2 _moveInput;
        public Vector2 MoveInput => _moveInput;
        public bool JumpInput { get; private set; }
        public bool SlidingInput { get; private set; }
        private bool ShiftRightInput { get; set; }
        private bool ShiftLeftInput { get; set; }
        public bool IsGrounded = false;

        private float[] _lanes = { -2.5f, 0f, 2.5f };
        private float _currentLane = 0f;
        private bool _isMoving = false;
        private bool _canRightMove = true;
        private bool _canLeftMove = true;
        private float _shiftDuration = 0.17f;
        private float _speedMultiplier = 2f;

        public bool CanCancelMove => _lastPosition != -1f;
        private float _lastPosition = -1f;
        private float _lastPositionTime = 0.2f;
        private float _lastPositionCooldownTime = 0f;
        
        public void ReadMoveInput(InputAction.CallbackContext context) => _moveInput = context.ReadValue<Vector2>(); 
        public void ReadJumpInput(InputAction.CallbackContext context) => JumpInput = context.ReadValueAsButton();
        public void ReadSlidingInput(InputAction.CallbackContext context) => SlidingInput = context.ReadValueAsButton();

        private void Update()
        {
            if (JumpInput)
            {
                _lastPosition = -1f;
                _lastPositionCooldownTime = 0;
            }
            
            if (_lastPosition == -1f) return;
            
            if (_lastPositionCooldownTime <= Time.realtimeSinceStartup)
            {
                _lastPositionCooldownTime = 0;
                _lastPosition = -1f;
            }
        }
        
        public void ReadShiftRightInput(InputAction.CallbackContext context)
        {
            ShiftRightInput = context.ReadValueAsButton();
            if (!_canRightMove && !ShiftRightInput) _canRightMove = true;
        }

        public void ReadShiftLeftInput(InputAction.CallbackContext context)
        {
            ShiftLeftInput = context.ReadValueAsButton();
            if (!_canLeftMove && !ShiftLeftInput) _canLeftMove = true;
        }

        public void Move()
        {
            if (_isMoving) return;
            if (!_canLeftMove || !_canRightMove) return;

            float newPos = _currentLane;
            
            if (ShiftRightInput && newPos < _lanes[_lanes.Length - 1])
            {
                _canRightMove = false;
                newPos += 2.5f;
            }

            if (ShiftLeftInput && newPos > _lanes[0])
            {
                _canLeftMove = false;
                newPos -= 2.5f;
            }

            if (newPos != _currentLane)
            {
                _lastPosition = _currentLane;
                _lastPositionCooldownTime = Time.realtimeSinceStartup + _lastPositionTime;
                float speed = EffectController.Instance.ElectricalEffectIsActive() ? _shiftDuration / _speedMultiplier : _shiftDuration;
                StartCoroutine(ApplyMoveRoutine(newPos, speed));
            }
        }

        public void CancelMove()
        {
            StopCoroutine(nameof(ApplyMoveRoutine));
            StartCoroutine(ApplyMoveRoutine(_lastPosition, 0.14f));
        }
        
        private IEnumerator ApplyMoveRoutine(float targetPos, float duration = 0.2f)
        {
            _isMoving = true;
            float startPos = _currentLane;

            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                elapsedTime += Time.deltaTime;
                float newPosX = Mathf.Lerp(startPos, targetPos, t);
                transform.position = new Vector3(newPosX, transform.position.y, transform.position.z);
                yield return null;
            }

            _currentLane = targetPos;
            transform.position = new Vector3(targetPos, transform.position.y, transform.position.z);
            _isMoving = false;
        }

        public void ApplyForce(Vector3 direction, float force) => _rb.AddForce(transform.forward + direction * force, ForceMode.Impulse);

        public void UpdateIsGrounded()
        {
            IsGrounded = Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z), Vector3.down, _groundedRayLength);
        }
    }
}