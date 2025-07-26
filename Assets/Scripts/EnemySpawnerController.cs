using System.Linq;
using UnityEngine;

// INHERITANCE - EnemySpawnerController inherits from MonoBehaviour
public class EnemySpawnerController : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject enemyPrefab;

    // Constants
    private const float INITIAL_DELAY = 1f;
    private const float INTERVAL_BETWEEN_PLANES = 0.3f;
    private const int INTERVAL_BETWEEN_WAVES = 5;
    private const int WAVE_LIMIT = 5;
    private const int PLANES_PER_SQUAD = 5;
    private const float LEFT_SPAWN_X = -42f;
    private const float RIGHT_SPAWN_X = 42f;
    private const float SPAWN_Y = 2f;
    private const float SPAWN_Z = 20f;

    void Start()
    {
        Invoke(nameof(StartStage1), INITIAL_DELAY);
    }

    private void StartStage1()
    {
        string[] enemySquads = {
            nameof(SpawnLeftToRightSquad),
            nameof(SpawnRightToLeftSquad)
        };

        for (int waveCount = 0; waveCount < WAVE_LIMIT; waveCount++)
        {
            float delay = INTERVAL_BETWEEN_WAVES * (waveCount + 1);
            string squadMethod = enemySquads[waveCount % enemySquads.Length];
            Invoke(squadMethod, delay);
        }
    }

    private void SpawnLeftToRightSquad()
    {
        SpawnSquad(nameof(SpawnLeftToRightPlane));
    }

    private void SpawnRightToLeftSquad()
    {
        SpawnSquad(nameof(SpawnRightToLeftPlane));
    }

    private void SpawnSquad(string planeSpawnMethod)
    {
        for (int i = 0; i < PLANES_PER_SQUAD; i++)
        {
            Invoke(planeSpawnMethod, i * INTERVAL_BETWEEN_PLANES);
        }
    }

    private void SpawnLeftToRightPlane()
    {
        Vector3 startPosition = new(LEFT_SPAWN_X, SPAWN_Y, SPAWN_Z);
        Vector3 rotation = new(0, 180, 0);
        float moveSpeed = Mathf.Abs(enemyPrefab.GetComponent<EnemyController>().MoveSpeed);

        SpawnEnemyPlane(startPosition, rotation, moveSpeed);
    }

    private void SpawnRightToLeftPlane()
    {
        Vector3 startPosition = new(RIGHT_SPAWN_X, SPAWN_Y, SPAWN_Z);
        Vector3 rotation = new(0, 0, 180);
        float moveSpeed = -Mathf.Abs(enemyPrefab.GetComponent<EnemyController>().MoveSpeed);

        SpawnEnemyPlane(startPosition, rotation, moveSpeed);
    }

    private void SpawnEnemyPlane(Vector3 startPosition, Vector3 rotation, float moveSpeed)
    {
        GameObject enemyObj = Instantiate(enemyPrefab, enemyPrefab.transform.position, enemyPrefab.transform.rotation);
        EnemyController enemy = enemyObj.GetComponent<EnemyController>();

        enemy.Initialize(startPosition);
        enemy.MoveSpeed = moveSpeed;
        enemy.transform.Rotate(rotation);
    }
}
