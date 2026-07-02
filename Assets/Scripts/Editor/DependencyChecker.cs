using System;
using System.Threading;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

// Script realizado por qwen 3.7 plus
public static class DependencyChecker
{
    [MenuItem("Tools/Update Dependencies Safely")]
    public static void UpdateDependencies()
    {
        Debug.Log("Iniciando búsqueda de actualizaciones compatibles...");

        // 1. Listar paquetes actuales
        var listRequest = Client.List(offlineMode: false, includeIndirectDependencies: false);

        int timeoutMs = 45000; // 45 segundos de margen
        int elapsedMs = 0;
        
        // Gestiona no exceder el tiempo de espera al consultar el Package Manager
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

        // Si la petición fue exitosa, procedemos a comprobar y actualizar versiones
        if (listRequest.Status == StatusCode.Success)
        {
            bool changesMade = false;

            // 2. Comprobar y actualizar versiones usando la API oficial (Client.Add)
            foreach (var package in listRequest.Result)
            {
                string currentVersion = package.version;
                string latestCompatible = package.versions.latestCompatible;

                // Comprueba si cada librería tiene una versión compatible más reciente y actualiza si es necesario
                if (!string.IsNullOrEmpty(latestCompatible) && currentVersion != latestCompatible)
                {
                    Debug.Log($"[UPDATE] {package.name}: {currentVersion} -> {latestCompatible}");
                    
                    // Client.Add() actualiza automáticamente manifest.json y packages-lock.json
                    var addRequest = Client.Add($"{package.name}@{latestCompatible}");
                    
                    int addTimeoutMs = 180000; // 3 minutos de margen por paquete
                    int addElapsedMs = 0;
                    
                    // Espera a que la petición de actualización se complete o se agote el tiempo
                    while (!addRequest.IsCompleted)
                    {
                        Thread.Sleep(100);
                        addElapsedMs += 100;
                        if (addElapsedMs >= addTimeoutMs)
                        {
                            Debug.LogError($"Tiempo de espera agotado al actualizar {package.name}.");
                            break;
                        }
                    }
                    
                    // Verifica el resultado de la actualización
                    if (addRequest.Status == StatusCode.Success)
                    {
                        Debug.Log($"[SUCCESS] {package.name} actualizado correctamente.");
                        changesMade = true;
                    }
                    else
                    {
                        // Manejo seguro de errores por si Error es null
                        string errorMsg = addRequest.Error != null ? addRequest.Error.message : "Unknown error";
                        Debug.LogError($"[ERROR] Fallo al actualizar {package.name}: {errorMsg}");
                    }
                }
            }

            if (changesMade)
            {
                // Forzamos la actualización del AssetDatabase para importar los nuevos archivos
                AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
                Debug.Log("Proceso completado. manifest.json y packages-lock.json actualizados con éxito.");
            }
            else
            {
                Debug.Log("Todos los paquetes ya están en su versión compatible más reciente.");
            }
        }
        else
        {
            // Log de error seguro por si Error es null
            string errorMsg = listRequest.Error != null ? listRequest.Error.message : "Unknown error";
            Debug.LogError("Error al listar los paquetes: " + errorMsg);
            if (Application.isBatchMode) EditorApplication.Exit(1);
            return;
        }

        if (Application.isBatchMode)
        {
            EditorApplication.Exit(0);
        }
    }
}