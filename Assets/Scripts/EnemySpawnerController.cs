using UnityEngine;

// INHERITANCE - EnemySpawnerController inherits from MonoBehaviour
public class EnemySpawnerController : MonoBehaviour
{
    public GameObject enemyPrefab;
    private int waveCount = 0;
    private float intervalBetweenPlanes = 0.3f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 1f, 10f);
    }

    private void SpawnEnemy()
    {
        if (waveCount++ % 2 == 0)
        {
            LeftToRightSquad();
        } else
        {
            RightToLeftSquad();
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
