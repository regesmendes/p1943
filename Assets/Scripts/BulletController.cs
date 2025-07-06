using System;
using UnityEngine;

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
            if (playerController != null)
            {
                var enemyController = other.gameObject.GetComponent<EnemyController>();
                playerController.IncreaseScore(enemyController.points);
            }
            else
            {
                var hitPlayerController = other.gameObject.GetComponent<PlayerController>();
                if (hitPlayerController != null)
                {
                    GameMasterController.Instance.GameOver(hitPlayerController.playerName, hitPlayerController.GetScore());
                }
            }
            Instantiate(explosionParticle, other.gameObject.transform.position, other.gameObject.transform.rotation);
            Destroy(other.gameObject);
            AudioSource.PlayClipAtPoint(explosionSound, Camera.main.transform.position);
            Destroy(gameObject);
        }
    }
}
