using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float _acceleration = 15f;
    public float _breakAcceleration = 50f;
    private float _maxSpeed = 10f;
    
    private Rigidbody2D _rigidbody2D;

    private float _moveDirection = 0f;

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
    }

    private void FixedUpdate()
    {
        var acceleration = _moveDirection * _rigidbody2D.velocity.x > 0 ? _acceleration : _breakAcceleration;
        _rigidbody2D.AddForce(_moveDirection * acceleration * Vector2.right, ForceMode2D.Force);
        
        _rigidbody2D.velocity = new Vector2(Mathf.Clamp(_rigidbody2D.velocity.x, -_maxSpeed, _maxSpeed), _rigidbody2D.velocity.y);
    }
}
