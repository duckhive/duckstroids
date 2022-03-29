using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int lives = 3;
    public float respawnTime = 3.0f;

    public ParticleSystem vfxAsteroidExplosion;
    public ParticleSystem vfxPlayerDeathExplosion;

    public int score = 0;

    public GameObject uiGameOverPanel;
    public GameObject uiPausePanel;
    public GameObject uiHudPanel;
    public GameObject uiStartPanel;
    
    public TMP_Text uiScoreText;
    public TMP_Text uiLivesText;

    public bool gameActive;
    public bool paused;
    
    [HideInInspector] public Player player;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        player = FindObjectOfType<Player>();
        uiStartPanel.SetActive(true);
        uiHudPanel.SetActive(false);

        lives = 1000;
    }

    private void Update()
    {
        if(Input.GetButtonDown("Pause") && !paused)
            PauseGame();
        else if(Input.GetButtonDown("Pause") && paused)
            ResumeGame();
        
        if(Input.GetButtonDown("Pause") && !gameActive)
            ResetScene();
            
        
        
        uiScoreText.text = $"{score}";
        uiLivesText.text = $"x {lives}";
    }

    public void PlayerDied()
    {
        gameActive = false;
        FeedbacksManager.Instance.playerDeathFeedbacks.PlayFeedbacks();

        vfxPlayerDeathExplosion.transform.position = player.transform.position;
        vfxPlayerDeathExplosion.Play();
        
        lives--;

        if (lives <= 0)
            GameOver();
        else
            Invoke(nameof(Respawn), respawnTime);
    }

    public void AsteroidShot(Asteroid asteroid)
    {
        if (asteroid.size < 0.75f)
            score += 300;
        else if (asteroid.size < 1.15f)
            score += 200;
        else
            score += 100;
        
        FeedbacksManager.Instance.asteroidShotFeedbacks.PlayFeedbacks();
        
        vfxAsteroidExplosion.transform.position = asteroid.transform.position;
        vfxAsteroidExplosion.Play();
        
        AsteroidPooler.Instance.ReturnObject(asteroid.gameObject);
    }

    public void ResetScene()
    {
        lives = 3;
        score = 0;

        gameActive = true;
        uiGameOverPanel.SetActive(false);
        uiHudPanel.SetActive(true);
        uiStartPanel.SetActive(false);
        AsteroidPooler.Instance.ReturnAllAsteroids();
        FindObjectOfType<AsteroidSpawner>().ResetSpawnRate();
        
        Respawn();
    }
    
    public void PauseGame()
    {
        if (!gameActive) return;
        
        uiPausePanel.SetActive(true);
        paused = true;
        Time.timeScale = 0;
        
    }

    public void ResumeGame()
    {
        uiPausePanel.SetActive(false);
        paused = false;
        Time.timeScale = 1;
    }

    public void ContinueGame()
    {
        lives++;
        uiGameOverPanel.SetActive(false);
        gameActive = true;
        Respawn();
        AsteroidPooler.Instance.ReturnAllAsteroids();

    }

    public void Restart()
    {
        ResetScene();
        ResumeGame();
    }
    
    private void Respawn()
    {
        gameActive = true;
        player.transform.position = Vector3.zero;
        player.gameObject.SetActive(true);
    }

    private void GameOver()
    {
        uiGameOverPanel.SetActive(true);
        gameActive = false;

        FeedbacksManager.Instance.gameOverFeedbacks.PlayFeedbacks();
    }
}