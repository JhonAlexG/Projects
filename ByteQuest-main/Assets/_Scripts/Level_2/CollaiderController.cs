using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollaiderController : MonoBehaviour
{
    public int points = 0;
    public int totalPoints = 0;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Sphere"))
        {
            totalPoints += points;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Sphere"))
        {
            totalPoints -= points;
        }
    }
}
