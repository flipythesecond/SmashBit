using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
   
    public static GameManager instance;

    public int playerScore;
    public int enemyScore;

    public int maxRounds = 10;
    private int currentRound = 1;

    public TextMeshProUGUI scoreText;

    public GameObject winScreen;
    public GameObject loseScreen;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        
        UpdateScoreUI();
    }

    public void PlayerWonRound()
    {
        playerScore++;
        NextRound();
    }

    public void EnemyWonRound()
    {
        enemyScore++;
        NextRound();
    }

    void NextRound()
    {
        currentRound++;

        UpdateScoreUI();

        if (currentRound > maxRounds)
        {
            EndGame();
        }
        else
        {
            ResetRound();
        }
    }

    void UpdateScoreUI()
    {
        scoreText.text = playerScore + " - " + enemyScore;
    }

    void EndGame()
    {
        if (playerScore > enemyScore)
        {
            winScreen.SetActive(true);
        }
        else
        {
            loseScreen.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    void ResetRound()
    {
        PlayerController player = FindFirstObjectByType<PlayerController>();
        EnemyBehaviour enemy = FindFirstObjectByType<EnemyBehaviour>();

        if (player != null)
        {
            player.transform.position = new Vector3(-3f, 1f, 0f);
            player.currentHealth = player.maxHealth;
        }

        if (enemy != null)
        {
            enemy.transform.position = new Vector3(3f, 1f, 0f);
            enemy.currentHealth = enemy.maxHealth;
        }

        if (player != null)
        {
            player.UpdateHealthSlider();
        }           
        if (enemy != null)
        {
            enemy.UpdateHealthSlider();
        }
    }
}

