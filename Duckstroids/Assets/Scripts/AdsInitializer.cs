using UnityEngine;
using UnityEngine.Advertisements;
 
public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
{
    public static AdsInitializer Instance;
    
    [SerializeField] private string androidGameId;
    [SerializeField] private string iOSGameId;
    [SerializeField] private bool testMode = true;
    [SerializeField] private RewardedAdsButton rewardedAdsButton;
    private string _gameId;
    
    
 
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        
        InitializeAds();
    }

    public void InitializeAds()
    {
#if UNITY_IOS
        _gameId = iOSGameId;
#elif UNITY_ANDROID
        _gameId = androidGameId;
#endif
        Advertisement.Initialize(_gameId, testMode, this);
    }

    public void OnInitializationComplete()
    {
        Debug.Log("ad initialization complete.");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }

    public void LoadAd()
    {
        rewardedAdsButton.LoadAd();
    }
}