using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    public void Reiniciar(string nombre)
    {
        SceneManager.LoadScene(nombre);
    }

    public void MenuInicial(string nombre)
    {
        SceneManager.LoadScene(nombre);
    }

    public void Salir()
    {
        Debug.Log("Exit...");
        Application.Quit();
    }    
}
