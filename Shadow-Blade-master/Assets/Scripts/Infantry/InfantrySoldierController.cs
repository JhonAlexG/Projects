using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO:
// Fix Push Force
//? Fix idle animation
// If enemy get damage, enemy will be pushed back and move again

public class InfantrySoldierController : MonoBehaviour
{
    public float hp = 100f;

    public AudioClip electricityStunSound;

    private AudioSource audioSource;

    private bool normalDamage = false;
    private bool electricityDamage = false;
    private bool isGettingDamage = false;
    private float stunTime = 0f;
    private GameObject player;
    private Animator animator;
    private List<GameObject> others = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hp > 0)
        {
            if (stunTime <= 0)
            {
                if (normalDamage)
                {
                    animator.SetBool("Move", true);
                    animator.SetBool("Damage", false);
                }
                else if (electricityDamage)
                {
                    animator.SetBool("Move", true);
                    animator.SetBool("Electricity", false);
                }

                if (animator.GetBool("Move") && !animator.GetBool("Attack") && !animator.GetBool("Damage") && !animator.GetBool("Electricity") && player != null && player.GetComponent<PlayerController>().hp > 0)
                {
                    Move();
                }
                else {
                    animator.SetBool("Move", false);
                }
            }
            else {
                stunTime -= Time.deltaTime;

                if (normalDamage)
                {
                    animator.SetBool("Move", false);
                    animator.SetBool("Attack", false);
                    animator.SetBool("Damage", true);
                }
                else if (electricityDamage)
                {
                    animator.SetBool("Move", false);
                    animator.SetBool("Attack", false);
                    animator.SetBool("Electricity", true);
                }
            }

            if (isGettingDamage && !animator.GetCurrentAnimatorStateInfo(0).IsName("Damage") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Electricity"))
            {
                animator.SetBool("Damage", false);
                animator.SetBool("Electricity", false);
                animator.SetBool("Move", true);
                isGettingDamage = false;
            }
        }
        else {
            Dead();
        }
    }

    private void Move()
    {
        if (player.transform.position.x > transform.position.x)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else {
            gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        gameObject.transform.Translate(2 * Time.deltaTime, 0, 0);
    }

    private void Dead()
    {
        animator.SetBool("Dead", true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            animator.SetBool("Move", true);
            player = other.gameObject;
        }

        if (other.gameObject.tag == "Water")
        {
            others.Add(other.gameObject);
        }

        if (Mathf.Abs(other.gameObject.transform.position.x - transform.position.x) < 2.5f)
        {
            if ((other.gameObject.tag == "Sword" || other.gameObject.tag == "Normal" || other.gameObject.tag == "Electricity") && animator.GetBool("Electricity"))
            {
                Debug.Log("Se ha destruido la electricidad");

                electricityDamage = false;
                normalDamage = false;
                stunTime = 0f;

                animator.SetBool("Damage", false);
                animator.SetBool("Electricity", false);
                animator.SetBool("Move", true);
            }
            else {
                if (other.gameObject.tag == "Sword")
                {
                    hp -= 12.5f;
                    stunTime = 0.5f;
                    isGettingDamage = true;
                    normalDamage = true;
                }

                if (other.gameObject.tag == "Normal")
                {
                    hp -= 12.5f;
                    stunTime = 0.5f;
                    isGettingDamage = true;
                    normalDamage = true;
                }
                
                if (other.gameObject.tag == "Electricity")
                {
                    hp -= others.Count > 0 ? 20f : 10f;
                    stunTime = others.Count > 0 ? 2f : 0.5f;
                    isGettingDamage = true;
                    
                    if (others.Count > 2)
                    {
                        normalDamage = false;
                        electricityDamage = true;
                    }
                    else {
                        normalDamage = true;
                        electricityDamage = false;
                    }

                    audioSource.PlayOneShot(electricityStunSound);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            animator.SetBool("Move", false);
        }

        if (other.gameObject.tag == "Water")
        {
            others.Remove(other.gameObject);
            Debug.Log("Water Exit");
        }
    }
}
