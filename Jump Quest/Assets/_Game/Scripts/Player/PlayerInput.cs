using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerInput : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float jumpDuration = 0.3f; // Время, в течение которого можно прыгнуть от стены
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private Transform groundCheck;

    [Header("Input Actions")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference jumpAction;

    private Rigidbody _rb;
    private Vector2 _moveInput;
    private float _jumpVelocity;
    private bool _isGrounded;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        float gravity = Physics.gravity.y;
        _jumpVelocity = Mathf.Abs(gravity) * jumpDuration + Mathf.Sqrt(2 * jumpHeight * Mathf.Abs(gravity));
    }

    private void OnEnable()
    {
        moveAction.action.Enable();
        jumpAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        jumpAction.action.Disable();
    }

    private void Update()
    {
        _moveInput = moveAction.action.ReadValue<Vector2>();

        if (jumpAction.action.WasPressedThisFrame())
        {
            if (_isGrounded)
            {
                Jump(_jumpVelocity);
            }
        }
    }

    private void FixedUpdate()
    {
        Move();
        CheckGround();
    }

    private void CheckGround()
    {
        _isGrounded = Physics.OverlapSphere(groundCheck.position, groundCheckRadius, groundLayer).Length > 0;
    }

    private void Move()
    {
        Vector3 movement = new Vector3(_moveInput.x, 0, 0) * moveSpeed;
        _rb.linearVelocity = new Vector3(movement.x, _rb.linearVelocity.y, 0);
    }

    private void Jump(float force)
    {
        _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, force, 0);
    }
}