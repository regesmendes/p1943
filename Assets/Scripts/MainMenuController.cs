using UnityEngine;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    public TMP_InputField playerNameInput;

    public void OnStartButtonClicked()
    {
        string playerName = playerNameInput.text.Trim();
        GameMasterController.Instance.StartGame(playerName);
    }
}
