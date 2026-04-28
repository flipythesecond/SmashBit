using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // singleton instance
    public static GameManager instance;
    // scores
    public int playerScore;
    public int enemyScore;
    // max rounds
    public int maxRounds = 10;
    private int currentRound = 1;
    // UI references
    public TextMeshProUGUI scoreText;
    // end game screens
    public GameObject winScreen;
    public GameObject loseScreen;

    // make sure this is the only instance of GameManager
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // persist across scenes
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // if another instance exists, destroy this one
            Destroy(gameObject);
        }
    }


    void Start()
    {
        // initialize scores and UI
        UpdateScoreUI();
    }
    // called when player wins a round
    public void PlayerWonRound()
    {
        playerScore++;
        NextRound();
    }
    // called when enemy wins a round
    public void EnemyWonRound()
    {
        enemyScore++;
        NextRound();
    }
    // advances to the next round or ends the game if max rounds reached
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
    // updates the score display on the UI
    void UpdateScoreUI()
    {
        scoreText.text = playerScore + " - " + enemyScore;
    }
    // ends the game and shows win/lose screen based on final scores
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
    // resets player and enemy positions and health for the next round
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
        // update health sliders after resetting health
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

