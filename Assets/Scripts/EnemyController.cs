using System.Collections;
using UnityEngine;

public class EnemyController : GameEntity
{
    [Header("Combat Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private int points = 100;
    
    // ENCAPSULATION - Public property for private points field
    public int Points => points;
    
    [Header("Movement Settings")]
    [SerializeField] private float xBounds = 44f;
    [SerializeField] private float circleRadius = 10f;
    [SerializeField] private float circleSpeed = 1f;
    
    // Constants
    private const float FLIGHT_ALTITUDE = 2f;
    private const float FLIGHT_Z_POSITION = 20f;
    private const float BULLET_SPEED = 10f;
    private const float BULLET_OFFSET = 2f;
    private const float FULL_CIRCLE = 2 * Mathf.PI;
    
    // Private fields
    private GameObject target;
    private Vector3 startingPosition;
    private bool isCircling;
    private Vector3 circleCenter;
    private float circleAngle;
    private float circleStartAngle;
    private bool isCircleDone;

    public void Initialize(Vector3 startPos)
    {
        startingPosition = startPos;
        transform.position = startingPosition;
        FindTarget();
        StartCoroutine(ShootingCoroutine());
    }
    
    private void FindTarget()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {
        Move();
        CheckBoundaries();
    }

    private IEnumerator ShootingCoroutine()
    {
        var waitTime = new WaitForSeconds(1f / fireRate);
        
        while (CanShoot())
        {
            yield return waitTime;
            if (CanShoot())
            {
                FireBullet();
            }
        }
    }
    
    private bool CanShoot()
    {
        return target != null && !GameMasterController.Instance.isGameOver;
    }
    
    private void FireBullet()
    {
        Vector3 bulletDirection = (target.transform.position - transform.position).normalized;
        Vector3 bulletStartPosition = transform.position + bulletDirection * BULLET_OFFSET;
        
        GameObject bulletObj = Instantiate(bulletPrefab, bulletStartPosition, Quaternion.identity);
        BulletController bulletController = bulletObj.GetComponent<BulletController>();
        bulletController.bulletDirection = bulletDirection;
        bulletController.bulletSpeed = BULLET_SPEED;
    }

    protected override void Die()
    {
        Destroy(gameObject);
    }
    protected override void Move()
    {
        if (!isCircling)
        {
            MoveStraight();
            CheckForCircleStart();
        }
        else
        {
            MoveInCircle();
        }
    }
    
    private void MoveStraight()
    {
        Vector3 currentPos = transform.position;
        transform.position = new Vector3(
            currentPos.x + MoveSpeed * Time.deltaTime,
            FLIGHT_ALTITUDE,
            FLIGHT_Z_POSITION
        );
    }
    
    private void CheckForCircleStart()
    {
        if (isCircleDone) return;
        
        bool shouldStartCircle = (MoveSpeed > 0 && transform.position.x >= 0) ||
                                (MoveSpeed < 0 && transform.position.x <= 0);
        
        if (shouldStartCircle)
        {
            InitializeCircleMovement();
        }
    }
    
    private void InitializeCircleMovement()
    {
        isCircling = true;
        circleCenter = new Vector3(transform.position.x, FLIGHT_ALTITUDE, transform.position.z - circleRadius);
        Vector3 offset = transform.position - circleCenter;
        circleStartAngle = Mathf.Atan2(offset.z, offset.x);
        circleAngle = circleStartAngle;
    }
    
    private void MoveInCircle()
    {
        UpdateCirclePosition();
        UpdateCircleRotation();
        CheckForCircleCompletion();
    }
    
    private void UpdateCirclePosition()
    {
        circleAngle += circleSpeed * Time.deltaTime * GetDirectionMultiplier();
        float x = circleCenter.x + Mathf.Cos(circleAngle) * circleRadius;
        float z = circleCenter.z + Mathf.Sin(circleAngle) * circleRadius;
        transform.position = new Vector3(x, FLIGHT_ALTITUDE, z);
    }
    
    private void UpdateCircleRotation()
    {
        float yRotation = circleAngle * Mathf.Rad2Deg;
        float xRotation = MoveSpeed > 0 ? 180 : 0;
        float bankAngle = 30f * GetDirectionMultiplier(); // Inside wing down
        transform.rotation = Quaternion.Euler(xRotation, -yRotation, bankAngle);
    }
    
    private void CheckForCircleCompletion()
    {
        if (Mathf.Abs(circleAngle - circleStartAngle) >= FULL_CIRCLE)
        {
            isCircling = false;
            isCircleDone = true;
        }
    }
    
    private float GetDirectionMultiplier()
    {
        return -MoveSpeed / Mathf.Abs(MoveSpeed);
    }
    protected override void CheckBoundaries()
    {
        if (transform.position.x > xBounds || transform.position.x < -xBounds)
        {
            Destroy(gameObject);
        }
    }
}
