# Deseño

## Diagrama da arquitectura

### Diagrama de componentes

```mermaid
flowchart TB
    %% tu
    Cliente[Usuario]

    %% app
    subgraph O dispositivo do usuario
        subgraph Aplicación
            ui[UI]
            Backend[Backend]
        end
        prefs[Prefs]
    end

    %% relacións
    Cliente --> |interactúa| ui
    ui <--> |intercambio de datos| Backend
    Backend --> |consulta e modifica| prefs

    
```

### Diagrama de Despregamento
```mermaid
flowchart TD
    %% nodo
    Usuario[Usuario]
    subgraph O dispositivo/navegador do usuario
        subgraph Sistema operativo o navegador
            Aplicacion[Aplicación]
            prefs[Prefs]

        end
    end

    %% relacións
    Usuario -->|usa| Aplicacion
    Aplicacion -->|usa| prefs

```

## Diagrama de Base de Datos

Non aplica e un valor int almacenado en prefs

## Diagrama de clases

Non aplica, non hai clases definidas no proxecto

## Deseño da interface de usuario

Intentar ser parecido ao matamarcianos orixinal.