using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.StateMachine
{
    public class PlayerMover : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Rigidbody _rb;
        
        [Header("Settings")]
        [SerializeField] private float _groundedRayLength = 0.2f;

        private Vector2 _moveInput;
        public Vector2 MoveInput => _moveInput;
        public bool JumpInput { get; private set; }
        public bool SlidingInput { get; private set; }
        public bool ShiftRightInput { get; private set; }
        public bool ShiftLeftInput { get; private set; }
        public bool IsGraunded = false;

        private float[] _lanes = { -2.5f, 0f, 2.5f };
        private float _currentLane = 0f;
        private bool _isMoving = false;
        private float _shiftDuration = 0.15f;

        public void ReadMoveInput(InputAction.CallbackContext context) => _moveInput = context.ReadValue<Vector2>(); 
        public void ReadJumpInput(InputAction.CallbackContext context) => JumpInput = context.ReadValueAsButton();
        public void ReadSlidingInput(InputAction.CallbackContext context) => SlidingInput = context.ReadValueAsButton();
        public void ReadShiftRightInput(InputAction.CallbackContext context) => ShiftRightInput = context.ReadValueAsButton();
        public void ReadShiftLeftInput(InputAction.CallbackContext context) => ShiftLeftInput = context.ReadValueAsButton();

        public void Move(float minX, float maxX, float speed)
        {
            if (_isMoving) return;

            float newPos = _currentLane;

            if (ShiftRightInput && newPos < _lanes[_lanes.Length - 1]) newPos += 2.5f;
            if (ShiftLeftInput && newPos > _lanes[0]) newPos -= 2.5f;

            if (newPos != _currentLane)
            {
                StartCoroutine(ApplyDamageEffectRoutine(newPos));
            }

            //float velocityX = _moveInput.x * (speed * Time.deltaTime);
            //float newPosX = transform.position.x + velocityX;
            //Vector3 newPos = transform.position;

            //if (newPosX > minX && newPosX < maxX) transform.position = new Vector3(newPosX, newPos.y, newPos.z);
        }

        IEnumerator ApplyDamageEffectRoutine(float targetPos)
        {
            _isMoving = true;
            float startPos = _currentLane;

            float elapsedTime = 0f;
            while (elapsedTime < _shiftDuration)
            {
                float t = elapsedTime / _shiftDuration;
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

        public void UpdateIsGraunded()
        {
            IsGraunded = Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z), Vector3.down, _groundedRayLength);
            Debug.DrawRay(transform.position, Vector3.down * _groundedRayLength, IsGraunded ? Color.green : Color.red);
        }
    }
}