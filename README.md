# Rueda la bola :8ball: en Unity
Es un proyecto con base en Unity y el tutorial de [Roll a ball](https://learn.unity.com/project/roll-a-ball), aplicando los cambios pedidos.

## Partes del proyecto

### Personalizar el tablero

De forma incial se le puede agregar colores, al cambiarle su material, en el menu de inspección del objeto. Así como poder agregarles texturas a los objetos es con arrastrar un archivo de imagen (.png) al objecto deseado, o de estar usando un prefab se lo puede aplicar al prefab mismo.

![Escena texturas](media/imagenEscenaTexturas.png)

Así como poder agregarle unas columnas que se dedicaran a obstaculizar al jugador. Estas apareceran de forma aleatoria en el tablero.

![archivo pared](media/imagenColumnaArchivo.png)

### Enemigo complejo

Teniendo al enemigo, se le debe de poner un comportamiento de movimiento, en este caso de le puso el navmesh agent, para que se mueva de forma autonoma por el tablero, pero se le asigno al jugador (la bola) como objetivo para que lo persiga. Un video donde explican el como se usa el navmesh agent es [este](https://www.youtube.com/watch?v=CHV1ymlw-P8).

Para darle un poco mas de complejidad, se le dio la habilidad de aumentar su numero al enemigo debido a su reducido movimiento.
```csharp
void SpawnEnemy()
    {
        // Genera un nuevo enemigo
        Instantiate(enemyPrefab, transform.position, Quaternion.identity);
    }
```

Así como el darle la habilidad de que cuando el jugador salte el enemigo iguale su altura, evitando así que el jugador salte sobre ellos, y los ignore.

![Escena Enemigos aereos](media/imagenMultiplesEnemigoAereo.png)

La opción mas avanzada del enemigo, es que cuando el jugador se aleje mucho del enemigo, este aparecera en su ubicación aproximada, dando como resultado que el jugador no pueda escapar de el, y que el jugador intente regular su velocidad.
```csharp	
void MoveEnemy()
    {
        // Para ver si el enemigo esta cerca o no del jugador
        float distanceToPlayer = Vector3.Distance(transform.position, target.position);

        if (distanceToPlayer > 10)
        {
            // Para obtener una posición aproximada del jugador
            float randomX = Random.Range(target.position.x - 5, target.position.x + 5);
            float randomZ = Random.Range(target.position.z - 5, target.position.z + 5);

            // Mover el enemigo a la posición aproximada del jugador
            transform.position = new Vector3(randomX , target.position.y, randomZ);
        }
    }
```