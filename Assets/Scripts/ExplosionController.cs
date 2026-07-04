using UnityEngine;

/// <summary>
/// Clase que controla el comportamiento de las explosiones en el juego.
/// Maneja la reproducción del sonido de la explosión y la destrucción del objeto después de un tiempo determinado.
/// </summary>
public class ExplosionController : MonoBehaviour
{
    // Tiempo de espera antes de destruir la explosión
    const float DELAY = 0.25f;

    // Sonido de la explosión
    [SerializeField]
    AudioClip explosionSound;

    /// <summary>
    /// Inicializa la explosión reproduciendo el sonido y programando su destrucción después de un tiempo determinado.
    /// </summary>
    void Start()
    {
        // Reproducir sonido de la explosión en la posición de la cámara
        AudioSource.PlayClipAtPoint(explosionSound, Camera.main.transform.position);
        // Destruir la explosión después de un cierto tiempo
        Destroy(gameObject, DELAY);
    }
}
