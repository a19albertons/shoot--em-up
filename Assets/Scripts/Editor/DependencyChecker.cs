using System;
using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public static class DependencyChecker
{
    private static ListRequest listRequest;
    // Actualiza las dependencias de los paquetes instalados a la versión más reciente compatible con la versión actual del editor de Unity.

    [MenuItem("Tools/Update Dependencies Safely")]
    public static void UpdateDependencies()
    {
        Debug.Log("Iniciando búsqueda de actualizaciones compatibles...");
        // Listamos solo los paquetes instalados directamente en el proyecto
        listRequest = Client.List(offlineMode: false, includeIndirectDependencies: false);
        EditorApplication.update += Progress;
    }

    private static void Progress()
    {
        if (listRequest.IsCompleted)
        {
            EditorApplication.update -= Progress;

            if (listRequest.Status == StatusCode.Success)
            {
                bool changesMade = false;

                // Leemos el manifest actual para modificarlo
                string manifestPath = "Packages/manifest.json";
                string manifestText = File.ReadAllText(manifestPath);


                foreach (var package in listRequest.Result)
                {
                    string currentVersion = package.version;
                    // Unity nos dice de forma nativa cuál es la versión máxima compatible con ESTE editor
                    string latestCompatible = package.versions.latestCompatible;

                    Debug.Log($"[CHECK] {package.name}: current={currentVersion} latestCompatible={latestCompatible ?? "NULL/EMPTY"}");


                    if (!string.IsNullOrEmpty(latestCompatible) && currentVersion != latestCompatible)
                    {
                        Debug.Log($"[UPDATE] {package.name}: {currentVersion} -> {latestCompatible}");
                        // Reemplazamos la versión vieja por la compatible en el manifest.json
                        manifestText = manifestText.Replace(
                            $"\"{package.name}\": \"{currentVersion}\"",
                            $"\"{package.name}\": \"{latestCompatible}\""
                        );
                        changesMade = true;
                    }
                }

                // Guardamos los cambios en el manifest.json si se realizaron actualizaciones
                // Generamos un log para informar al usuario en el ci cd
                if (changesMade)
                {
                    File.WriteAllText(manifestPath, manifestText);
                    Debug.Log("Se han guardado las actualizaciones compatibles en manifest.json.");
                }
                else
                {
                    Debug.Log("Todos los paquetes ya están en su versión compatible más reciente.");
                }
            }
            // En caso de error al procesar el manifest.json, mostramos un mensaje de error en la consola
            else
            {
                Debug.LogError("Error al listar los paquetes: " + listRequest.Error.message);
            }

            if (Application.isBatchMode)
            {
                EditorApplication.Exit(0);
            }
        }
    }
}