using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float thrustSpeed = 5.0f;
    [SerializeField] private float turnMultiplier = 1f;
    
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

        if (!_turning)
            _rb.angularVelocity = Vector3.zero;

        /*if (Input.GetAxis("Horizontal") < 0)
            _turnDirection = -1;
        else if (Input.GetAxis("Horizontal") > 0)
            _turnDirection = 1;
        else
            _turnDirection = 0;*/

    }

    private void FixedUpdate()
    {
        if(Input.GetAxis("Vertical") > 0)
            _rb.AddForce(transform.forward * thrustSpeed);
        if(Input.GetAxis("Vertical") < 0)
            _rb.AddForce(-transform.forward * thrustSpeed);
        if(_turnDirection != 0f)
            _rb.AddTorque(transform.up * _turnDirection * turnMultiplier, ForceMode.VelocityChange);
    }
}
