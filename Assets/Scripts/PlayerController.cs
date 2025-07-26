using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : GameEntity
{
    private Vector2 moveInput;
    private PlayerInput playerInput;
    public GameObject bulletPrefab;

    private readonly float zBounds = 22f;
    private readonly float xBounds = 38f;

    private int score = 0;
    public AudioClip shootSound;
    private const string DefaultEnemyTag = "Enemy";
    private const float BulletSpeed = 30f;
    private Vector3 BulletStartingPosition = Vector3.forward * 2f;

    public string playerName;

    public int GetScore()
    {
        return score;
    }

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        Move();
        CheckBoundaries();
    }

    public void OnMove()
    {
        moveInput = playerInput.actions["Move"].ReadValue<Vector2>();
    }

    public void OnAttack()
    {
        var bulletObj = Instantiate(bulletPrefab, transform.position + BulletStartingPosition, Quaternion.identity);
        var bulletController = bulletObj.GetComponent<BulletController>();
        bulletController.targetTag = DefaultEnemyTag;
        bulletController.bulletSpeed = BulletSpeed;
        bulletController.playerController = this;
        AudioSource.PlayClipAtPoint(shootSound, transform.position);
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
    }

    protected override void Die()
    {
        GameMasterController.Instance.GameOver(playerName, GetScore());
        Destroy(gameObject);
    }
    protected override void Move()
    {
        Vector3 move = new(moveInput.x, 0, moveInput.y);
        transform.position += MoveSpeed * Time.deltaTime * move;
    }
    protected override void CheckBoundaries()
    {
        if (transform.position.z > zBounds || transform.position.z < -zBounds ||
            transform.position.x > xBounds || transform.position.x < -xBounds)
        {
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, -xBounds, xBounds),
                2f,
                Mathf.Clamp(transform.position.z, -zBounds, zBounds)
            );
        }
    }
}
