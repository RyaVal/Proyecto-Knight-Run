using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCheck : MonoBehaviour
{
   private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemyCheck>())
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
