using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Transform groundCheck;
    
    [Header("Basic movement")]
    public float baseAcceleration = 40f;
    public float runAccelerationMultiplier = 1.5f;
    public float breakAccelerationMultiplier = 1.5f;
    public float inAirAccelerationMultiplier = 0.3f;
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
    private TrailRenderer _trailRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _trailRenderer = GetComponent<TrailRenderer>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -50f)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
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
            IsGrounded() &&
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
            _trailRenderer.Clear();
            _trailRenderer.enabled = true;
        }
    }

    private Collider2D IsGrounded() => Physics2D.OverlapCircle(groundCheck.position, 0.1f, LayerMask.GetMask("Ground"));

    private void FixedUpdate()
    {
        var acceleration = baseAcceleration;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            acceleration *= runAccelerationMultiplier;
        }
        if (_moveDirection * _rigidbody2D.velocity.x < 0)
        {
            acceleration *= breakAccelerationMultiplier;
        }
        if (!IsGrounded())
        {
            acceleration *= inAirAccelerationMultiplier;
        }
        _rigidbody2D.AddForce(_moveDirection * acceleration * Vector2.right, ForceMode2D.Force);

        if (IsGrounded())
        {
            var finalMaxSpeed = Input.GetKey(KeyCode.LeftShift) ? maxRunSpeed : maxSpeed;
            _rigidbody2D.velocity = new Vector2(Mathf.Clamp(_rigidbody2D.velocity.x, -finalMaxSpeed, finalMaxSpeed), _rigidbody2D.velocity.y);
        }

        if (Time.time - _lastDashTime < dashDuration)
        {
            var vel = _rigidbody2D.velocity;
            vel.x = _dashVelocity;
            _rigidbody2D.velocity = vel;
        } 
        else if (_inDash)
        {
            _inDash = false;
            _trailRenderer.enabled = false;
            var vel = _rigidbody2D.velocity;
            vel.x = _originalVelocity;
            _rigidbody2D.velocity = vel;
        }
    }
    
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Moving Platform"))
        {
            var force = (other.rigidbody.velocity - _rigidbody2D.velocity) * 0.7f;
            _rigidbody2D.AddForce(force);
        }
    }
}
