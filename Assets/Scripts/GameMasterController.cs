using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameMasterController : MonoBehaviour
{
    public static GameMasterController Instance;

    public GameObject PlayerPrefab;
    private PlayerController Player;
    private string ScorePlayerName;
    private int ScorePlayerScore;
    private bool isGameOver = false;


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
        ScorePlayerName = "YetToCome";
        ScorePlayerScore = 1000;
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
        UpdateLabels();

        if (isGameOver && Mouse.current.leftButton.wasPressedThisFrame)
        {
            isGameOver = false;
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
    }

    private void UpdateLabels()
    {
        foreach(TMP_Text text in gameObject.GetComponentsInChildren<TMP_Text>())
        {
            Debug.Log("Updating label: " + text.name);
            if (text.name == "Player" && Player != null)
            {
                text.SetText(Player.playerName + ": " + Player.GetScore());
            }
            else if (text.name == "Score")
            {
                text.SetText(ScorePlayerName + ": " + ScorePlayerScore);
            }
            else if (text.name == "GameOver")
            {
                text.SetText(isGameOver ? "Game Over" : "");
            }
        }
    }

    public void GameOver(string playerName, int score)
    {
        isGameOver = true;
        if (score > ScorePlayerScore)
        {
            ScorePlayerName = playerName;
            ScorePlayerScore = score;
        }
    }
}
