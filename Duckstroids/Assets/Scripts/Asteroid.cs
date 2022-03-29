using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float splitSpeed = 3.0f;
    
    public float size = 1.0f;
    public float minSize = 0.5f;
    public float maxSize = 1.5f;

    public float speed = 50.0f;
    public float maxLifetime = 30.0f;

    [HideInInspector] public Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        transform.eulerAngles = new Vector3(0, Random.value * 360, 0);
        transform.localScale = Vector3.one * size;

        rb.mass = size;
    }

    public void SetTrajectory(Vector3 direction)
    {
        rb.AddForce(direction * speed);

        StartCoroutine(SendToPoolAfterDelay(30));
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent<Bullet>(out Bullet bullet))
        {
            if((size * 0.5f) >= minSize)
            {
                CreateSplit();
                CreateSplit();
            }
            
            GameManager.Instance.AsteroidShot(this);
        }

        if (other.gameObject.TryGetComponent<Asteroid>(out Asteroid asteroid))
        {
            FeedbacksManager.Instance.asteroidsBump.PlayFeedbacks();
        }
    }

    private void CreateSplit()
    {
        var pos = transform.position;
        pos += (new Vector3(Random.insideUnitCircle.x, 0, Random.insideUnitCircle.y) * 0.5f);

        var asteroidObject = AsteroidPooler.Instance.SpawnObject(pos, transform.rotation);
        var half = asteroidObject.GetComponent<Asteroid>();
        //var half = Instantiate(this, pos, transform.rotation);
        half.size = size * 0.5f;
        asteroidObject.transform.eulerAngles = new Vector3(0, Random.value * 360, 0);
        asteroidObject.transform.localScale = Vector3.one * half.size;
        half.SetTrajectory(new Vector3(Random.insideUnitCircle.normalized.x, 0, Random.insideUnitCircle.normalized.y) * splitSpeed);
        
        var randomForce = new Vector3(Random.value, Random.value, Random.value);
        half.rb.AddTorque(randomForce * 10, ForceMode.Impulse);
    }

    private IEnumerator SendToPoolAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        AsteroidPooler.Instance.ReturnObject(gameObject);
    }
}