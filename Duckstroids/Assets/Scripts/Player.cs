using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float thrustSpeed = 5.0f;
    [SerializeField] private float turnMultiplier = 1.0f;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private float respawnCooldown = 3.0f;
    
    private bool _thrusting;
    private bool _turning;
    private float _turnDirection;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        gameObject.layer = LayerMask.NameToLayer("Ignore Collisions");
        Invoke(nameof(TurnOnCollision), respawnCooldown);
    }

    private void Update()
    {
        _thrusting = Input.GetAxis("Vertical") != 0;
        _turning = Input.GetAxis("Horizontal") != 0;
        _turnDirection = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Shoot") && !GameManager.Instance.paused)
        {
            Shoot();
            FeedbacksManager.Instance.shootGunFeedbacks.PlayFeedbacks();
        }
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.paused)
        {
            if(Input.GetAxis("Vertical") > 0)
                _rb.AddForce(transform.forward * thrustSpeed);
            if(Input.GetAxis("Vertical") < 0)
                _rb.AddForce(-transform.forward * thrustSpeed);
            if(_turnDirection != 0f)
                _rb.AddTorque(Vector3.up * _turnDirection * turnMultiplier, ForceMode.VelocityChange);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent<Asteroid>(out Asteroid asteroid))
        {
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
            
            gameObject.SetActive(false);
            
            GameManager.Instance.PlayerDied();
        }
    }
    
    private void Shoot()
    {
        var bullet = Instantiate(bulletPrefab, transform.position + transform.forward, transform.rotation);
        bullet.Project(transform.forward);
    }

    private void TurnOnCollision()
    {
        gameObject.layer = LayerMask.NameToLayer("Player");
    }
}