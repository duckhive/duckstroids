public class BulletPooler : ObjectPoolerManager
{
    public static BulletPooler Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else 
            Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
    }
}