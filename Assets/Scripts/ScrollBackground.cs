using UnityEngine;

/// <summary>
/// Clase que controla el desplazamiento del fondo del juego, creando un efecto de movimiento continuo. Maneja la velocidad de desplazamiento y la reposición de las imágenes cuando salen de la pantalla.
/// </summary>
public class ScrollBackground : MonoBehaviour
{
    // Añadimos un campo para la velocidad a la que se desplazarán las imágenes
    [SerializeField]
    float speed;

    // Altura de la imagen para determinar cuándo pasarla hacia arriba. La inicializaremos en el método Start()
    float height;

    // Start is called before the first frame update
    /// <summary>
    /// Inicializa la altura de la imagen obteniendo el tamaño del SpriteRenderer asociado al objeto. Esto permite calcular cuándo la imagen ha salido completamente de la pantalla para reposicionarla y crear un efecto de desplazamiento continuo.
    /// </summary>
    void Start()
    {
        // Accedemos a la propiedad SpriteRenderer, a su tamaño y su altura
        height = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    // Update is called once per frame
    /// <summary>
    /// Actualiza la posición del fondo en cada frame, moviéndolo hacia abajo a una velocidad determinada. Cuando la imagen sale completamente de la pantalla, la reposiciona en la parte superior para crear un efecto de desplazamiento continuo.
    /// </summary>
    void Update()
    {
        // En qué dirección nos movemos y cuánto movemos. Vector.down (0, -1, 0)
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        // Reposicionamos cuando el centro de la imagen haya recorrido toda su altura
        if (transform.position.y < -height)
        {
            // Queremos desplazar el doble de la altura, porque queremos saltar la imagen que está saliendo por la parte inferior
            transform.Translate(Vector3.up * 2 * height);
        }
    }
}
