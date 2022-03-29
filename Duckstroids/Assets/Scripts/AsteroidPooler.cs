using UnityEngine;

public class AsteroidPooler : ObjectPoolerManager
{
    public static AsteroidPooler Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else 
            Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
    }
    
    public override void ReturnObject(GameObject objectToReturn)
    {
        pool.Add(objectToReturn);
        objectToReturn.SetActive(false);
        objectToReturn.transform.localScale = Vector3.one;
        objectToReturn.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}