using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] private Asteroid asteroidPrefab;
    [SerializeField] private float spawnRate = 2.0f;
    [SerializeField] private float spawnAmount = 1.0f;
    [SerializeField] private float spawnDistance = 15.0f;
    [SerializeField] private float trajectoryVariance = 15.0f;

    private float _timer;

    private void Start()
    {
        InvokeRepeating(nameof(Spawn), spawnRate, spawnRate);
    }

    private void Update()
    {
        if(GameManager.Instance.gameActive)
            SpawnAtDecreasingRate();
    }

    public void SpawnAtDecreasingRate()
    {
        _timer += Time.deltaTime;

        if (_timer >= 20 && spawnRate > 0.1f)
        {
            CancelInvoke();
            _timer = 0;
            spawnRate -= 0.1f;
            InvokeRepeating(nameof(Spawn), spawnRate, spawnRate);
        }
    }
    
    private void Spawn()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            var spawnDirection = Random.insideUnitCircle.normalized * spawnDistance;
            var spawnPoint = transform.position + new Vector3(spawnDirection.x, 0, spawnDirection.y);

            var variance = Random.Range(-trajectoryVariance, trajectoryVariance);
            var rotation = Quaternion.AngleAxis(variance, Vector3.up);
            
            var asteroid = Instantiate(asteroidPrefab, spawnPoint, rotation);
            asteroid.size = Random.Range(asteroid.minSize, asteroid.maxSize);
            asteroid.SetTrajectory(rotation * (new Vector3(-spawnDirection.x, 0, -spawnDirection.y)));

            var randomForce = new Vector3(Random.value, Random.value, Random.value);
            asteroid.rb.AddTorque(randomForce * 10, ForceMode.Impulse);
        }
    }
}