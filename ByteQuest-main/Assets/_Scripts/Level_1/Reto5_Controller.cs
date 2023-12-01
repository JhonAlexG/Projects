using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reto5_Controller : MonoBehaviour
{
    public Animator animator;
    
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Sphere")){
            animator.SetBool("isOn", true);
        }
    }
}
