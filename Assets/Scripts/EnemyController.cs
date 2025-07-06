using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject bulletPrefab;
    private GameObject target;
    public Vector3 startingPosition;
    public float speed = 10f;
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
        target = GameObject.Find("Player");
        StartCoroutine(Shoot());
    }

    void FixedUpdate()
    {
        if (!isCircling)
        {
            transform.position = new Vector3(
                transform.position.x + speed * Time.deltaTime,
                2f,
                20f
            );

            if (
                !isCircleDone
            && ((speed > 0 && transform.position.x >= 0)
                || (speed < 0 && transform.position.x <= 0)
            )) {
                isCircling = true;
                circleCenter = new Vector3(transform.position.x, 2f, transform.position.z - circleRadius);
                Vector3 offset = transform.position - circleCenter;
                circleStartAngle = Mathf.Atan2(offset.z, offset.x);
                circleAngle = circleStartAngle;
            }
        }
        else
        {
            circleAngle += circleSpeed * Time.deltaTime * (-speed / Mathf.Abs(speed));
            float x = circleCenter.x + Mathf.Cos(circleAngle) * circleRadius;
            float z = circleCenter.z + Mathf.Sin(circleAngle) * circleRadius;
            transform.position = new Vector3(x, 2f, z);

            if (Mathf.Abs(circleAngle - circleStartAngle) >= 2 * Mathf.PI)
            {
                isCircling = false;
                isCircleDone = true;
            }
        }

        if (transform.position.x > xBounds || transform.position.x < -xBounds)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator Shoot()
    {
        while (target != null)
        {
            yield return new WaitForSeconds(1 / fireRate);
            if (target == null)
            {
                Debug.Log("Target is dead! Suspending firing.");
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
}
