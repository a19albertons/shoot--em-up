using UnityEngine;

/// <summary>
/// Clase que controla el comportamiento de los impactos en el juego.
/// Maneja la reproducción del sonido de impacto y la destrucción del objeto después de un tiempo determinado.
/// </summary>
public class HitController : MonoBehaviour
{
    const float DELAY = 0.25f;

    [SerializeField]
    AudioClip clip;

    /// <summary>
    /// Inicializa el impacto reproduciendo el sonido y programando su destrucción después de un tiempo determinado.
    /// </summary>
    void Start()
    {
        // Reproducimos un sonido
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
        // Pasado un tiempo determinado, el objeto se destruirá
        Destroy(gameObject, DELAY);
    }
}
