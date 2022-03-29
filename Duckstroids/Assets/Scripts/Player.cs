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
    [SerializeField] private float rotSpeed = 1f;
    
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
        if(SimpleInput.GetAxisRaw("Horizontal") != 0 || SimpleInput.GetAxisRaw("Vertical") != 0)
            Rotate();
        
        if (SimpleInput.GetButtonDown("Shoot"))
            Shoot();
        
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.paused)
        {
            if(SimpleInput.GetAxis("Vertical") > 0)
                _rb.AddForce(Vector3.forward * thrustSpeed);
            if(SimpleInput.GetAxis("Vertical") < 0)
                _rb.AddForce(-Vector3.forward * thrustSpeed);
            if(SimpleInput.GetAxis("Horizontal") < 0)
                _rb.AddForce(-Vector3.right * thrustSpeed);
            if(SimpleInput.GetAxis("Horizontal") > 0)
                _rb.AddForce(Vector3.right * thrustSpeed);
            
            
            //if(_turnDirection != 0f)
            //    _rb.AddTorque(Vector3.up * _turnDirection * turnMultiplier, ForceMode.VelocityChange);
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
    
    public void Shoot()
    {
        if (!GameManager.Instance.paused && GameManager.Instance.gameActive)
        {
            var bulletObject =
                BulletPooler.Instance.SpawnObject(transform.position + transform.forward, transform.rotation);
            var bullet = bulletObject.GetComponent<Bullet>();
            bullet.Project(transform.forward);
            FeedbacksManager.Instance.shootGunFeedbacks.PlayFeedbacks();
        }
    }

    private void TurnOnCollision()
    {
        gameObject.layer = LayerMask.NameToLayer("Player");
    }

    private void Rotate()
    {
        var x = SimpleInput.GetAxisRaw("Horizontal");
        var z = SimpleInput.GetAxisRaw("Vertical");
        var move = new Vector3(x, 0, z);
        var angle = Mathf.Atan2(move.z, move.x) * Mathf.Rad2Deg;
        transform.forward = move;
    }
}