using UnityEngine;

public class HighScoreUpdater : MonoBehaviour
{
    public TMPro.TextMeshProUGUI HighScoreText;

    void Update()
    {
        int highScore = GameMasterController.Instance.GetHighScorePlayerScore();
        string playerName = GameMasterController.Instance.GetHighScorePlayerName();
        HighScoreText.text = $"{playerName}: {highScore}";
    }
}
