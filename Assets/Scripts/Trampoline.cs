using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    public float cooldown = 1f;

    private float _lastJumpTime;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && Time.time - _lastJumpTime > cooldown)
        {
            var velocity = other.attachedRigidbody.velocity;
            velocity.y *= -1.1f;
            other.attachedRigidbody.velocity = velocity;
            _lastJumpTime = Time.time;
        }
    }
}
