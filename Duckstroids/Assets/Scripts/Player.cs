using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float thrustSpeed = 5.0f;
    [SerializeField] private float turnMultiplier = 1f;
    [SerializeField] private Bullet bulletPrefab;
    
    private bool _thrusting;
    private bool _turning;
    private float _turnDirection;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _thrusting = Input.GetAxis("Vertical") != 0;
        _turning = Input.GetAxis("Horizontal") != 0;
        _turnDirection = Input.GetAxis("Horizontal");
        
        if(Input.GetButtonDown("Fire1"))
            Shoot();
    }

    private void FixedUpdate()
    {
        if(Input.GetAxis("Vertical") > 0)
            _rb.AddForce(transform.forward * thrustSpeed);
        if(Input.GetAxis("Vertical") < 0)
            _rb.AddForce(-transform.forward * thrustSpeed);
        if(_turnDirection != 0f)
            _rb.AddTorque(Vector3.up * _turnDirection * turnMultiplier, ForceMode.VelocityChange);
    }

    private void Shoot()
    {
        var bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bullet.Project(transform.forward);
    }
}
