using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableController : MonoBehaviour
{
    [SerializeField]
    float maxhealth;

    private float _currentHealth;

    private void Awake()
    {
        _currentHealth = maxhealth;
    }

    public void TakeDamage(float value, bool isPercentage = false)
    {
        float damage = Mathf.Abs(value);

        if (isPercentage)
        {
            damage = maxhealth * 100 / maxhealth;
        }

        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }


}

// <>
