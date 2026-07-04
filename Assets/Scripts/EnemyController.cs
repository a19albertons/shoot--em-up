using UnityEngine;

/// <summary>
/// Clase que controla el comportamiento de las naves enemigas en el juego.
/// Maneja la velocidad de caída, la destrucción al salir de la pantalla y las colisiones con el jugador o disparos.
/// </summary>
public class EnemyController : MonoBehaviour
{
    // Velocidad de caída de la nave enemiga
    [SerializeField]
    float speed;

    [SerializeField]
    GameObject explosionPrefab; // Prefab de la explosión

    // Altura a la que se destruirá la nave enemiga
    const float DESTROY_HEIGHT = -6f;

    /// <summary>
    /// Actualiza la posición de la nave enemiga y la destruye si sale de la pantalla.
    /// </summary>
    void Update()
    {
        // Movimiento hacia abajo
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        // Destruir la nave enemiga cuando alcanza la altura de destrucción
        if (transform.position.y < DESTROY_HEIGHT)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Detecta colisiones con disparos y dispara la explosión correspondiente.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Colisión con nave enemiga");
        Destroy(gameObject);
        DestroyEnemy(); // Destruir la nave enemiga
    }

    /// <summary>
    /// Genera una explosión en la posición de la nave enemiga y destruye el objeto.
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Colisión con nave enemiga");
        DestroyEnemy(); // Destruir la nave enemiga
    }

    // Destruir la nave enemiga y crear una explosión
    /// <summary>
    /// Genera una explosión en la posición de la nave enemiga y destruye el objeto.
    /// </summary>
    void DestroyEnemy()
    {
        // Instanciar la animación de la explosión en la posición de la nave enemiga
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        // Destruir la nave enemiga
        Destroy(gameObject);
    }
}
