using UnityEngine;

public class DamageableControllerEnemy : MonoBehaviour
{
    [SerializeField]
    private float health = 100f;

    public void TakeDamage(float amount, bool isPercentage)
    {
        if (isPercentage)
        {
            amount = health * amount / 100f;
        }

        health -= amount;

        if (health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        // Aquí puedes añadir efectos de muerte, como animaciones, sonidos, etc.
        Destroy(gameObject); // Destruye el enemigo cuando su salud llega a 0
    }
}
