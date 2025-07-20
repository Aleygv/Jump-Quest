using System;
using UnityEngine;


public class PlayerJump : MonoBehaviour
{
    [Header("Jump stats")]
    [SerializeField] private float maxJumpTime;
    [SerializeField] private float maxJumpHeight;
    private float _startJumpVelocity;
    
    [Header("Character Components")]
    private PlayerInput _playerInput;
    private CharacterController _characterController;

    private void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _characterController = GetComponent<CharacterController>();
        
    }
}
