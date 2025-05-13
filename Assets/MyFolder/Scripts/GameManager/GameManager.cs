using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState
    {
        Title,
        Countdown,
        Playing,
        GameOver,
        Result,
    }

    public GameState CurrentState { get; private set; }

    [Header("UI Panels")]
    public GameObject titlePanel;
    public GameObject countdownPanel;
    public GameObject gameplayPanel;
    public GameObject resultPanel;

    [Header("Countdown Settings")]
    public TMP_Text countdownText;
    public int countdownTime = 3;

    [Header("Game Timer")]
    public float gameDuration = 60f;
    public float gameTimer = 0f;
    public TMP_Text gameTimerText;

    [Header("External Controllers")]
    public ResultManager resultManager;
    public SpawnManager spawnManager;
    public EnemySpawner enemySpawner;

    [Header("Start Buttons")]
    public Button startButton;

    [Header("Result Buttons")]
    public Button restartButton;
    public Button returnToTitleButton;

    //敵の数をカウント
    private int activeEnemyCount;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    private void Start()
    {
        spawnManager.autoSpawn = false;

        // ボタンにイベント登録
        if (startButton != null) startButton.onClick.AddListener(StartGame);
        if (restartButton != null) restartButton.onClick.AddListener(RestartGame);
        if (returnToTitleButton != null) returnToTitleButton.onClick.AddListener(ReturnToTitle);

        ChangeState(GameState.Title);
    }

    private void Update()
    {
        switch (CurrentState)
        {
            case GameState.Title:
                spawnManager.autoSpawn = false;
                break;

            case GameState.Countdown:
                break;

            case GameState.Playing:
                spawnManager.autoSpawn = true;
                gameTimer -= Time.deltaTime;

                if (gameTimerText != null)
                    gameTimerText.text = Mathf.CeilToInt(gameTimer).ToString();

                spawnManager.TimeSpanChanged(gameTimer, gameDuration);

                if (gameTimer <= 0f)
                    EndGame();
                break;

            case GameState.GameOver:
                spawnManager.autoSpawn = false;
                break;

            case GameState.Result:
                break;
        }
    }

    public void ChangeState(GameState newState)
    {
        CurrentState = newState;

        titlePanel.SetActive(newState == GameState.Title);
        countdownPanel.SetActive(newState == GameState.Countdown);
        gameplayPanel.SetActive(newState == GameState.Playing);
        resultPanel.SetActive(newState == GameState.Result);

        switch (newState)
        {
            case GameState.Title:
                break;

            case GameState.Countdown:
                countdownText.text = "準備中..."; // すぐ表示される
                StartCoroutine(CountdownCoroutine());
                break;

            case GameState.Playing:
                gameTimer = gameDuration;
                spawnManager.autoSpawn = true;
                break;

            case GameState.GameOver:
                spawnManager.autoSpawn = false;
                int score = ScoreManager.Instance.GetScore();
                int combo = ScoreManager.Instance.GetMaxComboCount();
                resultManager.ShowResult(score, combo);
                ChangeState(GameState.Result);
                break;

            case GameState.Result:
                break;
        }
    }

    public void StartGame()
    {
        Debug.Log("StartGame() called");
        ChangeState(GameState.Countdown);
    }

    public void EndGame()
    {
        Debug.Log("EndGame() called");
        spawnManager.autoSpawn = false;
        DestroyAllEnemies();
        ChangeState(GameState.GameOver);
    }

    IEnumerator CountdownCoroutine()
    {
        Debug.Log("CountdownCoroutine() called");

        int count = countdownTime;
        countdownText.text = count.ToString();

        while (count > 0)
        {
            yield return new WaitForSeconds(1f);
            count--;
            countdownText.text = count > 0 ? count.ToString() : "START!";
        }

        yield return new WaitForSeconds(1f);
        ChangeState(GameState.Playing);
    }

    public void RestartGame()
    {
        Debug.Log("RestartGame() called");

        ScoreManager.Instance.ResetScore();

        spawnManager.hasSpawnedInitially = false;


        // ゲームタイマー初期化
        gameTimer = gameDuration;
        if (gameTimerText != null)
            gameTimerText.text = gameDuration.ToString();

        // カウントダウンテキスト初期化
        if (countdownText != null)
            countdownText.text = countdownTime.ToString();

        spawnManager.autoSpawn = false;
        ChangeState(GameState.Countdown);
    }

    public void ReturnToTitle()
    {
        Debug.Log("ReturnToTitle() called");

        spawnManager.autoSpawn = false;
        ChangeState(GameState.Title);
    }

    #region 敵の数チェック
    public void RegisterEnemy()
    {
        activeEnemyCount++;
    }

    public void UnregisterEnemy()
    {
        activeEnemyCount = Mathf.Max(0, activeEnemyCount - 1);
    }


    public bool AreAllEnemiesDefeated()
    {
        return activeEnemyCount == 0;
    }
    #endregion  


    private void DestroyAllEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
    }
}
