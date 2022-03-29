using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidSpawner : MonoBehaviour
{
    //[SerializeField] private Asteroid asteroidPrefab;
    [SerializeField] private float spawnRate = 2.0f;
    [SerializeField] private float spawnAmount = 1.0f;
    [SerializeField] private float spawnDistance = 15.0f;
    [SerializeField] private float trajectoryVariance = 15.0f;

    private float _spawnRateStart;
    private float _timer;
    

    private void Start()
    {
        InvokeRepeating(nameof(Spawn), spawnRate, spawnRate);
        _spawnRateStart = spawnRate;
    }

    private void Update()
    {
        if(GameManager.Instance.gameActive)
            SpawnAtDecreasingRate();
    }

    public void ResetSpawnRate()
    {
        spawnRate = _spawnRateStart;
    }
    
    private void SpawnAtDecreasingRate()
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

            var asteroidObject = AsteroidPooler.Instance.SpawnObject(spawnPoint, rotation);
            var asteroid = asteroidObject.GetComponent<Asteroid>();
            asteroid.size = Random.Range(asteroid.minSize, asteroid.maxSize);
            asteroid.SetTrajectory(rotation * (new Vector3(-spawnDirection.x, 0, -spawnDirection.y)));
            asteroid.rb.mass = asteroid.size;
            asteroidObject.transform.eulerAngles = new Vector3(0, Random.value * 360, 0);
            asteroidObject.transform.localScale = Vector3.one * asteroid.size;


            var randomForce = new Vector3(Random.value, Random.value, Random.value);
            asteroid.rb.AddTorque(randomForce * 10, ForceMode.Impulse);
        }
    }
}