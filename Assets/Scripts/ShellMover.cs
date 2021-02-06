using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellMover : MonoBehaviour
{
    public float speed;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        transform.position += transform.right * (speed * Time.deltaTime);
    }
}
