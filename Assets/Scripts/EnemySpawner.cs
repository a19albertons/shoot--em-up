using System.Collections;
using UnityEngine;

/// <summary>
/// Clase que se encarga de generar naves enemigas en intervalos regulares.
/// Permite establecer un retraso inicial antes de comenzar la generación y el intervalo
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    // Tiempo entre intervalos de generación de naves enemigas
    [SerializeField]
    float interval;

    // Tiempo de espera antes de comenzar a generar naves enemigas
    float delay;

    // Prefab de la nave enemiga
    [SerializeField]
    GameObject enemy;

    [SerializeField]
    private Coroutine enemySpawnerCoroutine;

    // Coordenadas mínima y máxima en el eje X
    const float MIN_X = -3.5f;
    const float MAX_X = 3.5f;

    /// <summary>
    /// Inicia la corutina de generación de naves enemigas al comenzar el juego.
    /// </summary>
    /// <returns>Devuelve la corutina de generación de naves enemigas.</returns>
    IEnumerator EnemySpawn()
    {
        // Retraso antes de comenzar a generar naves enemigas
        yield return new WaitForSeconds(delay);

        // Generación infinita de naves enemigas
        while (true)
        {
            // Generar una posición aleatoria en el eje X dentro del rango establecido
            Vector3 position = new Vector3(Random.Range(MIN_X, MAX_X), transform.position.y, 0);

            // Instanciar una nueva nave enemiga en la posición aleatoria
            Instantiate(enemy, position, Quaternion.identity);

            // Esperar antes de generar la siguiente nave enemiga
            yield return new WaitForSeconds(interval);
        }
    }

    // Función para establecer el retraso antes de comenzar a generar naves enemigas
    /// <summary>
    /// Establece el retraso antes de comenzar a generar naves enemigas.
    /// Esta pensada para ser usada desde un script externo, como el GameLogic, para ajustar dinámicamente el retraso según las necesidades del juego.
    /// </summary>
    /// <param name="newDelay"></param>
    public void SetDelay(float newDelay)
    {
        delay = newDelay;
    }

    /// <summary>
    /// Detiene la generación de naves enemigas y finaliza la corutina correspondiente.
    /// </summary>
    public void StopSpawning()
    {
        // Detiene la corutina de generación de naves enemigas si su valor es distinto a null
        if (enemySpawnerCoroutine != null)
        {
            StopCoroutine(enemySpawnerCoroutine);
            enemySpawnerCoroutine = null;
        }
    }

    /// <summary>
    /// Inicia la generación de naves enemigas y comienza la corutina correspondiente.
    /// </summary>
    public void StartSpawning()
    {
        // Inicia la corutina de generación de naves enemigas si su valor es nulo
        if (enemySpawnerCoroutine == null)
        {
            enemySpawnerCoroutine = StartCoroutine("EnemySpawn");
        }
    }
}
