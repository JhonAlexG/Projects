using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntonioElectricity : MonoBehaviour
{
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5f);
        audioSource = gameObject.GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Translate(4 * Time.deltaTime, 0, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Sword")
        {
            Destroy(gameObject);
        }
    }
}
