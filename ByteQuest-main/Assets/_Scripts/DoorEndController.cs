using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorEndController : MonoBehaviour
{
    public bool isInsideDoor = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInsideDoor = true;
        }
    }
}
