using UnityEngine;
using UnityEngine.InputSystem;

public class SimplePlayerController : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    
    Camera _mainCamera;
    Rigidbody _rigidbody;
    InputAction _inputAction;

    Vector3 _movement;

    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = Camera.main;
        _rigidbody = GetComponent<Rigidbody>();
        PlayerInput _playerInput = GetComponent<PlayerInput>();
        _inputAction = _playerInput.actions["movement"];
    }

    // Update is called once per frame
    void Update()
    {
        //GetUserInput();
        RotateTowardsMouse();
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        var moveInput = _inputAction.ReadValue<Vector2>();
        _movement = new Vector3(moveInput.x, 0f, moveInput.y);
        
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
    private Vector3 RotateTowardsMouse()
    {
        Vector3 direction;

        Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue() /*Input.mousePosition*/);

        if (Physics.Raycast(ray, out var raycastHit, Mathf.Infinity))
        {
            direction = raycastHit.point - transform.position;
        }
        else
        {
            direction = ray.GetPoint(50) - transform.position;
        }

        direction = direction.normalized;
        direction.y = 0;
        transform.forward = direction;

        return direction;
    }
}
