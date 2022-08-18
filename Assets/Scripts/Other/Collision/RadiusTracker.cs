using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiusTracker : MonoBehaviour
{
    private Collider[] colliders;
    public float radius = 0.25f;
    public Transform target;

    void Update()
    {
        colliders =  Physics.OverlapSphere(target.position, radius);
    }

    public Collider[] ReturnColliders()
    {
        return colliders;
    }

}
