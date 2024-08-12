using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class FlagController : MonoBehaviour
{
    [SerializeField]
    private int cantidadEnemigos;

    [SerializeField]
    private int enemigosEliminados;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        cantidadEnemigos = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    private void ActivarBandera()
    {
        animator.SetTrigger("activate");
    }

    public void EnemigoEliminado()
    {
        enemigosEliminados += 1;

        if (enemigosEliminados == cantidadEnemigos) 
        {
            ActivarBandera();   
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && enemigosEliminados == cantidadEnemigos)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
