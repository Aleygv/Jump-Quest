using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerInput : MonoBehaviour
{
    [Header("Movement Settings")] [SerializeField]
    private float moveSpeed = 10f;

    [SerializeField] private float jumpHeight = 0.5f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;

    [Header("Input Actions")] [SerializeField]
    private InputActionReference moveAction;
    [SerializeField] private InputActionReference jumpAction;

    private Rigidbody _rb;
    private Vector2 _moveInput;
    private float _jumpVelocity;
    private bool _isGrounded;
    private bool _canWallJump;

    //Test variables
    private bool _canMove = true;
    private bool _wasGrounded;
    
    private bool _isWallSliding;
    private float _wallSlideSpeed = 2f;
    
    private float _gravityScaleFall = 15f;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        float gravity = Physics.gravity.y;
        _jumpVelocity = Mathf.Sqrt(-2f * gravity * jumpHeight);
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
        
        if (IsReachedJumpHeight())
        {
            _rb.AddForce(new Vector3(_rb.linearVelocity.x, -_gravityScaleFall) * _rb.mass, ForceMode.Acceleration);
        }
        
        WallSlide();
        FLipDirection();
    }

    private void FixedUpdate()
    {
        CheckGround(); // Всегда проверяем землю
    
        if (_canMove)
        {
            Move();
        }
    }

    private void CheckGround()
    {
        _wasGrounded = _isGrounded;
        _isGrounded = Physics.OverlapSphere(groundCheck.position, groundCheckRadius, groundLayer).Length > 0;

        if (_isGrounded && !_wasGrounded)
        {
            _canMove = true;
        }
    }

    private bool IsWalled()
    {
        return Physics.OverlapSphere(wallCheck.position, 0.2f, wallLayer).Length > 0;
    }
    
    private bool IsReachedJumpHeight()
    {
        return _rb.position.y >= _rb.position.y + jumpHeight;
    }

    private void WallSlide()
    {
        if (IsWalled() && !_isGrounded && _rb.linearVelocity.y != 0)
        {
            if (_rb.linearVelocity.y > 0)
            {
                _rb.AddForce(new Vector3(_rb.linearVelocity.x, -_gravityScaleFall) * _rb.mass, ForceMode.Acceleration);
            }
            _isWallSliding = true;
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, Mathf.Clamp(_rb.linearVelocity.y, -_wallSlideSpeed, float.MaxValue));
        }
        else
        {
            _isWallSliding = false;
        }
    }
    
    private void FLipDirection()
    {
        if (_moveInput.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (_moveInput.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void Move()
    {
        Vector3 movement = new Vector3(_moveInput.x, 0, 0) * moveSpeed;
        _rb.linearVelocity = new Vector3(movement.x, _rb.linearVelocity.y, 0);
    }
    
    private void Jump(float force)
    {
        _rb.AddForce(new Vector3(_rb.linearVelocity.x, force) * _rb.mass, ForceMode.Impulse);
        
    }
}