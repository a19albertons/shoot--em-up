using UnityEngine;

/// <summary>
/// Clase que controla el comportamiento de los disparos en el juego.
/// Maneja la velocidad de los disparos, la destrucción al salir de la pantalla y las colisiones con enemigos.
/// </summary>
public class ShootController : MonoBehaviour
{
    // Velocidad de los disparos
    [SerializeField]
    float speed;

    [SerializeField]
    GameObject hit; // Efecto de impacto al colisionar

    // Tiempo que duran los disparos antes de autodestruirse
    [SerializeField]
    float lifetime;

    /// <summary>
    /// Inicializa el disparo programando su destrucción después de un tiempo determinado.
    /// </summary>
    void Start()
    {
        // Destruir el disparo después de un cierto tiempo
        Destroy(gameObject, lifetime);
    }

    /// <summary>
    /// Actualiza la posición del disparo moviéndolo hacia arriba en cada frame.
    /// Se llama en cada frame para asegurar un movimiento suave y consistente.
    /// </summary>
    void Update()
    {
        // Mover el disparo hacia arriba
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    // Método para destruir el disparo cuando sale de la pantalla
    /// <summary>
    /// Destruye el disparo cuando sale de la pantalla, evitando que permanezca en memoria innecesariamente.
    /// Se llama automáticamente cuando el objeto deja de ser visible por la cámara.
    /// </summary>
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Detecta colisiones con enemigos y, si se produce una colisión, añade puntos al jugador, instancia un efecto de impacto y destruye el disparo.
    /// Se llama automáticamente cuando el disparo entra en contacto con un objeto que tiene un Collider2D y está marcado como "Enemy".
    /// </summary>
    /// <param name="other">Objeto con el que colisiona</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            GameManager.GetInstance().AddScore(50); // Añadir puntos al destruir un enemigo
            Instantiate(hit, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        // Debug.Log("Colisión con disparo");
    }
}
