using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager instance;

    int lives = 3;

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
            foreach (GameObject obj in deshabilitar)
            {
                obj.SetActive(false);
            }
        }
    }

    public void ReduceLife()
    {
        lives--;
        Debug.Log("Vidas restantes: " + lives);
        while (lives >= 0 && lives < imgLives.Length)
        {
            imgLives[lives].SetActive(false);
            break;
        }
    }
}
