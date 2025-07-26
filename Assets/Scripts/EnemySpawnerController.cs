using System.Linq;
using UnityEngine;

// INHERITANCE - EnemySpawnerController inherits from MonoBehaviour
public class EnemySpawnerController : MonoBehaviour
{
    public GameObject enemyPrefab;
    private readonly float intervalBetweenPlanes = 0.3f;

    void Start()
    {
        Invoke(nameof(Stage1), 1f);
    }

    private void Stage1()
    {
        int intervalBetweenWaves = 5;
        int waveLimit = 5;

        string[] enemySquads = {
            nameof(LeftToRightSquad),
            nameof(RightToLeftSquad)
        };

        for (int waveCount = 0, interval = intervalBetweenWaves; waveCount++ < waveLimit;)
        {
            Invoke(enemySquads[waveCount % enemySquads.Count()], interval * waveCount);
        }
    }

    private void LeftToRightSquad()
    {
        for (int i = 0; i < 5; i++)
        {
            Invoke(nameof(LeftToRightPlane), i * intervalBetweenPlanes);
        }
    }

    private void RightToLeftSquad()
    {
        for (int i = 0; i < 5; i++)
        {
            Invoke(nameof(RightToLeftPlane), i * intervalBetweenPlanes);
        }
    }

    private void LeftToRightPlane()
    {
        var enemyObj = Instantiate(
            enemyPrefab,
            enemyPrefab.transform.position,
            enemyPrefab.transform.rotation
        );
        var enemy = enemyObj.GetComponent<EnemyController>();
        enemy.startingPosition = new Vector3(-42, 2, 20);
        enemy.transform.Rotate(0, 180, 0); // Rotate to face right direction
    }

    private void RightToLeftPlane()
    {
        var enemyObj = Instantiate(
            enemyPrefab,
            enemyPrefab.transform.position,
            enemyPrefab.transform.rotation
        );
        var enemy = enemyObj.GetComponent<EnemyController>();
        enemy.startingPosition = new Vector3(42, 2, 20);
        enemy.MoveSpeed *= -1;
        enemy.transform.Rotate(0, 0, 180);
    }
}
