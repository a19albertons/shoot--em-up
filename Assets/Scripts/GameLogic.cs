using TMPro;
using UnityEngine;

/// <summary>
/// Clase que controla la lógica del juego, incluyendo la generación de asteroides y naves enemigas, así como el reinicio del juego y el manejo del Game Over.
/// </summary>
public class GameLogic : MonoBehaviour
{
    static GameLogic instance;

    [SerializeField]
    float delay;

    [SerializeField]
    AsteroidsSpawner AsteroidSpawner;

    [SerializeField]
    EnemySpawner EnemySpawner;

    [SerializeField]
    ShipController shipController;

    // Método estático para obtener la instancia del GameLogic
    /// <summary>
    /// Devuelve la instancia del GameLogic, permitiendo el acceso a sus métodos y propiedades desde otras clases.
    /// </summary>
    /// <returns>La instancia del GameLogic.</returns>
    public static GameLogic GetInstance()
    {
        return instance;
    }

    /// <summary>
    /// Inicializa el juego configurando los retrasos y comenzando la generación de asteroides y naves enemigas, así como la inicialización del jugador.
    /// </summary>
    void Start()
    {
        AsteroidSpawner.SetDelay(delay);
        AsteroidSpawner.StartSpawning();
        EnemySpawner.SetDelay(delay);
        EnemySpawner.StartSpawning();
        shipController.SetDuration(delay);
        shipController.StartStartPlayer();
    }

    /// <summary>
    /// Reinicia el juego deteniendo la generación de asteroides y naves enemigas, reiniciando la posición de la nave y reiniciando la generación de asteroides y naves enemigas. Además, limpia el escenario de todo lo creado durante la partida
    /// </summary>
    public void ReiniciarEntidades()
    {
        // Detener la generación de asteroides y naves enemigas, reiniciar la posición de la nave y reiniciar la generación de asteroides y naves enemigas
        AsteroidSpawner.StopSpawning();
        EnemySpawner.StopSpawning();
        shipController.StopStartPlayer();
        DestroyAllWithTag("AsteroidBig");
        DestroyAllWithTag("Enemy");
        DestroyAllWithTag("Shoot");
        shipController.StartStartPlayer();
        EnemySpawner.StartSpawning();
        AsteroidSpawner.StartSpawning();
    }

    /// <summary>
    /// Detiene la generación de asteroides, naves enemigas y propia porque has perdido todas las vidas
    /// </summary>
    public void GameOver()
    {
        // Detener la generación de asteroides, naves enemigas y propia porque has perdido todas las vidas
        AsteroidSpawner.StopSpawning();
        EnemySpawner.StopSpawning();
        shipController.StopStartPlayer();
        DestroyAllWithTag("AsteroidBig");
        DestroyAllWithTag("Enemy");
        DestroyAllWithTag("Shoot");
    }

    // Función Awake se ejecuta cuando se instancia el objeto
    /// <summary>
    /// Se ejecuta cuando se instancia el objeto. Se asegura de que solo exista una instancia del GameLogic y evita que se destruya al cambiar de escena.
    /// </summary>
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Evitar que el objeto se destruya al cambiar de escena
            DontDestroyOnLoad(AsteroidSpawner.gameObject); // Evitar que el objeto se destruya al cambiar de escena
            DontDestroyOnLoad(EnemySpawner.gameObject); // Evitar que el objeto se destruya al cambiar de escena
            DontDestroyOnLoad(shipController.gameObject); // Evitar que el objeto se destruya al cambiar de escena
        }
        else if (instance != this)
        {
            // Si ya existe una instancia, destruimos el nuevo GameManager para mantener la singularidad
            Destroy(gameObject);
            if (AsteroidSpawner != null)
            {
                Destroy(AsteroidSpawner.gameObject);
            }
            if (EnemySpawner != null)
            {
                Destroy(EnemySpawner.gameObject);
            }
            if (shipController != null)
            {
                Destroy(shipController.gameObject);
            }
        }
    }

    // Destruye todos los GameObjects con la tag especificada
    /// <summary>
    /// Destruye todos los GameObjects con la tag especificada. Esta función es útil para limpiar el escenario de todos los objetos generados durante la partida, como asteroides, naves enemigas y disparos.
    /// </summary>
    /// <param name="tag">Indica el nombre del tag de los objetos a destruir</param>
    void DestroyAllWithTag(string tag)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in objects)
        {
            Destroy(obj);
        }
    }
}