using UnityEngine;

// INHERITANCE - Base class for all game entities (initially Player and Enemy) and it inherits from MonoBehaviour
public abstract class GameEntity : MonoBehaviour
{
    // ENCAPSULATION - private fields with public accessor
    private int health = 100;
    private float moveSpeed = 10f;

    // ENCAPSULATION - Public properties to access private fields
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

    // ABSTRACTION - High-level method that hides implementation details
    public void TakeDamage(int damage)
    {
        Health -= damage;
        OnDamageTaken(damage);

        if (Health <= 0)
        {
            Die();
        }
    }

    // POLYMORPHISM - Virtual methods that can be overridden by child classes
    protected virtual void OnDamageTaken(int damage)
    {
        Debug.Log($"{gameObject.name} took {damage} damage!");
    }

    // POLYMORPHISM - Abstract methods that MUST be implemented by child classes
    protected abstract void Die();
    protected abstract void Move();
    protected abstract void CheckBoundaries();
}