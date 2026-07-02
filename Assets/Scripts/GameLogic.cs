using TMPro;
using UnityEngine;

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
    public static GameLogic GetInstance()
    {
        return instance;
    }

    void Start()
    {
        AsteroidSpawner.SetDelay(delay);
        AsteroidSpawner.StartSpawning();
        EnemySpawner.SetDelay(delay);
        EnemySpawner.StartSpawning();
        shipController.SetDuration(delay);
        shipController.StartStartPlayer();
    }

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
        }
    }

    // Destruye todos los GameObjects con la tag especificada
    void DestroyAllWithTag(string tag)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in objects)
        {
            Destroy(obj);
        }
    }
}