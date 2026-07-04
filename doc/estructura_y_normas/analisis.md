# Análise: Requirimentos do sistema

## Descrición xeral

## Casos de uso

```mermaid
flowchart LR
    %% Definición do actor usando un icono
    Usuario((fa:fa-user Usuario))

    subgraph S["Sistema de xestión das tarefas"]
        %% Definición dos casos de uso (forma de píldora/óvalo)
        Disparar([Disparar])
        DestruirMeteorito([DestruirMeteorito])
        DestruirNaveEnemiga([DestruirNaveEnemiga])
        MenuPausa([MenuPausa])
        Reanudar([Reanudar])
        Reiniciar([Reiniciar])
        Salir([Salir])
    



    end

    
    %% Relacións
    Usuario --> Disparar
    Disparar --> DestruirMeteorito
    Disparar --> DestruirNaveEnemiga
    Usuario --> MenuPausa
    MenuPausa --> Reanudar
    MenuPausa --> Reiniciar
    MenuPausa --> Salir



```

## Funcionalidades

### FUNCIONAIS

- Añadir funcionalidades novas e melloradas ao xogo matamarcianos.
- Correción de bugs.

### NON FUNCIONAIS

- Soporte para diferentes resolucións de pantalla.
- Soporte para diferentes idiomas.
- Menu de pausa.

## Tipos de usuarios

- Usuario: pode interactuar ca nave, disparar e superar a sua mellor puntuación.
