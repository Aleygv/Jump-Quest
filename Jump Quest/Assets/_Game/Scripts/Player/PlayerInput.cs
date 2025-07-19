using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public Rigidbody _rb;
    public float moveSpeed = 5f;
    public float jumpForce = 50f;
    private Vector2 _moveDirection;

    public InputActionReference move;
    public InputActionReference jump;
    
    private bool _isGrounded;

    private void Start()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, 1f);
    }

    private void Update()
    {
        _moveDirection = move.action.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = new Vector3(_moveDirection.x * moveSpeed * Time.deltaTime,
            _moveDirection.y * moveSpeed * Time.deltaTime, 0f);
    }

    private void OnEnable()
    {
        jump.action.started += Jump;
    }
    
    private void Jump(InputAction.CallbackContext obj)
    {
        if (Physics.Raycast(transform.position, Vector3.down, 1f))
        {
            Physics.gravity = Vector3.Lerp(Physics.gravity, new Vector3(0, -120f, 0), Time.deltaTime);
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        else
        {
            Physics.gravity = new Vector3(0, -9.81f, 0);
        }
    }

    private void OnDisable()
    {
        jump.action.started -= Jump;
    }
}