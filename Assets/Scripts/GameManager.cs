using TMPro;
using UnityEngine;

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

    // Método estático para obtener la instancia del GameManager
    public static GameManager GetInstance()
    {
        return instance;
    }

    private void OnGUI()
    {
        for (int i = 0; i < imgLives.Length; i++)
        {
            imgLives[i].SetActive(i < lives);
        }
        txtScore.text = string.Format("{0,4:D4}", score);
    }

    // Función Awake se ejecuta cuando se instancia el objeto
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
        }
    }

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
            if (Input.GetKeyDown(KeyCode.Space))
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
        }
        else
        {
            // Reiniciamos el juego
            GameLogic.GetInstance().ReiniciarEntidades();
        }
    }

    public void AddScore(int puntuacion)
    {
        score += puntuacion;
        // Controla la logica de date una vida extra. Al llegar a la mitad de la puntuación máxima
        if (score == 5000 && lives < 3)
        {
            lives++;
        }
    }
}
