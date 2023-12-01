using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicLevel1 : MonoBehaviour
{
    private AudioSource audioSource;
    private bool musicaActivada = false;

    private void Start()
    {
        // Asegúrate de tener un AudioSource en el mismo objeto que este script.
        audioSource = GetComponent<AudioSource>();
        audioSource.Stop(); // Asegúrate de que la música comience detenida.
    }

    private void OnTriggerEnter(Collider other)
    {
        // Comprueba si el jugador (u otro objeto) entró en el BoxCollider.
        if (other.CompareTag("Player") && !musicaActivada)
        {
            // Activa la música si es la primera vez que entra el jugador.
            audioSource.Play();
            musicaActivada = true;
        }
    }
}
