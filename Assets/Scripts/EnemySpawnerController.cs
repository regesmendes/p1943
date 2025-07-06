using UnityEngine;

public class EnemySpawnerController : MonoBehaviour
{
    public GameObject enemyPrefab;
    private int waveCount = 0;
    private float intervalBetweenPlanes = 0.3f;

    void Start()
    {
        InvokeRepeating("spawnEnemy", 1f, 10f);
    }

    private void spawnEnemy()
    {
        if (waveCount++ % 2 == 0)
        {
            leftToRightSquad();
        } else
        {
            rightToLeftSquad();
        }
    }

    private void leftToRightSquad()
    {
        for (int i = 0; i < 5; i++)
        {
            Invoke("leftToRightPlane", i * intervalBetweenPlanes);
        }
    }

    private void rightToLeftSquad()
    {
        for (int i = 0; i < 5; i++)
        {
            Invoke("rightToLeftPlane", i * intervalBetweenPlanes);
        }
    }

    private void leftToRightPlane()
    {
        var enemyObj = Instantiate(
            enemyPrefab,
            enemyPrefab.transform.position,
            enemyPrefab.transform.rotation
        );
        var enemy = enemyObj.GetComponent<EnemyController>();
        enemy.startingPosition = new Vector3(-42, 2, 20);
    }

    private void rightToLeftPlane()
    {
        var enemyObj = Instantiate(
            enemyPrefab,
            enemyPrefab.transform.position,
            enemyPrefab.transform.rotation
        );
        var enemy = enemyObj.GetComponent<EnemyController>();
        enemy.startingPosition = new Vector3(42, 2, 20);
        enemy.speed *= -1;
        enemy.transform.Rotate(0, 0, 180);
    }
}
