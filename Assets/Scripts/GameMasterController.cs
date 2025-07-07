using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameMasterController : MonoBehaviour
{
    public static GameMasterController Instance;

    public GameObject PlayerPrefab;
    private PlayerController Player;
    private string HighScorePlayerName;
    private int HighScorePlayerScore;
    public bool isGameOver = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        HighScorePlayerName = "YetToCome";
        HighScorePlayerScore = 1000;
    }

    public void StartGame(string playerName)
    {
        StartCoroutine(LoadGameScene(playerName));
    }

    private IEnumerator LoadGameScene(string playerName)
    {
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        yield return null;

        GameObject playerObj = Instantiate(PlayerPrefab, PlayerPrefab.transform.position, PlayerPrefab.transform.rotation);
        playerObj.name = "Player";
        Player = playerObj.GetComponent<PlayerController>();
        Player.playerName = playerName;
    }

    public void Update()
    {
        if (Player != null)
        {
            UpdateHighScore(Player.playerName, Player.GetScore());
        }

        if (isGameOver && Mouse.current.leftButton.wasPressedThisFrame)
        {
            isGameOver = false;
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
    }

    public void GameOver(string playerName, int score)
    {
        isGameOver = true;
        UpdateHighScore(playerName, score);
    }

    public string GetPlayerName()
    {
        return Player != null ? Player.playerName : "No Player";
    }

    public int GetPlayerScore()
    {
        return Player != null ? Player.GetScore() : 0;
    }

    private void UpdateHighScore(string playerName, int score)
    {
        if (score > HighScorePlayerScore)
        {
            HighScorePlayerName = playerName;
            HighScorePlayerScore = score;
        }
    }

    public int GetHighScorePlayerScore()
    {
        return HighScorePlayerScore;
    }

    public string GetHighScorePlayerName()
    {
        return HighScorePlayerName;
    }
}
