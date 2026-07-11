using System.Collections;
using UnityEngine;

/// <summary>
/// Clase que controla el comportamiento de la nave del jugador en el juego.
/// Maneja el movimiento, el disparo, la colisión con enemigos y asteroides, y la animación de inicio con parpadeo.
/// </summary>
public class ShipController : MonoBehaviour
{
    [SerializeField]
    private float force = 5f; // Fuerza del movimiento

    private Rigidbody2D rb; // Referencia al componente Rigidbody

    [SerializeField]
    private Vector3 endPosition; // Posición final de la nave al inicio

    private float duration; // Duración de la transición al inicio
    private bool active = false; // Variable para determinar si se puede realizar alguna acción

    [SerializeField]
    int blinkNum; // Número de parpadeos al inicio

    // Referencia al prefab del disparo
    [SerializeField]
    GameObject shootPrefab;

    // Distancia desde el centro de la nave hasta la posición donde se creará el disparo
    [SerializeField]
    float shootOffset = 0.5f;

    [SerializeField]
    GameObject explosion;
    Vector3 initialPosition; // Posición inicial de la nave

    [Tooltip("Arrastra aquí el objeto TrailRenderer que usa la nave")]
    [SerializeField]
    GameObject trail; // Referencia al objeto TrailRenderer

    private Coroutine startPlayerCoroutine; // Corutina para el inicio de la nave

    /// <summary>
    /// Inicializa la nave del jugador, guardando su posición inicial y obteniendo el componente Rigidbody2D para controlar el movimiento.
    /// </summary>
    void Start()
    {
        initialPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Inicia la corutina que controla la animación de inicio de la nave, haciendo que parpadee y se mueva a la posición final. Durante este tiempo, las colisiones están desactivadas y la nave no puede realizar acciones.
    /// </summary>
    /// <returns>La corutina de inicio de la nave</returns>
    IEnumerator StartPlayer()
    {
        Material mat = GetComponent<SpriteRenderer>().material;
        Color color = mat.color;
        Collider2D collider = GetComponent<Collider2D>();
        collider.enabled = false;
        Vector3 initialPosition = transform.position;
        float t = 0,
            t2 = 0; // Tiempos que van transcurriendo en cada uno de los distintos intervalos

        while (t < duration)
        {
            t += Time.deltaTime;
            Vector3 newPosition = Vector3.Lerp(initialPosition, endPosition, t / duration);
            transform.position = newPosition;

            t2 += Time.deltaTime;
            float newAlpha = blinkNum * (t2 / duration);
            if (newAlpha > 1)
            {
                t2 = 0;
            }
            color.a = newAlpha;
            mat.color = color;
            yield return null;
        }

        // Volvemos a activar las colisiones
        color.a = 1;
        mat.color = color;
        collider.enabled = true;
        active = true;
    }

    /// <summary>
    /// Habilita la inercia en la nave, permitiendo que continúe moviéndose después de soltar las teclas de dirección.
    /// Se llama en cada frame fijo para asegurar un movimiento suave y consistente.
    /// </summary>
    private void FixedUpdate()
    {
        if (active)
            CheckMove(); // Llamamos al método para comprobar el movimiento
    }

    /// <summary>
    /// Comprueba los ejes para calcular la inercia de la nave
    /// </summary>
    private void CheckMove()
    {
        // Obtenemos la dirección del movimiento en los ejes horizontal y vertical
        Vector2 direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        direction.Normalize(); // Normalizamos el vector para que tenga magnitud 1

        // Aplicamos una fuerza en la dirección obtenida
        rb.AddForce(direction * force, ForceMode2D.Impulse);
    }

    /// <summary>
    /// Comprueba si se ha pulsado la tecla de disparo (barra espaciadora) y, si la nave está activa, instancia un disparo en la posición correspondiente.
    /// Se llama en cada frame para detectar la entrada del jugador.
    /// </summary>
    void Update()
    {
        // Comprobar si la nave está activa y se ha pulsado la tecla de disparo (barra espaciadora)
        if (active && Input.GetKeyDown(KeyCode.Space) && Time.timeScale > 0)
        {
            // Calcular la posición donde se creará el disparo (un poco por delante de la nave)
            Vector3 shootPosition = transform.position + Vector3.up * shootOffset;

            // Crear el disparo en la posición calculada y sin rotación
            Instantiate(shootPrefab, shootPosition, Quaternion.identity);
        }
    }

    /// <summary>
    /// Detecta colisiones con enemigos o asteroides y, si la nave está activa, reduce la vida del jugador y destruye la nave, generando una explosión en su posición.
    /// </summary>
    /// <param name="other">Objeto con el que colisiona</param>
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "AsteroidBig") // Añadido extra
        {
            Debug.Log("Colisión con nave enemiga");
            GameManager.GetInstance().ReduceLife(); // Reducir vida en el GameManager
            DestroyShip(); // Destruir la nave
        }
    }

    /// <summary>
    /// Desactiva la nave del jugador, genera una explosión en su posición y evita que pueda realizar acciones mientras está destruida.
    /// </summary>
    void DestroyShip()
    {
        // Desactivar comportamiento
        active = false;
        // Instanciar la animación de la explosión
        Instantiate(explosion, transform.position, Quaternion.identity);
    }

    // Función para establecer la duración de la transición al inicio
    // a nivel interno se reutiliza el delay de game logic
    /// <summary>
    /// Establece la duración de la transición de inicio de la nave, que determina cuánto tiempo tarda en moverse a su posición final y parpadear. Esta función permite ajustar dinámicamente la duración según las necesidades del juego.
    /// </summary>
    /// <param name="newDuration">El tiempo de duración de la transición</param>
    public void SetDuration(float newDuration)
    {
        duration = newDuration;
    }

    /// <summary>
    /// Detiene la corutina de inicio de la nave, resetea su posición a la inicial y limpia el TrailRenderer para evitar que se vea la estela de la nave al reiniciar. Esto asegura que la nave esté lista para reiniciar el juego sin efectos visuales residuales.
    /// </summary>
    public void StopStartPlayer()
    {
        // Detiene la corutina de la nave si su valor es distinto a null
        if (startPlayerCoroutine != null)
        {
            // Resetear posición de la nave
            transform.position = initialPosition;
            trail.GetComponent<TrailRenderer>().Clear(); // Limpia el TrailRenderer para evitar que se vea la estela de la nave al reiniciar
            StopCoroutine(startPlayerCoroutine);
            startPlayerCoroutine = null;
        }
    }

    /// <summary>
    /// Inicia la corutina de inicio de la nave, que controla la animación de parpadeo y el movimiento a la posición final. Durante este tiempo, las colisiones están desactivadas y la nave no puede realizar acciones.
    /// </summary>
    public void StartStartPlayer()
    {
        // Inicia la corutina de la nave si su valor es nulo
        if (startPlayerCoroutine == null)
        {
            startPlayerCoroutine = StartCoroutine("StartPlayer");
        }
    }
}
