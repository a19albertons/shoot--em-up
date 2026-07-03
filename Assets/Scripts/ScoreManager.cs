using UnityEngine;

/// <summary>
/// Clase que gestiona la puntuación del jugador, incluyendo el almacenamiento de la puntuación máxima alcanzada utilizando PlayerPrefs.
/// </summary>
public class ScoreManager : MonoBehaviour
{
    static ScoreManager instance;

    private const string MaxScoreKey = "MaxScore"; // Clave para almacenar la puntuación máxima en PlayerPrefs

    // Método estático para obtener la instancia del ScoreManager
    /// <summary>
    /// Devuelve la instancia del ScoreManager, permitiendo el acceso a sus métodos y propiedades desde otras clases.
    /// </summary>
    /// <returns>La instancia del ScoreManager</returns>
    public static ScoreManager GetInstance()
    {
        return instance;
    }

    /// <summary>
    /// Inicializa la instancia del ScoreManager y asegura que solo exista una instancia en el juego. Si ya existe una instancia, destruye la nueva para mantener la singularidad.
    /// </summary>
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Evitar que el objeto se destruya al cambiar de escena
        }
        else if (instance != this)
        {
            // Si ya existe una instancia, destruimos el nuevo GameManager para mantener la singularidad
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Recibe la puntuación actual del jugador tras finalizar partida y la compara con la puntuación máxima almacenada. Si la puntuación actual es mayor, actualiza la puntuación máxima en PlayerPrefs.
    /// </summary>
    /// <param name="currentScore"></param>
    public void SubmitScore(int currentScore)
    {
        int maxScore = PlayerPrefs.GetInt(MaxScoreKey, 0); // Obtiene la puntuación máxima almacenada, o 0 si no existe
        Debug.Log("Entramso en submit Score");
        Debug.Log("Puntuación actual: " + currentScore);
        Debug.Log("Puntuación máxima almacenada: " + maxScore);
        // Comprueba el maximo vs el actual y actualiza si el actual es el mayor
        if (currentScore > maxScore)
        {
            PlayerPrefs.SetInt(MaxScoreKey, currentScore); // Actualiza la puntuación máxima si la actual es mayor
            PlayerPrefs.Save(); // Guarda los cambios en PlayerPrefs
            Debug.Log("Debería haberse guardado la puntuacion nueva si estamos aqui");
        }
    }

    /// <summary>
    /// Devuelve la puntuación máxima almacenada en PlayerPrefs. Si no existe una puntuación máxima, devuelve 0.
    /// </summary>
    /// <returns>La puntuación máxima almacenada</returns>
    public int GetMaxScore()
    {
        return PlayerPrefs.GetInt(MaxScoreKey, 0); // Devuelve la puntuación máxima almacenada, o 0 si no existe
    }
}