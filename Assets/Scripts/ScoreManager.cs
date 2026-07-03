using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    static ScoreManager instance;

    private const string MaxScoreKey = "MaxScore"; // Clave para almacenar la puntuación máxima en PlayerPrefs

    // Método estático para obtener la instancia del ScoreManager
    public static ScoreManager GetInstance()
    {
        return instance;
    }

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

    public int GetMaxScore()
    {
        return PlayerPrefs.GetInt(MaxScoreKey, 0); // Devuelve la puntuación máxima almacenada, o 0 si no existe
    }
}