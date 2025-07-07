using UnityEngine;
using UnityEngine.UI;
using TMPro;

// INHERITANCE - MainMenuController inherits from MonoBehaviour
public class MainMenuController : MonoBehaviour
{
    public TMP_InputField playerNameInput;
    public Button startButton;

    void Start()
    {
        // Disable button briefly to prevent accidental clicks from previous scene
        startButton.interactable = false;
        Invoke("EnableButton", 0.5f);
    }

    void EnableButton()
    {
        startButton.interactable = true;
    }

    public void OnStartButtonClicked()
    {
        string playerName = playerNameInput.text.Trim();
        GameMasterController.Instance.StartGame(playerName);
    }
}
