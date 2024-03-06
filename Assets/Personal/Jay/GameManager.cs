using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public string GamePlaySceneName;
    public string GameOverSceneName;
    public string MainMenuSceneName;
    public enum GameState
    {
        MainMenu,
        Gameplay,
        GameOver
    }

    public static GameManager instance;

    public Text enemyDeathCountText;

    private int enemyDeathCount;
    private GameState currentState;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SetState(GameState.MainMenu);
    }

    void Update()
    {
        if (currentState == GameState.Gameplay && Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(GamePlaySceneName);
    }

    public void GameOver()
    {
        SceneManager.LoadScene(GameOverSceneName);
        SetState(GameState.GameOver);
        UpdateGameOverUI();
    }

    public void IncreaseEnemyDeathCount()
    {
        enemyDeathCount++;
        UpdateEnemyDeathCountText();
    }

    public void TogglePauseMenu()
    {
        bool isPaused = currentState == GameState.Gameplay && Time.timeScale == 0f;
        Time.timeScale = isPaused ? 1f : 0f; // Pause or resume time based on menu state
        SetState(isPaused ? GameState.Gameplay : GameState.MainMenu);
    }

    private void SetState(GameState newState)
    {
        currentState = newState;
    }

    private void UpdateEnemyDeathCountText()
    {
        if (enemyDeathCountText != null)
        {
            enemyDeathCountText.text = "Enemies Defeated: " + enemyDeathCount;
        }
    }

    private void UpdateGameOverUI()
    {
        UpdateEnemyDeathCountText();
        // Add additional game over UI updates here if needed
    }
}