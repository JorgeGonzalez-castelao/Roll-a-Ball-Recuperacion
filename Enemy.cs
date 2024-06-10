using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent pathfinder;

    private Transform target;

    public GameObject enemyPrefab;
    
	// para el control del jugador
	public GameObject Player;

    // para el control del jugador
    public Rigidbody PlayerRb;

    // animator
    Animator anim;

    void Start()
    {
        // Obtiene el componente NavMeshAgent del objeto actual y lo asigna a la variable pathfinder.
        pathfinder = GetComponent<NavMeshAgent>();
        
        anim = GetComponent<Animator>();
        
        // busca a player y obtiene su transform
        target = GameObject.Find("Player").transform;

        // Llama a la función "SpawnEnemy" repetidamente cada 10 segundos, comenzando después de 10 segundos.
        InvokeRepeating("SpawnEnemy", 15f, 15f);
    }

// Update es una función de Unity que se llama una vez por frame.
    void Update()
    {
        // Configura el destino del pathfinder para que sea la posición del target. Esto hará que el objeto se mueva hacia el target.
        pathfinder.SetDestination(target.position);

        
        // Calcula la distancia entre el enemigo y el jugador
        float distanceToPlayer = Vector3.Distance(transform.position, target.position);

        // si el jugador tiene una altura diferente a cero, entonces se le asigna la posición del jugador
        if (target.position.y > 0)
        {
            transform.position = new Vector3(transform.position.x , target.position.y, transform.position.z);
            Debug.Log("El jugador tiene una altura diferente a cero");
        }
        
        MoveEnemy();
        
        // Imprime la posición del target en la consola de Unity para fines de depuración.
        Debug.Log(target.position); 
    }
    void SpawnEnemy()
    {
        // Spawn a new enemy
        Instantiate(enemyPrefab, transform.position, Quaternion.identity);
    }

    // function to move the enemy near the player if the player is more than 10 units away
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
}