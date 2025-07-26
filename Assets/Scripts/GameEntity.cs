using UnityEngine;

public abstract class GameEntity : MonoBehaviour
{
    private int health = 100;
    private float moveSpeed = 10f;

    public int Health
    {
        get { return health; }
        protected set { health = Mathf.Max(0, value); }
    }
    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        OnDamageTaken(damage);

        if (Health <= 0)
        {
            Die();
        }
    }

    protected virtual void OnDamageTaken(int damage)
    {
        Debug.Log($"{gameObject.name} took {damage} damage!");
    }

    protected abstract void Die();
    protected abstract void Move();
    protected abstract void CheckBoundaries();
}