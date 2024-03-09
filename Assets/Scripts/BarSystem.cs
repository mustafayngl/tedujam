using System;
using UnityEngine;
using UnityEngine.Android;


// this class is responsible for managing the bar system and attached to Line object
public class BarSystem : MonoBehaviour
{
    // Singleton instance
    public static BarSystem Instance { get; private set; }

    public Transform line; // Çizgi objesinin referansı
    public Transform bar; // Bar objesinin referansı
    public GameObject redArea; // Kırmızı alan objesinin referansı

    private bool inRedArea; // Çizgi kırmızı alanda mı?
    private bool gameWon; // Oyun kazanıldı mı?


    // Draw Speeds
    public int playerDrawSpeed = 100;
    public int enemyDrawSpeed = 40;


    
    public float scaleFactor = 5.0f;// Scaling factor for the red area || it is working good with 5.0f
    [SerializeField] private float moveSpeed = 3f; // Line's move speed
    
    
    
    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        inRedArea = false;
        gameWon = false;
    }

    void Update()
    {
        // Çizginin hareketini sağlayan kod
        MoveLine();


        // Space tuşuna basıldığında ve çizgi kırmızı alanda ise kazanma kontrolü yapılır
        if (Input.GetKeyDown(KeyCode.Space) && inRedArea && !gameWon)
        {
            WinGame();
        }

        // FOR TESTING PRESS X TO CHANGE THE SIZE OF THE RED AREA
        // when X is pressed, change the size of the red area based on the enemy and player's draw speeds
        if (Input.GetKeyDown(KeyCode.X))
        {
            ChangeRedAreaSize(enemyDrawSpeed, playerDrawSpeed);
            ChangeLineSpeed();
        }
    }

    // Change the size of the red area based on the enemy and player's draw speeds
    public void ChangeRedAreaSize(int enemyDrawSpeed, int playerDrawSpeed)
    {
        // If enemyDrawSpeed is 0, to avoid division by zero, we set the scale to maximum
        if (enemyDrawSpeed == 0)
        {
            redArea.transform.localScale = new Vector3(bar.localScale.x, 1, 1);
        }
        else
        {
            // Calculate the ratio of playerDrawSpeed to the sum of playerDrawSpeed and enemyDrawSpeed
            float ratio = (float)playerDrawSpeed / (playerDrawSpeed + enemyDrawSpeed);

            // Apply the scaling factor to the ratio
            ratio = Mathf.Pow(ratio, scaleFactor);

            // Calculate the new scale for the red area
            float newScale = bar.localScale.x * ratio;

            // Ensure the new scale does not exceed the bar's scale
            newScale = Mathf.Min(newScale, bar.localScale.x);

            // Set the scale of the red area based on the ratio
            redArea.transform.localScale = new Vector3(newScale, 1, 1);
        }
    }
    
    
    // Change the speed of the line based on the enemy and player's draw speeds
    public void ChangeLineSpeed()
    {
        // Maximum and minimum possible speeds
        float maxSpeed = 10f;
        float minSpeed = 1f;

        // Calculate the ratio of playerDrawSpeed to the sum of playerDrawSpeed and enemyDrawSpeed
        float ratio = (float)enemyDrawSpeed / (playerDrawSpeed + enemyDrawSpeed);

        // Calculate the new speed for the line
        float newSpeed = ratio * (maxSpeed - minSpeed) + minSpeed;

        // Set the speed of the line based on the ratio
        moveSpeed = newSpeed;
    }

    
    // Move the line based on a sine wave
    void MoveLine()
    {
        // Calculate the move direction using a sine wave and the move speed
        float moveDirection = Mathf.Sin(Time.time * moveSpeed);

        // Calculate the left and right bounds based on the bar's position and scale
        float barWidth = bar.localScale.x;
        float leftBound = bar.position.x - barWidth / 2;
        float rightBound = bar.position.x + barWidth / 2;

        // Calculate the new X position within the bounds
        float newX = Mathf.Lerp(leftBound, rightBound, (moveDirection + 1f) / 2f);

        // Set the new position of the line
        line.position = new Vector3(newX, line.position.y, line.position.z);
    }

    // When the line enters the red area, the following code is triggered
    void OnTriggerEnter2D(Collider2D other)
    {
        // Çizgi kırmızı alana girdiğinde tetiklenen kod
        if (other.gameObject == redArea)
        {
            inRedArea = true;
            Debug.Log("Kırmızı alana girdiniz!");
        }
    }


    // When the line exits the red area, the following code is triggered
    void OnTriggerExit2D(Collider2D other)
    {
        // Çizgi kırmızı alandan çıktığında tetiklenen kod
        if (other.gameObject == redArea)
        {
            inRedArea = false;
        }
    }

    void WinGame()
    {
        // Oyun kazanıldığında tetiklenen kod
        Debug.Log("Kazandınız!");
        gameWon = true;


        // TODO // it should be implemented in GameManager
    }
}