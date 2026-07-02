using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems; // Necesario para interactuar con el EventSystem

public class MenuPausa : MonoBehaviour
{
    [Header("UI del Menú")]
    public GameObject panelPausa;
    
    [Header("Navegación por Teclado")]
    [Tooltip("Arrastra aquí el botón de Reanudar para que sea el primero seleccionado")]
    public GameObject botonInicial;

    private bool juegoPausado = false;

    void Update()
    {
        // Detecta la tecla de escape para pausar/reanudar
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (juegoPausado)
            {
                Reanudar();
            }
            else
            {
                Pausar();
            }
        }
    }

    public void Reanudar()
    {
        panelPausa.SetActive(false);
        Time.timeScale = 1f; // Restablece el tiempo del juego
        juegoPausado = false;
    }

    void Pausar()
    {
        panelPausa.SetActive(true);
        Time.timeScale = 0f; // Congela el juego
        juegoPausado = true;

        // Limpia cualquier selección previa del EventSystem para evitar conflictos
        EventSystem.current.SetSelectedGameObject(null);
        
        // Asigna el foco del teclado al botón inicial
        EventSystem.current.SetSelectedGameObject(botonInicial);
    }

    public void Reiniciar()
    {
        // Es importante devolver la escala de tiempo a 1 antes de recargar la escena
        Time.timeScale = 1f;
        GameLogic.GetInstance().ReiniciarEntidades();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}