using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        MainMenu,
        Gameplay,
        GameOver
    }

    public static GameManager instance;

    public GameObject mainMenuUI;
    public GameObject gameplayUI;
    public GameObject gameOverUI;
    public GameObject pauseMenuUI;

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
        SetState(GameState.Gameplay);
    }

    public void GameOver()
    {
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
        bool isPaused = currentState == GameState.Gameplay && pauseMenuUI.activeSelf;
        pauseMenuUI.SetActive(!isPaused);
        Time.timeScale = isPaused ? 1f : 0f; // Pause or resume time based on menu state
        SetState(isPaused ? GameState.Gameplay : GameState.MainMenu);
    }

    private void SetState(GameState newState)
    {
        currentState = newState;

        mainMenuUI.SetActive(newState == GameState.MainMenu);
        gameplayUI.SetActive(newState == GameState.Gameplay);
        gameOverUI.SetActive(newState == GameState.GameOver);

        if (newState != GameState.Gameplay)
        {
            pauseMenuUI.SetActive(false);
        }
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
    }
}
