using System;
using System.IO;
using System.Threading;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public static class DependencyChecker
{
    [MenuItem("Tools/Update Dependencies Safely")]
    public static void UpdateDependencies()
    {
        Debug.Log("Iniciando búsqueda de actualizaciones compatibles...");

        // Lanzamos la petición de forma asíncrona, pero la esperaremos de forma síncrona
        var listRequest = Client.List(offlineMode: false, includeIndirectDependencies: false);

        // Bucle de espera síncrono obligatorio para Batchmode + -quit
        // Esto detiene la ejecución del hilo principal de Unity hasta recibir respuesta del Package Manager
        int timeoutMs = 45000; // 45 segundos de margen
        int elapsedMs = 0;
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

            if (changesMade)
            {
                File.WriteAllText(manifestPath, manifestText);
                Debug.Log("Se han guardado las actualizaciones compatibles en manifest.json.");
                
                // Forzamos la actualización inmediata del AssetDatabase antes de cerrar
                AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            }
            else
            {
                Debug.Log("Todos los paquetes ya están en su versión compatible más reciente.");
            }
        }
        else
        {
            Debug.LogError("Error al listar los paquetes: " + listRequest.Error.message);
            if (Application.isBatchMode) EditorApplication.Exit(1);
            return;
        }

        if (Application.isBatchMode)
        {
            EditorApplication.Exit(0);
        }
    }
}