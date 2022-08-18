using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallrunning : MonoBehaviour
{
    private Gravity gravity;
    private Movement movement;

    private Rigidbody rb;
    
    private PlayerLook playerLook;
    
    // All walls need to have the layer "Wall"
    public LayerMask wall;

    // The left transform on the Player
    public Transform left;

    // The right transform on the Player
    public Transform right;

    private float SPHERE_RADIUS = 0.5f;

    private bool isWallriding = false;
    private bool stopWallriding = false;


    public float accelerationSpeed = 5f;
    float defaultAccelerationSpeed;


    // Camera tilting
    public float tiltSpeed = 4.5f;
    float cameraTilt;
    float maxtilt = 15f;

    bool ran  = false;


    // Player friction
    public PhysicMaterial playerMaterial;
    private PhysicMaterial copy; 



    // lol
    bool fuck  = false;

    bool wallOnLeft = false;
    bool wallOnRight = false;


    void Start()
    {
        // A reference to the gravity so we can edit the gravity scale
        gravity = GetComponent<Gravity>();

        // A reference to the movement to bump up the speed
        movement = GetComponent<Movement>();
    
        defaultAccelerationSpeed = accelerationSpeed;

        playerLook = Camera.main.GetComponent<PlayerLook>();

        cameraTilt = 0;

        rb = GetComponent<Rigidbody>();

        
        copy = playerMaterial;

    }

    void Update()
    {
        Collider[] collidersLeft = Physics.OverlapSphere(left.transform.position, SPHERE_RADIUS, wall);
        Collider[] collidersRight = Physics.OverlapSphere(right.transform.position, SPHERE_RADIUS, wall);


        
        // WALLRIDE DETECTION AND WHAT TO DO

        if (collidersLeft.Length > 0 && collidersRight.Length == 0 && !stopWallriding && !ran)
        {
            fuck = true;
            wallOnLeft = true;
        }
        if (collidersRight.Length > 0 && collidersLeft.Length == 0 && !stopWallriding && !ran)
        {
            fuck=true;
            wallOnRight = true;
        }
        
        if (fuck)
        {
            fuck = false;
            isWallriding = true;
            ran=true;


            Vector3 velocity = rb.velocity;
            velocity.y = 0;
            rb.velocity = velocity;

            playerMaterial.staticFriction = 0;
            playerMaterial.dynamicFriction=0f;
        }



        if (isWallriding)
        {
            movement.ResetJumps();
            gravity.EditScale(Camera.main.transform.rotation.x * 15  * -1);


            movement.IncreaseSpeed(accelerationSpeed * Time.deltaTime);
            movement.IncreaseSpeedCap(accelerationSpeed * Time.deltaTime);

            accelerationSpeed -= 1 * Time.deltaTime;
            accelerationSpeed = Mathf.Clamp(accelerationSpeed, 1, defaultAccelerationSpeed);

            // We tilt the camera a little bit
            if (wallOnLeft)
            {
                cameraTilt -= tiltSpeed * Time.deltaTime;
                cameraTilt = Mathf.Clamp(cameraTilt, -maxtilt, 0);
                playerLook.editZrotation(cameraTilt);

            }

            if (wallOnRight)
            {
                cameraTilt += tiltSpeed * Time.deltaTime;
                cameraTilt = Mathf.Clamp(cameraTilt, 0, maxtilt);
                playerLook.editZrotation(cameraTilt);
            }
        
        }


        // What happens when we stop wallriding
        if (collidersLeft.Length == 0 && collidersRight.Length == 0 && isWallriding 
            || stopWallriding && isWallriding 
                || isWallriding && collidersLeft.Length == 0 && collidersRight.Length == 0 
                    || !isWallriding && cameraTilt != 0)
        {
            gravity.ResetScale();
            isWallriding = false;
            wallOnLeft = false;
            wallOnRight = false;
            ran=false;

            if (cameraTilt < 0)
            {
                cameraTilt += tiltSpeed * Time.deltaTime;
                cameraTilt = Mathf.Clamp(cameraTilt, -maxtilt, 0);
                playerLook.editZrotation(cameraTilt);
            }

            if (cameraTilt > 0)
            {
                cameraTilt -= tiltSpeed * Time.deltaTime;
                cameraTilt = Mathf.Clamp(cameraTilt, 0, maxtilt);
                playerLook.editZrotation(cameraTilt);
            }


            accelerationSpeed = defaultAccelerationSpeed;
            playerMaterial = copy;

        }





    }
}
