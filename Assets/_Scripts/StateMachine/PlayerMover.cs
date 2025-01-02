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
        public bool IsGraunded = false;

        public void ReadMoveInput(InputAction.CallbackContext context) => _moveInput = context.ReadValue<Vector2>(); 
        public void ReadJumpInput(InputAction.CallbackContext context)
        {
            JumpInput = context.ReadValueAsButton();
        }
        public void ReadSlidingInput(InputAction.CallbackContext context) => SlidingInput = context.ReadValueAsButton();

        public void Move(float minX, float maxX, float speed)
        {
            float velocityX = _moveInput.x * (speed * Time.deltaTime);
            float newPosX = transform.position.x + velocityX;
            Vector3 newPos = transform.position;

            if (newPosX > minX && newPosX < maxX) newPos = new Vector3(newPosX, newPos.y, newPos.z);

            transform.position = newPos;
        }

        public void ApplyForce(Vector3 direction, float force) => 
            _rb.AddForce(transform.forward + direction * force, ForceMode.Impulse);

        public void UpdateIsGraunded()
        {
            IsGraunded = Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z), Vector3.down, _groundedRayLength);
            Debug.DrawRay(transform.position, Vector3.down * _groundedRayLength, IsGraunded ? Color.green : Color.red);
        }
    }
}