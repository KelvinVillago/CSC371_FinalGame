using UnityEngine;
using UnityEngine.InputSystem;

public class SimplePlayerController : MonoBehaviour
{
    [SerializeField] float speed = 5f;

    Rigidbody _rigidbody;
    InputAction _inputAction;

    Vector3 _movement;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        PlayerInput _playerInput = GetComponent<PlayerInput>();
        _inputAction = _playerInput.actions["movement"];
    }

    // Update is called once per frame
    void Update()
    {
        GetUserInput();
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        _movement = _movement * speed;
        Vector3 newVelocity = new Vector3(_movement.x, _rigidbody.velocity.y, _movement.z);
        _rigidbody.velocity = _movement;
    }

    void GetUserInput()
    {
        var moveInput = _inputAction.ReadValue<Vector2>();
        _movement = new Vector3(moveInput.x, 0f, moveInput.y);
        if (_movement != Vector3.zero) transform.forward = _movement.normalized;
    }
}
