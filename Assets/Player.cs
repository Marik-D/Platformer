using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform groundCheck;
    
    [Header("Basic movement")]
    public float acceleration = 40f;
    public float runAcceleration = 60f;
    public float breakAcceleration = 50f;
    public float maxSpeed = 10f;
    public float maxRunSpeed = 15f;
    public float jumpImpulse = 20f;
    
    [Header("Dash")]
    public float dashCooldown = 1f;
    public float dashDistance = 10f;
    public float dashDuration = 0.2f;
    
    private Rigidbody2D _rigidbody2D;

    private float _moveDirection;
    private float _lastJumpTime;
    private float _lastDashTime;
    private float _dashVelocity;
    private float _originalVelocity;
    private bool _inDash;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            _moveDirection = -1f;
        } 
        else if (Input.GetKeyDown(KeyCode.D))
        {
            _moveDirection = 1f;
        }
        
        if (Input.GetKeyUp(KeyCode.A) && _moveDirection == -1f)
        {
            _moveDirection = 0;
        } 
        else if (Input.GetKeyUp(KeyCode.D) && _moveDirection == 1f)
        {
            _moveDirection = 0f;
        }

        if (Input.GetKeyDown(KeyCode.W) &&
            Physics2D.OverlapCircle(groundCheck.position, 0.1f, LayerMask.GetMask("Ground")) &&
            Time.time - _lastJumpTime > 0.2f
        )
        {
            _rigidbody2D.AddForce(new Vector2(_moveDirection, 1f) * jumpImpulse, ForceMode2D.Impulse);
            _lastJumpTime = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.Space) && Time.time - _lastDashTime > dashCooldown)
        {
            _lastDashTime = Time.time;
            _dashVelocity = _moveDirection * (dashDistance / dashDuration);
            _originalVelocity = _rigidbody2D.velocity.x;
            _inDash = true;
        }
    }

    private void FixedUpdate()
    {
        var baseAcceleration = Input.GetKey(KeyCode.LeftShift) ? runAcceleration : acceleration;
        var finalAcceleration = _moveDirection * _rigidbody2D.velocity.x > 0 ? baseAcceleration : breakAcceleration;
        _rigidbody2D.AddForce(_moveDirection * finalAcceleration * Vector2.right, ForceMode2D.Force);

        var finalMaxSpeed = Input.GetKey(KeyCode.LeftShift) ? maxRunSpeed : maxSpeed;
        
        _rigidbody2D.velocity = new Vector2(Mathf.Clamp(_rigidbody2D.velocity.x, -finalMaxSpeed, finalMaxSpeed), _rigidbody2D.velocity.y);

        if (Time.time - _lastDashTime < dashDuration)
        {
            var vel = _rigidbody2D.velocity;
            vel.x = _dashVelocity;
            _rigidbody2D.velocity = vel;
        } 
        else if (_inDash)
        {
            _inDash = false;
            var vel = _rigidbody2D.velocity;
            vel.x = _originalVelocity;
            _rigidbody2D.velocity = vel;
        }
    }
}
