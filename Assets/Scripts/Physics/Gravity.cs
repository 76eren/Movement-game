/*
/ This script is going to implement gravity
/ Basically a rigidbody3d doesn't have a gravity scale property 
/ so you're better off implementing the gravity yourself.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    public float scale = 7f;
    
    
    public Rigidbody rb;

    private float defaultScale;

    void Start()
    {
        rb.useGravity = false;
        defaultScale = scale;
    }

    void FixedUpdate()
    {
        rb.AddForce(Physics.gravity * scale * Time.fixedDeltaTime, ForceMode.Acceleration);
    }

    
    // Public functions
    public void ResetScale()
    {
        scale = defaultScale;
    }

    public void EditScale(float value)
    {
        scale = value;
    }

}
