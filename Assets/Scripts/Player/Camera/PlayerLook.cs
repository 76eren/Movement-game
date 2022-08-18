// https://www.youtube.com/watch?v=_QajrabyTJc
// Brackeys <33

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public float sensitivity = 100f;
    private Transform player;
    private float xRotation = 0f; 
    
    private float zRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime; 
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, transform.localRotation.y, zRotation);
        player.Rotate(Vector3.up * mouseX);

    }


    // Public functions
    
    public void editZrotation(float value)
    {   
        zRotation = value;
    }
}
