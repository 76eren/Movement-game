using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody rb;

    // Keeps track of inpit
    private float x;
    private float z;


    // Basic movement
    public float speedCap = 10f;
    [HideInInspector] public float defaultSpeedCap;
    public float speed = 200f;
    private float defaultSpeed;
    
    // Fast running
    float defaultFov;
    public float accelerationSpeed = 5f;
    float defaultaccelerationSpeed;


    // Jumping
    public float jumpForce = 1000f;
    public int jumps = 2;
    private int defualtJumps;
    public GameObject feet;
    public float feetRadius;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
        defaultSpeedCap = speedCap;
        
        defualtJumps = jumps;
        
        defaultFov = Camera.main.fieldOfView;

        defaultSpeed = speed;

        defaultaccelerationSpeed = accelerationSpeed;
    }

    void Update()
    {

        // Gets mouse input
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        // Fast running
        if (Input.GetKey(KeyCode.LeftShift))
        {
            // Increases the fov which gives the illusion of speed
            float fov = Camera.main.fieldOfView;
            fov += 100f * Time.deltaTime;
            fov = Mathf.Clamp(fov, defaultFov, 75);
            Camera.main.fieldOfView = fov;

            // Increases the speed
            speed += (accelerationSpeed * Time.deltaTime);
            speedCap += (accelerationSpeed * Time.deltaTime);
            
            // We can't just endlessly go faster so we reduce the accelerationSpeed over time
            accelerationSpeed -= 1 * Time.deltaTime;
            
            // We clamp the accelerationspeed so it doesn't go to a negative number AND because I don't want it to go to 0 either
            accelerationSpeed = Mathf.Clamp(accelerationSpeed, 0.2f, defaultaccelerationSpeed);

        }
        else 
        {
            // Decreases the camera FOV after stopping running
            float fov = Camera.main.fieldOfView;
            fov -= 100f * Time.deltaTime;
            fov = Mathf.Clamp(fov, defaultFov, 75);
            Camera.main.fieldOfView = fov;
        

            // Resets all of our values
            speedCap = defaultSpeedCap;
            speed = defaultSpeed;
            accelerationSpeed = defaultaccelerationSpeed;
        }

    }

    void FixedUpdate() 
    {
        Vector3 force = new Vector3(x * speed, 0, z * speed);
        rb.AddRelativeForce(force);

        // Adds a speed cap
        if (rb.velocity.magnitude > speedCap)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, speedCap);
        }

    }

    void Jump()
    {
        if (jumps > 0)
        {
            rb.AddForce(Vector3.up * jumpForce);
            jumps--;
        }
    }

    
    // Some public functions, these are supposed to be used by different classes
    
    // Gets called from the Feet class
    public void ResetJumps()
    {
        jumps = defualtJumps;
    }
    
    public void AddJumps(int amount)
    {
        jumps += amount;
    }


    public void IncreaseSpeedCap(float amount)
    {
        speedCap += amount;
    }

    public void IncreaseSpeed(float amount) 
    {
        speed += amount;
    }

    public void ResetAll()
    {
        speedCap = defaultSpeedCap;
        speed = defaultSpeed;
    }



}
