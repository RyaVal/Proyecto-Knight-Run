using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.UI;

public class Agro : MonoBehaviour
{
    public float radioBusqueda;
    public LayerMask capaPlayer;
    public Transform transformJugador;
    public float velocidadMovimineto;
    public float distanciaMaxima;
    public Vector3 puntoInicial;
    public bool mirandoDerecha;
    public Rigidbody2D rb2D;
    public Animator animator;


    public EstadosMovimiento estadoActual;

    public enum EstadosMovimiento
    {
        Esperando,
        Siguiendo,
        Volviendo,
    }

    private void Start()
    {
        puntoInicial = transform.position;
    }

    private void Update()
    {
        switch (estadoActual)
        {
            case EstadosMovimiento.Esperando:
                EstadoEsperando();
                break;
            case EstadosMovimiento.Siguiendo:
                EstadoSiguiendo();
                break;
            case EstadosMovimiento.Volviendo:
                EstadoVolviendo();
                break;
        }
    }

    private void EstadoEsperando()
    {
        Collider2D jugadorCollider = Physics2D.OverlapCircle(transform.position, radioBusqueda, capaPlayer);

        if (jugadorCollider)
        {
            transformJugador = jugadorCollider.transform;

            estadoActual = EstadosMovimiento.Siguiendo;
        }
    }

    private void EstadoSiguiendo()
    {
        animator.SetBool("Corriendo", true);

        if (transformJugador == null)
        {
            estadoActual = EstadosMovimiento.Volviendo;
            return;
        }

       if (transform.position.x < transform.position.x)
        {
            rb2D.velocity = new Vector2(velocidadMovimineto, rb2D.velocity.y);
        }
       else
        {
            rb2D.velocity = new Vector2(-velocidadMovimineto,rb2D.velocity.y);
        }

        GirarAObjetivo(transformJugador.position);

        if (Vector2.Distance(transform.position, puntoInicial) > distanciaMaxima ||
            Vector2.Distance(transform.position, transformJugador.position) > distanciaMaxima)
        {
            estadoActual = EstadosMovimiento.Volviendo;
            transformJugador = null;
        }
    }

    public void EstadoVolviendo()
    {
        if(transform.position.x < puntoInicial.x)
        {
            rb2D.velocity = new Vector2(velocidadMovimineto, rb2D.velocity.y);
        }
       else
        {
            rb2D.velocity = new Vector2(-velocidadMovimineto, rb2D.velocity.y);
        }

        GirarAObjetivo(puntoInicial);

        if (Vector2.Distance(transform.position, puntoInicial) < 0.1f)
        {
            rb2D.velocity = Vector2.zero;

            animator.SetBool("Corriendo", false);   

            estadoActual = EstadosMovimiento.Esperando;
        }
    }

    private void GirarAObjetivo(Vector3 objetivo)
    {
        if (objetivo.x < transform.position.x && !mirandoDerecha)
        {
            Girar();
        }
        else if (objetivo.x > transform.position.x && mirandoDerecha)
        {
            Girar();
        }
    }

    private void Girar()
    {
        mirandoDerecha = !mirandoDerecha;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioBusqueda);
        Gizmos.DrawWireSphere(puntoInicial, distanciaMaxima);  
    }
}
// <> |