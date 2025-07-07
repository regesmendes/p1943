using System;
using UnityEngine;

// INHERITANCE - BulletController inherits from MonoBehaviour
public class BulletController : MonoBehaviour
{

    public float bulletSpeed;
    public Vector3 bulletDirection;
    public string targetTag = "Player";
    public PlayerController playerController;

    private readonly float zBounds = 25f;
    private readonly float xBounds = 40f;
    public ParticleSystem explosionParticle;
    public AudioClip explosionSound;

    void Start()
    {
    }

    void Update()
    {
        transform.position += bulletSpeed * Time.deltaTime * bulletDirection;
        CheckBoundaries();
    }

    private void CheckBoundaries()
    {
        if (Math.Abs(transform.position.z) > zBounds || Math.Abs(transform.position.x) > xBounds)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(targetTag))
        {
            // POLYMORPHISM - Using GameEntity reference to call TakeDamage on any game entity
            var gameEntity = other.gameObject.GetComponent<GameEntity>();
            gameEntity.TakeDamage(100);

            // Award points if this is a player bullet hitting an enemy
            Score(other);
            Instantiate(explosionParticle, other.gameObject.transform.position, other.gameObject.transform.rotation);
            AudioSource.PlayClipAtPoint(explosionSound, Camera.main.transform.position);
            Destroy(gameObject);
        }
    }

    private void Score(Collider target)
    {
        if (playerController != null)
        {
            var enemyController = target.gameObject.GetComponent<EnemyController>();
            playerController.IncreaseScore(enemyController.points);
        }
    }
}
