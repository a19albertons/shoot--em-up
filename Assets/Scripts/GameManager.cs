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
    TextMeshProUGUI txtMessage;

    //Array paara las imágenes que marcan las vidas
    [SerializeField]
    GameObject[] imgLives;

    [SerializeField]
    GameObject[] deshabilitar;

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
        }
        else if (instance != this)
        {
            // Si ya existe una instancia, destruimos el nuevo GameManager para mantener la singularidad
            Destroy(gameObject);
        }
    }

    void Start()
    {
        txtMessage.gameObject.SetActive(false);
    }

    void Update()
    {
        if (lives == 0)
        {
            txtMessage.gameObject.SetActive(true);
            foreach (GameObject go in deshabilitar)
            {
                go.SetActive(false);
            }
            if (score > maxScore)
            {
                maxScore = score;
                txtMaxScore.text = string.Format("{0,4:D4}", maxScore);
            }
        }
    }

    public void ReduceLife()
    {
        lives--;
        Debug.Log("Vidas restantes: " + lives);
    }

    public void AddScore(int puntuacion)
    {
        score += puntuacion;
    }
}
