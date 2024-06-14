using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    private float _stopMove = 0; 
    private float _startMove = 4;

    private Rigidbody2D _rigidbody;
    private Vector2 _movementInput;
    private Vector2 _smoothedMovementInput;
    private Vector2 _movementInputSmoothVelocity;

    public bool canMove;
    public static PlayerMovement Instance;

    public Animator animator;
    public GameObject playerSprite;
    bool facingLeft = true;

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
        

        float playerSpeed = setSpeedFloat(_rigidbody.velocity.magnitude);
        animator.SetFloat("Speed", playerSpeed); //change animation to running depending on speed 

        //Debug.Log($"Player speed: {_rigidbody.velocity.x}");

        if(!canMove)
        {
            _speed = _stopMove;
        }
        else if(canMove)
        {
            _speed = _startMove;
        }


        if (_rigidbody.velocity.x < 0 && !facingLeft) //if walking to the left but facing right
        {
            Flip();
        }
        if (_rigidbody.velocity.x > 0 && facingLeft) //if walking to the right but is facing left
        {
            Flip();
        }

    }

    private float setSpeedFloat(float speed) 
    {
        if (speed > 2)
        {
            return 4;
        }
        else
        {
            return 0;
        }
        
    }

    private void Flip() //method to flip character
    {
        Vector3 currentScale = playerSprite.transform.localScale;
        currentScale.x *= -1;
        playerSprite.transform.localScale = currentScale;

        facingLeft = !facingLeft;
    }

    //getting input
    private void OnMove(InputValue inputValue)
    {
        _movementInput = inputValue.Get<Vector2>();
        
    }
}
