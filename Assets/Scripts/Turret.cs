using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform turretHead;
    public GameObject shellPrefab;
    public float aimRange = 20f;
    public float shotCooldown = 1.5f;
    
    private Transform _player;
    private float _lastShotTime;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player").transform;
    }
    
    // Update is called once per frame
    void Update()
    {
        var deltaPos = _player.position - turretHead.position;
        

        if (deltaPos.magnitude < aimRange)
        {
            turretHead.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.Cross(Vector3.forward, deltaPos));

            if (Time.time - _lastShotTime > shotCooldown)
            {
                _lastShotTime = Time.time;
                Instantiate(shellPrefab, turretHead.TransformPoint(Vector3.right * 2f), turretHead.rotation);
            }
        }
    }
}
