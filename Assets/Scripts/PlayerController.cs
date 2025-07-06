using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Vector2 moveInput;
    private PlayerInput playerInput;
    public float moveSpeed;
    public GameObject bulletPrefab;

    private readonly float zBounds = 22f;
    private readonly float xBounds = 38f;

    private int score = 0;
    public AudioClip shootSound;

    public string playerName;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        Vector3 move = new(moveInput.x, 0, moveInput.y);
        transform.position += moveSpeed * Time.deltaTime * move;
        CheckBoundaries();
    }

    private void CheckBoundaries()
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

    public void OnMove()
    {
        moveInput = playerInput.actions["Move"].ReadValue<Vector2>();
    }

    public void OnAttack()
    {
        var bulletObj = Instantiate(bulletPrefab, transform.position + Vector3.forward * 2, Quaternion.identity);
        var bulletController = bulletObj.GetComponent<BulletController>();
        bulletController.targetTag = "Enemy";
        bulletController.bulletSpeed = 30f;
        bulletController.playerController = this;
        AudioSource.PlayClipAtPoint(shootSound, transform.position);
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
    }

    public int GetScore()
    {
        return score;
    }
}
