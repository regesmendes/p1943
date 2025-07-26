using System.Collections;
using UnityEngine;

public class EnemyController : GameEntity
{
    public GameObject bulletPrefab;
    private GameObject target;
    public Vector3 startingPosition;
    public float xBounds = 44f;
    public float fireRate = 0.5f;

    public float circleRadius = 10f;
    public float circleSpeed = 1f;

    private bool isCircling = false;
    private Vector3 circleCenter;
    private float circleAngle = 0f;
    private float circleStartAngle = 0f;
    private bool isCircleDone = false;

    public int points = 100;

    void Start()
    {
        transform.position = startingPosition;
        target = GameObject.Find(GameMasterController.Instance.PlayerPrefab.tag);
        StartCoroutine(Shoot());
    }

    void FixedUpdate()
    {
        Move();
        CheckBoundaries();
    }

    IEnumerator Shoot()
    {
        while (target != null && !GameMasterController.Instance.isGameOver)
        {
            yield return new WaitForSeconds(1 / fireRate);
            if (target == null || GameMasterController.Instance.isGameOver)
            {
                // Debug.Log("Target is dead! Suspending firing.");
                break;
            }
            Vector3 bulletDirection = target.transform.position - transform.position;
            Vector3 bulletStartPosition = transform.position + bulletDirection.normalized * 2;
            var bulletObj = Instantiate(bulletPrefab, bulletStartPosition, Quaternion.identity);
            var bulletController = bulletObj.GetComponent<BulletController>();
            bulletController.bulletDirection = bulletDirection.normalized;
            bulletController.bulletSpeed = 10f;
        }
    }

    protected override void Die()
    {
        Destroy(gameObject);
    }
    protected override void Move()
    {
        if (!isCircling)
        {
            transform.position = new Vector3(
                transform.position.x + MoveSpeed * Time.deltaTime,
                2f,
                20f
            );

            if (
                !isCircleDone
            && ((MoveSpeed > 0 && transform.position.x >= 0)
                || (MoveSpeed < 0 && transform.position.x <= 0)
            ))
            {
                isCircling = true;
                circleCenter = new Vector3(transform.position.x, 2f, transform.position.z - circleRadius);
                Vector3 offset = transform.position - circleCenter;
                circleStartAngle = Mathf.Atan2(offset.z, offset.x);
                circleAngle = circleStartAngle;
            }
        }
        else
        {
            circleAngle += circleSpeed * Time.deltaTime * (-MoveSpeed / Mathf.Abs(MoveSpeed));
            float x = circleCenter.x + Mathf.Cos(circleAngle) * circleRadius;
            float z = circleCenter.z + Mathf.Sin(circleAngle) * circleRadius;
            transform.position = new Vector3(x, 2f, z);

            float yRotation = circleAngle * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(MoveSpeed > 0 ? 180 : 0, -yRotation, 0);

            if (Mathf.Abs(circleAngle - circleStartAngle) >= 2 * Mathf.PI)
            {
                isCircling = false;
                isCircleDone = true;
            }
        }
    }
    protected override void CheckBoundaries()
    {
        if (transform.position.x > xBounds || transform.position.x < -xBounds)
        {
            Destroy(gameObject);
        }
    }
}
