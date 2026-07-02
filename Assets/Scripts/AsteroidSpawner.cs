using System.Collections;
using UnityEngine;

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

    public void StartSpawning()
    {
        // Inicia la corutina de generación de asteroides si su valor es nulo
        if (asteroidSpawnCoroutine == null)
        {
            asteroidSpawnCoroutine = StartCoroutine("AsteroidSpawn");
        }
    }

    // Función para establecer el retraso antes de comenzar a generar asteroides
    public void SetDelay(float newDelay)
    {
        delay = newDelay;
    }
}
