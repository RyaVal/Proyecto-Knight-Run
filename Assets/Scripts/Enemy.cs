using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float vida;
    private Animator animator;
    
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TomarDa�o (float da�o)
    {
        vida -= da�o;

        if(vida <= 0)
        {
            Muerto();
        }
    }

    public void Muerto ()
    {
        animator.SetTrigger("Muerto");
    }
}
