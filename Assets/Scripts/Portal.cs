using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Vector2 normal;
    public Portal linkedPortal;
    
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
        if (other.gameObject.CompareTag("Player") && linkedPortal != null)
        {
            var localVelocity = transform.InverseTransformVector(other.attachedRigidbody.velocity);
            
            if (Vector3.Dot(Vector3.up, localVelocity) < 0f) // moving into portal
            {
                Debug.Log("portal");
                other.transform.position =
                    linkedPortal.transform.TransformPoint(transform.InverseTransformPoint(other.transform.position));
                other.attachedRigidbody.velocity = linkedPortal.transform.TransformVector(-localVelocity);
            }
        }
    }
}
