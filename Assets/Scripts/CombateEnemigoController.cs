using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombateEnemigoController : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.GetContact(0).normal.y <= -0.9)
        {
            GameObject.FindGameObjectWithTag("Bandera").GetComponent<FlagController>().EnemigoEliminado();
            other.gameObject.GetComponent<CharacterController2D>();
            Destroy(gameObject);
        }
    }
}

//<>