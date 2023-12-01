using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyReto3 : MonoBehaviour
{
    public PhysicsPickup physicsPickup;
    public bool isHolding = false;
    public string nuevoLayerName;
    private int nuevoLayerID;
    public Animator animatorDoor;
    public AudioSource audioSource;
    public AudioClip clipDeSonido; // Agrega una variable para el clip de sonido.

    private bool sonidoReproducido = false; // Variable para rastrear si el sonido se ha reproducido.

    private void Start()
    {
        nuevoLayerID = LayerMask.NameToLayer(nuevoLayerName);
    }

    private void Update()
    {
        isHolding = physicsPickup.IsholdingObj;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Sensor") && !isHolding && !sonidoReproducido)
        {
            gameObject.layer = LayerMask.NameToLayer(nuevoLayerName);
            animatorDoor.SetTrigger("isOpen");

            // Reproduce el sonido utilizando el clip de sonido definido.
            if (audioSource != null && clipDeSonido != null)
            {
                audioSource.PlayOneShot(clipDeSonido);
                sonidoReproducido = true; // Marca el sonido como reproducido.
            }
        }
    }
}