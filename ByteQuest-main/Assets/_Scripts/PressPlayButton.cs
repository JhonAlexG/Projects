using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PressPlayButton : MonoBehaviour
{
    public float raycastDistance = 5f; // Distancia máxima del rayo.
    public string buttonTag = "PlayButton"; // Etiqueta del objeto botón.
    [SerializeField] private Animator buttonAnimator;
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private Image buttonImage;

    void Update()
    {
        // Mira en la dirección de la cámara.
        Vector3 raycastDirection = transform.forward;

        // Dispara un rayo desde la posición de la cámara en la dirección de la mira.
        RaycastHit hit;
        if (Physics.Raycast(transform.position, raycastDirection, out hit, raycastDistance))
        {
            // Comprueba si el objeto alcanzado tiene la etiqueta "button".
            if (hit.collider.CompareTag(buttonTag))
            {
                // Activa la animación (por ejemplo, un bool llamado "IsPress" en el Animator).
                if (buttonAnimator != null)
                {
                    buttonImage.color = Color.green;
                    if (Input.GetMouseButtonDown(0))
                    {
                        // Activa la animación del botón.
                        if (buttonAnimator != null)
                        {
                            buttonAnimator.SetBool("IsPress", true);
                        }

                        // Activa la animación de apertura de la puerta.
                        if (doorAnimator != null)
                        {
                            doorAnimator.SetTrigger("isOpen");
                        }
                    }

                    else if (Input.GetMouseButtonUp(0))
                    {
                        buttonAnimator.SetBool("IsPress", false);
                    }
                }
            }
            else
            {
                buttonImage.color = Color.white;
            }
        }
    }
}
