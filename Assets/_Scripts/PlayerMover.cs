using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float _groundedRayLength = 0.2f;

    private Rigidbody _rb;

    private Vector2 _moveInput;
    private bool _jumpInput;
    private bool _slidingInput;

    public Vector2 MoveInput => _moveInput;
    public bool JumpInput => _jumpInput;
    public bool SlidingInput => _slidingInput;
    public bool IsGraunded = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void ReadMoveInput(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }
    
    public void ReadJumpInput(InputAction.CallbackContext context)
    {
        _jumpInput = context.action.IsPressed();
    }
    
    public void ReadSlidingInput(InputAction.CallbackContext context)
    {
        _slidingInput = context.action.IsPressed();
    }
    
    public void AplyGravity(float currentGravityForce)
    {
        Vector3 velocity = new Vector3(0, -currentGravityForce * Time.deltaTime, 0);

        Debug.Log(transform.position.y);
        Debug.Log(velocity.y);

        if (transform.position.y <= 0.3 && _rb.linearVelocity.y < 0)
        {
            _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z);
        } else
        {
            _rb.linearVelocity += velocity;
        }
    }

    public void Move(float minX, float maxX, float speed)
    {
        float velocityX = _moveInput.x * (speed * Time.deltaTime);
        float newPosX = transform.position.x + velocityX;

        Vector3 newPos = transform.position;

        if (newPosX > minX && newPosX < maxX)
        {
            newPos = new Vector3(newPosX, newPos.y, newPos.z);
        }

        transform.position = newPos;
    }

    public void ResetYPos()
    {
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z);
    }

    public void AppluForce(float force)
    {
        _rb.AddForce(new Vector3(_rb.linearVelocity.x, force, _rb.linearVelocity.z), ForceMode.Impulse);
    }

    public void UpdateIsGraunded()
    {
        IsGraunded = Physics.Raycast(new Vector3(transform.position.x, transform.position.y < 0 ? 0 : transform.position.y, transform.position.z), Vector3.down, _groundedRayLength);
        Debug.DrawRay(transform.position, Vector3.down * _groundedRayLength, IsGraunded ? Color.green : Color.red);
    }
}
