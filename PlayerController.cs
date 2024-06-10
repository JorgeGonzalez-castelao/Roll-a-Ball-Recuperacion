using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private float movementX;
    private float movementY;
	
	// contador
	private int count;
    
    // velocidad de la bola
    public float speed = 0;

	// objeto conn los datos importantes
	public TextMeshProUGUI countText;

	// texto que aperece una vez ganado
	public GameObject winTextObject;

	// para el prefab de los pickups
	public GameObject pickupPrefab;

	// para el prefab de las columnas
	public GameObject ColumnsPrefab;

	// para el prefab de las los enemigos
	public GameObject EnemyPrefab;

    // cuan pocos pickups pueden aparecer
	public int minPickups = 5; 

    // cuan muchos pickups pueden aparecer
    public int maxPickups = 10;
	
	// fuerza del salto
	private float jumpForce = 5.0f;
	
	// variable local para saber si la pelota está saltando
    private bool isJumping = false;

	// variable para del animator
	Animator anim;

	// Al saltar que pasara
    void OnJump()
    {
		rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
        
		// cambiamos el valor de la variable isJumping  a true
		isJumping = true;
    }
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
		anim = GetComponent<Animator>();

		count = 0;

		SpawnPickupsRandomly();
		SpawnColumnsRandomly();
        SpawnEnemy();
		SetCountText();
		
        // Desactivamos el cuadro de victoria
        winTextObject.SetActive(false);
        StartCoroutine(SpeedReduction());
    }

    int CountPickupsLeft(string tag = "PickUp")
    {
        // Cuenta el numero de objetos en la escena
        GameObject[] pickups = GameObject.FindGameObjectsWithTag("PickUp");

        int pickupCount = pickups.Length;
        // si el contador es igual a 0 entonces se vuelve verdaderdo el hasWin del animator
        if (pickupCount == 0)
        {
            anim.SetBool("hasWin", true);
        }
		return pickupCount;
    }

    float CountSpeed()
    {
        // Cuenta la velocidad de la bola
        float speedNow = speed;
		return speedNow;
    }

    private void FixedUpdate(){
        Vector3 movement = new Vector3 (movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);

        OutOfBounds();
		hasToBeAlert();
    }

    void OnMove(InputValue movementValue){
        Vector2 movementVector = movementValue.Get<Vector2>();
        Debug.Log(movementVector);
        
        movementX = movementVector.x; 
        movementY = movementVector.y;
        
    }
    void OutOfBounds()
    {
        // Si la bola se sale del plano, se reinicia
        if (transform.position.y < -10){
            transform.position = new Vector3(0, 0.5f, 0);
            rb.velocity = Vector3.zero;
        }
    }

	void SetCountText()
	{
		countText.text = "Count: " + count.ToString();
		countText.text = countText.text + "\nPick Ups left: " + CountPickupsLeft().ToString();
		countText.text = countText.text + "\nSpeed: " + speed.ToString() ;
		hasWin();
	}

	void OnTriggerEnter(Collider other){
        if (other.gameObject.CompareTag("PickUp")){
            other.gameObject.SetActive(false);
            Debug.Log("Coin collected!");
			count++;
			speed = speed + 10;
			SetCountText();
        }
    }

	void SpawnPickupsRandomly()
    {
        int numPickupsToSpawn = Random.Range(minPickups, maxPickups + 1);

        for (int i = 0; i < numPickupsToSpawn; i++)
        {
            // Spawn de un objeto en una posición aleatoria
            Vector3 spawnPosition = new Vector3(Random.Range(-10f, 10f), 0.5f, Random.Range(-10f, 10f));

            Instantiate(pickupPrefab, spawnPosition, Quaternion.identity);
        }
    }
	void SpawnColumnsRandomly()
    {
        int numColumnsToSpawn = Random.Range(minPickups, maxPickups + 1);

        for (int i = 0; i < numColumnsToSpawn; i++)
        {
            // Spawn de un objeto en una posición aleatoria
            Vector3 spawnPosition = new Vector3(Random.Range(-10f, 10f), 0.5f, Random.Range(-10f, 10f));

            Instantiate(ColumnsPrefab, spawnPosition, Quaternion.identity);
        }
    }
	void SpawnEnemy()
    {
        Vector3 spawnPosition = new Vector3(0, 0.5f, 3);
        Instantiate(EnemyPrefab, spawnPosition, Quaternion.identity);        
    }


    // function that every second reduce in 1 the speed of the player
    IEnumerator SpeedReduction()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            speed = speed - 1;
            SetCountText();
        }
    }

    // Funcion que vuelve verdaderdo el animator si el enemigo se encuentra a menos de 1 uniades de distancia del jugador
    void hasToBeAlert()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, EnemyPrefab.transform.position);
        if (distanceToPlayer <= 3)
        {
            anim.SetBool("alert", true);
        }

        if(distanceToPlayer > 3 )
        {
            anim.SetBool("alert", false);
        }
    }

    // Funcion que vuelve verdaderdo el animator si el enemigo se encuentra a menos de 1 uniades de distancia del jugador
    void hasWin()
    {
       if (CountPickupsLeft() == 0)
        {
            winTextObject.SetActive(true);
        }
    }
}
