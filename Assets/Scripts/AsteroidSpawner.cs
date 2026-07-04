using System.Collections;
using UnityEngine;

/// <summary>
/// Clase que se encarga de generar asteroides en intervalos regulares.
/// Permite establecer un retraso inicial antes de comenzar la generación y el intervalo
/// </summary>
public class AsteroidsSpawner : MonoBehaviour
{
    // Tiempo entre intervalos de generación de asteroides
    [SerializeField]
    float interval;

    // Tiempo de espera antes de comezar a generar asteroides
    [SerializeField]
    float delay;

    // Prefab de la nave enemiga
    [SerializeField]
    GameObject AsteroidBig;

    // Coordenadas mínima y máxima en el eje X
    const float MIN_X = -4f;
    const float MAX_X = 4f;

    private Coroutine asteroidSpawnCoroutine;

    /// <summary>
    /// Inicia la corutina de generación de asteroides al comenzar el juego.
    /// </summary>
    /// <returns>Devuelve la corutina de generación de asteroides.</returns>
    IEnumerator AsteroidSpawn()
    {
        // Retraso antes de empezar a generar asteroides
        yield return new WaitForSeconds(delay);

        // Generación infinita de asteroides
        while (true)
        {
            // Generar una posición aleatoria en el eje X dentro del rango establecido
            Vector3 position = new Vector3(Random.Range(MIN_X, MAX_X), transform.position.y, 0);

            Debug.Log("Xerando asteroide en: " + position);

            // Instanciar un nuevo asteroide en la posición aleatoria
            Instantiate(AsteroidBig, position, Quaternion.identity);

            // Esperar antes de generar el siguiente asteroide
            yield return new WaitForSeconds(interval);
        }
    }

    /// <summary>
    /// Detiene la generación de asteroides y finaliza la corutina correspondiente.
    /// </summary>
    public void StopSpawning()
    {
        // Detetiene la corutina de generación de asteroides si su valor es distinto a null
        if (asteroidSpawnCoroutine != null)
        {
            // Detiene la corutina de generación de asteroides y la define como nula
            StopCoroutine(asteroidSpawnCoroutine);
            asteroidSpawnCoroutine = null;
        }
    }

    /// <summary>
    /// Inicia la generación de asteroides y comienza la corutina correspondiente.
    /// </summary>
    public void StartSpawning()
    {
        // Inicia la corutina de generación de asteroides si su valor es nulo
        if (asteroidSpawnCoroutine == null)
        {
            asteroidSpawnCoroutine = StartCoroutine("AsteroidSpawn");
        }
    }

    // Función para establecer el retraso antes de comenzar a generar asteroides
    /// <summary>
    /// Establece el retraso antes de comenzar a generar asteroides.
    /// Esta pensada para ser usada desde un script externo, como el GameLogic, para ajustar dinámicamente el retraso según las necesidades del juego.
    /// </summary>
    /// <param name="newDelay"></param>
    public void SetDelay(float newDelay)
    {
        delay = newDelay;
    }
}
