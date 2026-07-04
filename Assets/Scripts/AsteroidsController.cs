using UnityEngine;

/// <summary>
/// Clase que controla el comportamiento de los asteroides en el juego.
/// Maneja la velocidad aleatoria, la destrucción al salir de la pantalla y las colisiones con el jugador o disparos.
/// </summary>
public class AsteroidsController : MonoBehaviour
{
    [SerializeField]
    float minSpeedY; // caída vertical

    [SerializeField]
    float maxSpeedY;

    [SerializeField]
    float minSpeedX; // movemento lateral

    [SerializeField]
    float maxSpeedX;

    [SerializeField]
    GameObject explosionPrefab;

    Rigidbody2D rb;

    const float DESTROY_Y = -7f;

    /// <summary>
    /// Inicializa el asteroide con una velocidad aleatoria y configura su comportamiento.
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Velocidade aleatoria en X e Y
        float speedY = Random.Range(minSpeedY, maxSpeedY);
        float speedX = Random.Range(minSpeedX, maxSpeedX);

        rb.linearVelocity = new Vector2(speedX, -speedY);
    }

    /// <summary>
    /// Comprueba la posición del asteroide y lo destruye si sale de la pantalla.
    /// </summary>
    void Update()
    {
        if (transform.position.y < DESTROY_Y)
        {
            Destroy(gameObject);
        }
    }

    // Removido la alternativa de un disparo por ser trigger no collision
    /// <summary>
    /// Detecta colisiones con el jugador y dispara la explosión correspondiente.
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Explode();
        }
    }

    // Coherencia con el disparo es tipo trigger no collision normal
    /// <summary>
    /// Detecta colisiones con disparos y dispara la explosión correspondiente.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Shoot"))
        {
            Explode();
        }
    }

    /// <summary>
    /// Genera una explosión en la posición del asteroide y destruye el objeto.
    /// </summary>
    void Explode()
    {
        if (explosionPrefab != null)
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
