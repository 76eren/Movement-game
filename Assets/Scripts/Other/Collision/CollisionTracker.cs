using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTracker : MonoBehaviour
{
    private GameObject currentCol;

    void OnCollisionEnter(Collision collision)
    {
        currentCol = collision.gameObject;
    }

    public GameObject ReturnCollision()
    {
        return currentCol;
    }



}
