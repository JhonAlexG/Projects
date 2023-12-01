using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonReto2 : MonoBehaviour
{
    public Animator animator;
    public GameObject Block;
    public Vector3 spawnPoint;
    public int cont = 0;
    private bool isRaycastHitting = false;

    private void Start()
    {
        animator = GetComponent<Animator>();

    }

    private void Update()
    {
        if (isRaycastHitting && Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("isStay2");
            animator.SetBool("IsPress", true);
            if (cont < 8)
            {
                GenerarBloque(spawnPoint);
            }
            else
            {
                Debug.Log("MAximo alcanzado");
            }
        }else if (Input.GetMouseButtonUp(0))
        {
            
            animator.SetBool("IsPress", false);
        }
    }

    private void GenerarBloque(Vector3 spawnPoint)
    {

        Instantiate(Block, spawnPoint, Quaternion.identity);
        cont++;
    }

    private void OnMouseEnter()
    {
        isRaycastHitting = true;
    }

    // Cuando un rayo sale de colisión con el objeto
    private void OnMouseExit()
    {
        isRaycastHitting = false;
    }
}
