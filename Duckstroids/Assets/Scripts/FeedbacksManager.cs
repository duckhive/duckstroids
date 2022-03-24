using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class FeedbacksManager : MonoBehaviour
{
    public static FeedbacksManager Instance;
    
    public MMFeedbacks asteroidShotFeedbacks;
    public MMFeedbacks playerDeathFeedbacks;
    public MMFeedbacks gameOverFeedbacks;
    public MMFeedbacks shootGunFeedbacks;
    public MMFeedbacks asteroidsBump;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
}
