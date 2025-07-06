using UnityEngine;

public class GameOverUpdater : MonoBehaviour
{

    public TMPro.TextMeshProUGUI GameOverText;

    void Update()
    {
        GameOverText.text = GameMasterController.Instance.isGameOver ? "Game Over" : string.Empty;
    }
}
