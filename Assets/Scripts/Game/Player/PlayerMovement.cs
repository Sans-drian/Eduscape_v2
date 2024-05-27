using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //[SerializeField]
    private float _speed;

    private float _stopMove = 0; 
    private float _startMove = 8;

    private Rigidbody2D _rigidbody;
    private Vector2 _movementInput;
    private Vector2 _smoothedMovementInput;
    private Vector2 _movementInputSmoothVelocity;

    public bool canMove;
    public static PlayerMovement Instance;

    private void Awake()
    {
        Instance = this;

        _speed = _startMove;

        _rigidbody = GetComponent<Rigidbody2D>();
        canMove = true;
    }

    private void FixedUpdate() 
    {

        _smoothedMovementInput = Vector2.SmoothDamp(
            _smoothedMovementInput,
            _movementInput,
            ref _movementInputSmoothVelocity,
            0.1f
        );
        _rigidbody.velocity = _smoothedMovementInput * _speed; //move x axis and y axis with set speeds

        if(!canMove)
        {
            _speed = _stopMove;
        }
        else if(canMove)
        {
            _speed = _startMove;
        }

    }

    //getting input
    private void OnMove(InputValue inputValue)
    {

        _movementInput = inputValue.Get<Vector2>();
        
    }
}
