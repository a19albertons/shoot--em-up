using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems; // Necesario para interactuar con el EventSystem

/// <summary>
/// Clase que controla el menú de pausa del juego, permitiendo pausar, reanudar, reiniciar y salir del juego. Maneja la interfaz de usuario y la navegación por teclado.
/// </summary>
public class MenuPausa : MonoBehaviour
{
    [Header("UI del Menú")]
    public GameObject panelPausa;

    [Header("Navegación por Teclado")]
    [Tooltip("Arrastra aquí el botón de Reanudar para que sea el primero seleccionado")]
    public GameObject botonInicial;

    private bool juegoPausado = false;

    /// <summary>
    /// Actualiza el estado del juego en cada frame, verificando si se ha presionado la tecla Escape para pausar o reanudar el juego. Maneja la visibilidad del panel de pausa y la escala de tiempo del juego.
    /// </summary>
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

    /// <summary>
    /// Reanuda el juego ocultando el panel de pausa y restableciendo la escala de tiempo a 1. También actualiza el estado del juego para reflejar que ya no está pausado.
    /// </summary>
    public void Reanudar()
    {
        panelPausa.SetActive(false);
        Time.timeScale = 1f; // Restablece el tiempo del juego
        juegoPausado = false;
    }

    /// <summary>
    /// Pausa el juego mostrando el panel de pausa y deteniendo la escala de tiempo. También limpia cualquier selección previa del EventSystem y asigna el foco del teclado al botón inicial para facilitar la navegación por teclado.
    /// </summary>
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

    /// <summary>
    /// Reinicia el juego recargando la escena actual y restableciendo la escala de tiempo a 1. También llama a las funciones necesarias para reiniciar las entidades del juego y resetear la puntuación y vidas del jugador.
    /// </summary>
    public void Reiniciar()
    {
        // Es importante devolver la escala de tiempo a 1 antes de recargar la escena
        Time.timeScale = 1f;
        GameLogic.GetInstance().ReiniciarEntidades();
        GameManager.GetInstance().ResetGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Sale del juego cerrando la aplicación. Esta función es útil para permitir al jugador salir del juego desde el menú de pausa.
    /// </summary>
    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}