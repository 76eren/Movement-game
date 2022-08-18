using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{

    // All of our referendes
    private LineRenderer lr;
    public Transform tip;

    // Variables
    public float range = 25f;
    private Camera cam; // The raycast will be from the camera rather than from the gun. Gun is just a cosmetic (like most video games)
    private bool grappling = false;
    
    public GameObject player;
    private GameObject previousObject = null;

    private Quaternion defaultRotation;


    // Due to an issue with unity we are unable to grapple onto objects we're "colliding" with
    // Instead of colliding we just go through the object
    // For this reason I have decided to disable grappling onto objects we're colliding with
    private CollisionTracker collisionTracker;
    private RadiusTracker radiusTracker;

    private GameObject coll;

    // This is broken -------
    private Collider[] feetSphere;
    public GameObject feet;
    private GameObject something;
    // ------------------------


    bool shouldStop = false;


    void Start()
    {
        lr = GetComponent<LineRenderer>();
        cam = Camera.main;
        defaultRotation = transform.localRotation;
        collisionTracker = player.GetComponent<CollisionTracker>();
        radiusTracker = GetComponent<RadiusTracker>();
    }


    void Update()
    {
        coll = collisionTracker.ReturnCollision();

        feetSphere = radiusTracker.ReturnColliders();
        if (feetSphere != null)
        {
            feetSphere = feetSphere.Where(val => val != player).ToArray();
            feetSphere = feetSphere.Where(val => val != feet).ToArray();
        }
        else 
        {
            feetSphere = new Collider[0];
        }


        if (feetSphere.Length > 0 )
        {
            something  = feetSphere[0].gameObject;
            print(something.name);
        } 

        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range))
        {
            if (hit.transform.gameObject == coll)
            {
                shouldStop=true;
            }
            if (something != null)
            {
                if (something == hit.transform.gameObject)
                {
                    shouldStop=true;
                }
            }
        }

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range) && Input.GetMouseButton(1) && coll != hit.transform.gameObject && coll != player && !shouldStop)
        {
            grappling = true;
        }
        else 
        {
            if (!Input.GetMouseButton(1) || shouldStop)
            {
                if (previousObject != null)
                {
                    Destroy(previousObject.GetComponent<Rigidbody>());
                }
                
                previousObject = null;
                grappling=false;
                lr.enabled=false;
                
                if (player.GetComponent<SpringJoint>() != null)
                {
                    Destroy(player.GetComponent<SpringJoint>());
                    transform.localRotation = defaultRotation;
                }
                shouldStop=false;
            }

            
        }

        if (grappling)
        {   
            
            // transform.LookAt(hit.point); This doesn't work lol


            if (player.GetComponent<SpringJoint>() == null)
            {
                if (hit.transform.gameObject.GetComponent<Rigidbody>() == null)
                {
                    hit.transform.gameObject.AddComponent<Rigidbody>();
                    hit.transform.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    hit.transform.gameObject.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;

                }

                player.AddComponent<SpringJoint>();
                SpringJoint sj = player.GetComponent<SpringJoint>();
                sj.connectedAnchor = hit.point;
                sj.connectedBody = hit.transform.gameObject.GetComponent<Rigidbody>();
                
                sj.spring = 30;
                sj.damper = 15f;
                sj.tolerance = 0.025f;
                sj.autoConfigureConnectedAnchor = false;
                sj.maxDistance = 0;
                sj.minDistance= 10;
                
            }
            

            lr.enabled=true;
            lr.SetPosition(0, tip.transform.position);
            if (hit.point != Vector3.zero && hit.transform.gameObject != previousObject && previousObject == null)
            {
                lr.SetPosition(1, hit.point);
                previousObject = hit.transform.gameObject;
            }
        }
            
    }

}
