using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntonioController : MonoBehaviour
{
    public int hp = 1000;
    public GameObject electricityPrefab;
    
    private float pushAttackForce = 3f;
    private float stunTime = 0f;
    private float waitForAttack = 0f;
    private Animator animator;

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hp > 0)
        {
            if (stunTime <= 0)
            {
                if (animator.GetBool("Move"))
                {
                    Move();
                }

                if (animator.GetBool("Attack") && waitForAttack <= 0)
                {
                    Attack();
                }
            }
            else {
                stunTime -= Time.deltaTime;
            }

            if (waitForAttack > 0)
            {
                waitForAttack -= Time.deltaTime;
            }
        }
        else {
            Dead();
        }
    }

    private void Move()
    {
        if (player != null && player.GetComponent<PlayerController>().hp > 0)
        {
            if (player.transform.position.x > gameObject.transform.position.x)
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else {
                gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
            }

            gameObject.transform.Translate(2 * Time.deltaTime, 0, 0);

            if (player.transform.position.y > gameObject.transform.position.y)
            {
                gameObject.transform.Translate(0, 2 * Time.deltaTime, 0);
            }
            else {
                gameObject.transform.Translate(0, -2 * Time.deltaTime, 0);
            }
        }
        else {
            animator.SetBool("Move", false);
        }
    }

    private void Attack()
    {
        float angle = Mathf.Atan2(player.transform.position.y - gameObject.transform.position.y, player.transform.position.x - gameObject.transform.position.x) * Mathf.Rad2Deg;

        // if (player.transform.position.x > gameObject.transform.position.x)
        // {
        //     gameObject.transform.rotation = Quaternion.Euler(0, 0, angle);
        // }
        // else {
        //     gameObject.transform.rotation = Quaternion.Euler(180, 0, -angle);
        // }
        
        if (player.transform.position.x > gameObject.transform.position.x)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else {
            gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        // Instanciar como hijo de este objeto
        GameObject electricity = Instantiate(electricityPrefab, gameObject.transform.position, Quaternion.Euler(0.5f, 0, angle));
        waitForAttack = 3f;
    }

    private void Dead()
    {
        animator.SetBool("Dead", true);
        Destroy(gameObject, 0.85f);
    }

    private void PushAttackSide(GameObject other)
    {
        if (hp > 0)
        {
            if (other.transform.position.x > gameObject.transform.position.x)
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                gameObject.transform.position -= new Vector3(pushAttackForce, 0, 0);
            }
            else {
                gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
                gameObject.transform.position += new Vector3(pushAttackForce, 0, 0);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (Mathf.Sqrt(Mathf.Pow(other.transform.position.x - gameObject.transform.position.x, 2) + Mathf.Pow(other.transform.position.y - gameObject.transform.position.y, 2)) < 2f)
        {
            if (other.gameObject.CompareTag("Sword") || other.gameObject.CompareTag("Normal"))
            {
                hp -= 20;
                stunTime = 0.5f;
                PushAttackSide(other.gameObject);
            }

            if (other.gameObject.name != "Antonio Electricity" && other.gameObject.CompareTag("Electricity"))
            {
                Debug.Log("Electricity");

                hp -= 20;
                stunTime = 2f;
            }
        }
    }
}
