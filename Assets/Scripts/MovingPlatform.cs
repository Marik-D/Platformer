using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

public class MovingPlatform : MonoBehaviour
{
    public Transform to;
    public bool lockY = true;
    public float speed = 4f;

    private Vector2 _initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        _initialPosition = transform.position;
    }
    
    // Update is called once per frame
    void Update()
    {
        var pathPosition = Time.time * speed;
        var pathLength = lockY ? Mathf.Abs(to.position.x - _initialPosition.x) : ((Vector2)to.position - _initialPosition).magnitude;
        var currentLoopIndex = Mathf.FloorToInt(pathPosition / (2 * pathLength));
        var currentPathPosition = pathPosition - currentLoopIndex * 2 * pathLength;
        if (currentPathPosition > pathLength)
        {
            currentPathPosition = 2 * pathLength - currentPathPosition;
        }

        var pos = (Vector3)Vector2.Lerp(_initialPosition, (Vector2) to.position, currentPathPosition / pathLength);
        if (lockY)
        {
            pos.y = _initialPosition.y;
        }

        pos.z = transform.position.z;
        transform.position = pos;
    }
}
