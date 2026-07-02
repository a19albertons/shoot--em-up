using System;
using System.IO;
using System.Threading;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public static class DependencyChecker
{
    private static bool isResolving = false;

    [MenuItem("Tools/Update Dependencies Safely")]
    public static void UpdateDependencies()
    {
        Debug.Log("Iniciando búsqueda de actualizaciones compatibles...");

        // Lanzamos la petición de forma asíncrona, pero la esperaremos de forma síncrona
        var listRequest = Client.List(offlineMode: false, includeIndirectDependencies: false);

        // Bucle de espera síncrono obligatorio para Batchmode + -quit
        int timeoutMs = 45000; // 45 segundos de margen
        int elapsedMs = 0;
        // Bucle de espera especifico para el primer paso que es actualizar el manifest.json
        while (!listRequest.IsCompleted)
        {
            Thread.Sleep(100);
            elapsedMs += 100;
            if (elapsedMs >= timeoutMs)
            {
                Debug.LogError("Error: Tiempo de espera agotado al consultar el Package Manager.");
                if (Application.isBatchMode) EditorApplication.Exit(1);
                return;
            }
        }

        if (listRequest.Status == StatusCode.Success)
        {
            bool changesMade = false;
            string manifestPath = "Packages/manifest.json";
            string manifestText = File.ReadAllText(manifestPath);

            // Comprueba paquete a paquete si hay una versión compatible más reciente y actualiza el manifest.json
            foreach (var package in listRequest.Result)
            {
                string currentVersion = package.version;
                string latestCompatible = package.versions.latestCompatible;

                if (!string.IsNullOrEmpty(latestCompatible) && currentVersion != latestCompatible)
                {
                    Debug.Log($"[UPDATE] {package.name}: {currentVersion} -> {latestCompatible}");
                    manifestText = manifestText.Replace(
                        $"\"{package.name}\": \"{currentVersion}\"", 
                        $"\"{package.name}\": \"{latestCompatible}\""
                    );
                    changesMade = true;
                }
            }

            // Se ejecuta si hay cambios en el manifest.json, guardando los cambios y forzando la resolución de paquetes para actualizar packages-lock.json
            if (changesMade)
            {
                File.WriteAllText(manifestPath, manifestText);
                Debug.Log("Se han guardado las actualizaciones compatibles en manifest.json.");
                
                // Forzar resolución síncrona para actualizar packages-lock.json ---
                Debug.Log("Forzando la resolución del Package Manager para regenerar packages-lock.json...");
                isResolving = true;
                
                // Nos suscribimos al evento que nos avisa cuando finaliza la importación de paquetes
                Events.registeredPackages += OnRegisteredPackages;
                
                // Disparamos la resolución
                Client.Resolve();

                // Espera síncrona para el entorno Batchmode
                int resolveTimeoutMs = 180000; // 180 segundos de margen para descargar y resolver
                int resolveElapsedMs = 0;
                while (isResolving)
                {
                    Thread.Sleep(100);
                    resolveElapsedMs += 100;
                    if (resolveElapsedMs >= resolveTimeoutMs)
                    {
                        Debug.LogWarning("Tiempo de espera agotado esperando a que finalice la resolución de paquetes.");
                        break;
                    }
                }
                
                // Nos desuscribimos para limpiar el evento
                Events.registeredPackages -= OnRegisteredPackages;
                // ----------------------------------------------------------------------------

                // Forzamos la actualización del AssetDatabase antes de cerrar
                AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
                Debug.Log("Proceso completado. packages-lock.json actualizado con éxito.");
            }
            else
            {
                // Log si todos estan actualizados
                Debug.Log("Todos los paquetes ya están en su versión compatible más reciente.");
            }
        }
        else
        {
            // Log en caso de error al listar los paquetes
            Debug.LogError("Error al listar los paquetes: " + listRequest.Error.message);
            if (Application.isBatchMode) EditorApplication.Exit(1);
            return;
        }

        if (Application.isBatchMode)
        {
            EditorApplication.Exit(0);
        }
    }

    private static void OnRegisteredPackages(PackageRegistrationEventArgs args)
    {
        // En cuanto el Package Manager termina de registrar y escribir el archivo lock, liberamos el bucle
        isResolving = false;
    }
}