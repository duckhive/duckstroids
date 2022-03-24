using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int lives = 3;
    public float respawnTime = 3.0f;
    
    [HideInInspector] public Player player;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        player = FindObjectOfType<Player>();
    }

    public void PlayerDied()
    {
        lives--;

        if (lives <= 0)
            GameOver();
        else
            Invoke(nameof(Respawn), respawnTime);
    }

    private void Respawn()
    {
        player.transform.position = Vector3.zero;
        player.gameObject.SetActive(true);
    }

    private void GameOver()
    {
        Debug.Log("Player Died");
    }
}