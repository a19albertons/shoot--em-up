using System.Collections;
using UnityEngine;

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
    public void SetDelay(float newDelay)
    {
        delay = newDelay;
    }

    public void StopSpawning()
    {
        // Detiene la corutina de generación de naves enemigas si su valor es distinto a null
        if (enemySpawnerCoroutine != null)
        {
            StopCoroutine(enemySpawnerCoroutine);
            enemySpawnerCoroutine = null;
        }
    }

    public void StartSpawning()
    {
        // Inicia la corutina de generación de naves enemigas si su valor es nulo
        if (enemySpawnerCoroutine == null)
        {
            enemySpawnerCoroutine = StartCoroutine("EnemySpawn");
        }
    }
}
