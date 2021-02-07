using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolingEnemy : MonoBehaviour
{
    public Collider2D collisionCheck;
    
    private Rigidbody2D _rigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        var rigidbody2DVelocity = _rigidbody2D.velocity;
        rigidbody2DVelocity.x = -4f * transform.localScale.x;
        _rigidbody2D.velocity = rigidbody2DVelocity;

        if (collisionCheck.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            var transformLocalScale = transform.localScale;
            transformLocalScale.x *= -1f;
            transform.localScale = transformLocalScale;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.rigidbody.AddForce(((other.transform.position - transform.position).normalized + Vector3.up) * 20f, ForceMode2D.Impulse);
        }
    }
}
