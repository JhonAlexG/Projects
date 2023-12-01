using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfantryRangeAttack : MonoBehaviour
{
    public Animator infantrySoldierAnimator;
    public AudioSource infantrySoldierAudioSource;
    public AudioClip axeSound;
    
    private GameObject player;

    private float waitBeforeAttack = 0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (infantrySoldierAnimator.GetBool("Attack"))
        {
            infantrySoldierAnimator.SetBool("Move", false);
        }

        if (player != null && waitBeforeAttack <= 0 && Mathf.Abs(player.transform.position.x - transform.position.x) < 3f && player.GetComponent<PlayerController>().hp > 0 && gameObject.transform.parent.gameObject.GetComponent<InfantrySoldierController>().hp > 0)
        {
            infantrySoldierAudioSource.PlayOneShot(axeSound);
            infantrySoldierAnimator.SetBool("Attack", true);
            waitBeforeAttack = 1f;
        }
        else
        {
            if (player != null && Mathf.Abs(player.transform.position.x - transform.position.x) < 10f && !infantrySoldierAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                infantrySoldierAnimator.SetBool("Attack", false);
                infantrySoldierAnimator.SetBool("Move", true);
            }
        }

        if (waitBeforeAttack > 0)
        {
            waitBeforeAttack -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            player = other.gameObject;
            infantrySoldierAnimator.SetBool("Attack", true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            infantrySoldierAnimator.SetBool("Attack", false);
        }
    }
}
