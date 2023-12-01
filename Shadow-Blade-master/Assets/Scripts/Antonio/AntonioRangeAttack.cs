using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntonioRangeAttack : MonoBehaviour
{
    private GameObject player;
    private GameObject Antonio;
    private Animator AntonioAnimator;

    // Start is called before the first frame update
    void Start()
    {
        Antonio = gameObject.transform.parent.gameObject;
        AntonioAnimator = Antonio.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player in range");

            player = other.gameObject;
            Antonio.GetComponent<AntonioController>().player = player;
            AntonioAnimator.SetBool("Move", false);
            AntonioAnimator.SetBool("Attack", true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (player != null && player.GetComponent<PlayerController>().hp > 0)
            {
                AntonioAnimator.SetBool("Attack", false);
                AntonioAnimator.SetTrigger("Initial Move");
                AntonioAnimator.SetBool("Move", true);
            }
            else {
                AntonioAnimator.SetBool("Attack", false);
            }
        }
    }
}
