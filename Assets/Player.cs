using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform _groundCheck;
    
    public float _acceleration = 40f;
    public float _runAcceleration = 60f;
    public float _breakAcceleration = 50f;
    public float _maxSpeed = 10f;
    public float _maxRunSpeed = 15f;
    public float _jumpImpulse = 20f;
    public float _dashCooldown = 1f;
    public float _dashDistance = 10f;
    public float _dashDuration = 0.2f;
    
    private Rigidbody2D _rigidbody2D;

    private float _moveDirection = 0f;
    private float _lastJumpTime;
    private float _lastDashTime;
    private float _dashTarget;
    private float _dashStart;

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
            Physics2D.OverlapCircle(_groundCheck.position, 0.1f, LayerMask.GetMask("Ground")) &&
            Time.time - _lastJumpTime > 0.2f
        )
        {
            _rigidbody2D.AddForce(new Vector2(_moveDirection, 1f) * _jumpImpulse, ForceMode2D.Impulse);
            _lastJumpTime = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.Space) && Time.time - _lastDashTime > _dashCooldown)
        {
            _lastDashTime = Time.time;
            _dashStart = transform.position.x;
            _dashTarget = _dashStart + _moveDirection * _dashDistance;
        }
    }

    private void FixedUpdate()
    {
        var baseAcceleration = Input.GetKey(KeyCode.LeftShift) ? _runAcceleration : _acceleration;
        var acceleration = _moveDirection * _rigidbody2D.velocity.x > 0 ? baseAcceleration : _breakAcceleration;
        _rigidbody2D.AddForce(_moveDirection * acceleration * Vector2.right, ForceMode2D.Force);

        var maxSpeed = Input.GetKey(KeyCode.LeftShift) ? _maxRunSpeed : _maxSpeed;
        
        _rigidbody2D.velocity = new Vector2(Mathf.Clamp(_rigidbody2D.velocity.x, -maxSpeed, maxSpeed), _rigidbody2D.velocity.y);

        if (Time.time - _lastDashTime < _dashDuration)
        {
            var pos = transform.position;
            pos.x = Mathf.Lerp(_dashStart, _dashTarget, (Time.time - _lastDashTime) / _dashDuration);
            transform.position = pos;
        }
    }
}
