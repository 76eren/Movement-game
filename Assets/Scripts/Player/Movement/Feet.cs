using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feet : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>().ResetJumps();
        }
    }
}
