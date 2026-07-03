using UnityEngine;

/// <summary>
/// Clase que controla el desplazamiento del fondo del juego utilizando un shader, creando un efecto de movimiento continuo. Maneja la velocidad de desplazamiento y aplica el desplazamiento a la textura del material del objeto.
/// </summary>
public class ScrollBackgroundShader : MonoBehaviour
{
    // Velocidad a la que se desplazarán las imágenes
    [SerializeField]
    float speed;

    // Referencia al renderizador del objeto
    Renderer render;

    // Start se llama antes del primer frame update
    /// <summary>
    /// Inicializa el componente Renderer del objeto para poder manipular la textura del material y aplicar el desplazamiento continuo.
    /// </summary>
    void Start()
    {
        // Inicializamos el componente
        render = GetComponent<Renderer>();
    }

    // Update se llama una vez por frame
    /// <summary>
    /// Actualiza el desplazamiento de la textura del material en cada frame, creando un efecto de movimiento continuo. Calcula el offset basado en la velocidad y el tiempo transcurrido, y lo aplica al material del objeto.
    /// </summary>
    void Update()
    {
        // Vector que representa la cantidad de desplazamiento de la textura
        Vector2 offset = Vector2.up * speed * Time.time;

        // Accedemos al material del objeto y aplicamos el desplazamiento
        render.material.mainTextureOffset = offset;
    }
}
