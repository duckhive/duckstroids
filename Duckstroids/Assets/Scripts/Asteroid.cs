using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Asteroid : MonoBehaviour
{
    public float size = 1.0f;
    public float minSize = 0.5f;
    public float maxSize = 1.5f;

    public float speed = 50.0f;
    public float maxLifetime = 30.0f;

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        transform.eulerAngles = new Vector3(0, Random.value * 360, 0);
        transform.localScale = Vector3.one * size;

        _rb.mass = size;
    }

    public void SetTrajectory(Vector3 direction)
    {
        _rb.AddForce(direction * speed);
        
        Destroy(gameObject, maxLifetime);
    }
}