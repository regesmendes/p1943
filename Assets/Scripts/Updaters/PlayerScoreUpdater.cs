using TMPro;
using UnityEngine;

public class PlayerScoreUpdater : MonoBehaviour
{
    public TextMeshProUGUI Player;

    void Update()
    {
        if (!GameMasterController.Instance.isGameOver)
        {
            Player.text = GameMasterController.Instance.GetPlayerName() + ": " + GameMasterController.Instance.GetPlayerScore();
        }
    }
}
