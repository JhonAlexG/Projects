using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    public bool isHolding = false;
    public RuntimeAnimatorController nuevoController;
    public Animator Dooranim;
    public Animator anim; 
    public PhysicsPickup physicsPickup;
    public string nuevoLayerName;
    
    // Declarar nuevoLayerID a nivel de clase para que sea accesible en todo el script.
    private int nuevoLayerID;

    private void Start()
    {
        anim = GetComponent<Animator>();
        // Asignar el ID de la capa en el método Start.
        nuevoLayerID = LayerMask.NameToLayer(nuevoLayerName);
    }

    private void Update()
    {
        isHolding = physicsPickup.IsholdingObj;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Sensor") && !isHolding )
        {
            anim.runtimeAnimatorController = nuevoController;
            anim.SetTrigger("InSensor");
            Dooranim.SetTrigger("isOpen");
            gameObject.layer = nuevoLayerID;
        }
    }
}
