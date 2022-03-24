using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 500f;
    [SerializeField] private float maxLifetime = 10f;
    
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);
    }

    public void Project(Vector3 direction)
    {
        _rb.AddForce(direction * speed);
        
        Destroy(gameObject, maxLifetime);
    }
}
