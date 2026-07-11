using TMPro;
using UnityEngine;

/// <summary>
/// Clase que controla la lógica ligada a la interfaz del juego, incluyendo la gestión de vidas, puntuación y mensajes de Game Over. También maneja el reinicio del juego y la actualización de la puntuación máxima.
/// </summary>
public class GameManager : MonoBehaviour
{
    static GameManager instance;

    int lives = 3;
    int score = 0;
    int maxScore = 0;

    [SerializeField]
    TextMeshProUGUI txtScore;

    [SerializeField]
    TextMeshProUGUI txtMaxScore;

    [SerializeField]
    TextMeshProUGUI txtMessage1;

    [SerializeField]
    TextMeshProUGUI txtMessage2;

    //Array paara las imágenes que marcan las vidas
    [SerializeField]
    GameObject[] imgLives;

    [SerializeField]
    Canvas canvas;

    /// <summary>
    /// Inicializa el juego configurando la visibilidad del cursor, ocultando los mensajes de Game Over y estableciendo la puntuación máxima desde el ScoreManager.
    /// </summary>
    void Start()
    {
        Cursor.visible = false; // Oculta el cursor al iniciar el juego
        txtMessage1.gameObject.SetActive(false);
        txtMessage2.gameObject.SetActive(false);
        maxScore = ScoreManager.GetInstance().GetMaxScore();
        txtMaxScore.text = string.Format("{0,4:D4}", maxScore);
    }

    // Método estático para obtener la instancia del GameManager
    /// <summary>
    /// Devuelve la instancia del GameManager, permitiendo el acceso a sus métodos y propiedades desde otras clases.
    /// </summary>
    /// <returns>La instancia del GameManager</returns>
    public static GameManager GetInstance()
    {
        return instance;
    }

    /// <summary>
    /// Actualiza la interfaz del juego mostrando las vidas restantes y la puntuación actual. Se llama automáticamente en cada frame para reflejar los cambios en tiempo real.
    /// </summary>
    private void OnGUI()
    {
        for (int i = 0; i < imgLives.Length; i++)
        {
            imgLives[i].SetActive(i < lives);
        }
        txtScore.text = string.Format("{0,4:D4}", score);
    }

    // Función Awake se ejecuta cuando se instancia el objeto
    /// <summary>
    /// Se ejecuta cuando se instancia el objeto. Se asegura de que solo exista una instancia del GameManager y evita que se destruya al cambiar de escena.
    /// </summary>
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Evitar que el objeto se destruya al cambiar de escena
            DontDestroyOnLoad(canvas.gameObject); // Evitar que el objeto se destruya al cambiar de escena
        }
        else if (instance != this)
        {
            // Si ya existe una instancia, destruimos el nuevo GameManager para mantener la singularidad
            Destroy(gameObject);
            if (canvas != null)
            {
                Destroy(canvas.gameObject); // Evitar que el objeto se destruya al cambiar de escena
            }
        }
    }

    /// <summary>
    /// Actualiza la lógica del juego en cada frame, verificando si el jugador ha perdido todas las vidas y mostrando los mensajes de Game Over. Permite reiniciar el juego al presionar la tecla Espacio.
    /// </summary>
    void Update()
    {
        if (lives == 0)
        {
            txtMessage1.gameObject.SetActive(true);
            txtMessage2.gameObject.SetActive(true);
            if (score > maxScore)
            {
                maxScore = score;
                txtMaxScore.text = string.Format("{0,4:D4}", maxScore);
            }
            if (Input.GetKeyDown(KeyCode.Space) && Time.timeScale > 0)
            {
                // Reiniciamos el juego
                lives = 3;
                score = 0;
                txtMessage1.gameObject.SetActive(false);
                txtMessage2.gameObject.SetActive(false);
                GameLogic.GetInstance().ReiniciarEntidades();
            }
        }
    }

    /// <summary>
    /// Reduce la vida del jugador en uno y verifica si ha perdido todas las vidas. Si el jugador pierde todas las vidas, se detiene el juego y se envía la puntuación al ScoreManager. Si aún tiene vidas restantes, se reinicia el juego.>
    /// </summary>
    public void ReduceLife()
    {
        lives--;
        Debug.Log("Vidas restantes: " + lives);

        if (lives <= 0)
        {
            // Game over, paramos el juego y mostramos el mensaje de Game Over
            lives = 0;
            GameLogic.GetInstance().GameOver();
            Debug.Log("Game Over");
            // Guardamos la puntuación máxima si es necesario y obtenemos el scoreManager para enviar la puntuación
            ScoreManager.GetInstance().SubmitScore(score);
            Debug.Log("Puntuación enviada fue " + score);
        }
        else
        {
            // Reiniciamos el juego
            GameLogic.GetInstance().ReiniciarEntidades();
        }
    }

    /// <summary>
    /// Agrega la puntuación obtenida al puntaje total del jugador y verifica si se ha alcanzado un umbral para otorgar una vida extra. Si el puntaje alcanza 5000 y el jugador tiene menos de 3 vidas, se incrementa en una vida.
    /// </summary>
    /// <param name="puntuacion">Puntuación a agregar</param>
    public void AddScore(int puntuacion)
    {
        score += puntuacion;
        // Controla la logica de date una vida extra. Al llegar a la mitad de la puntuación máxima
        if (score == 5000 && lives < 3)
        {
            lives++;
        }
    }

    /// <summary>
    /// Funcion que es llamada al reiniciar el juego para resetear las vidas y la puntuación, y ocultar los mensajes de Game Over. Se utiliza para reiniciar la partida desde el menu de pausa
    /// </summary>
    public void ResetGame()
    {
        lives = 3;
        score = 0;
        txtMessage1.gameObject.SetActive(false);
        txtMessage2.gameObject.SetActive(false);
    }
}
