using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// TODO:
// Fix Push Force
// Add Cooldown for Jump
// Do Combo

public class PlayerController : MonoBehaviour
{
    public int hp = 100;

    public float speed = 5f;
    public float jumpForce = 25f;
    public int modeAttack;
    public GameObject modeAttackImage;
    public List<Sprite> modeAttackSprites;

    public GameObject ControlsCanvas;
    public GameObject AttackCanvas;
    public GameObject DashCanvas;
    public GameObject WinCanvas;
    public GameObject LoseCanvas;

    public GameObject gunEffect;
    public Animator gunEffectAnimator;
    public GameObject hpFiller;
    public Text hpValue;

    public AudioClip walkSound;
    public AudioClip shotgunSound;
    public AudioClip electricShotSound;
    public AudioClip swordSound;
    public AudioClip electricityStunSound;

    private AudioSource audioSource;

    private bool makeControlsCanvasVisible = false;
    private bool makeAttackCanvasVisible = false;
    private bool makeDashCanvasVisible = false;

    private float waitBeforeAttack = 0f;
    private float jumpTime = 0.75f;
    private bool canJump = true;
    private float timeDash = 0f;
    private float waitForDash = 0f;
    private float distanceDash = 15f;
    private float pushAttackForce = 2.5f;
    private float stunTime = 0f;
    private string[] modeAttacks = { "Normal", "Electricity" };

    private Animator animator;
    
    private List<GameObject> others = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        modeAttack = 0;
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
                Jump();
                Move();
                ActivateDash();
                Attack();
                ChangeModeAttack();
            }
            else {
                stunTime -= Time.deltaTime;
            }

            if (waitBeforeAttack > 0)
            {
                waitBeforeAttack -= Time.deltaTime;
            }

            if (animator.GetBool("Landing"))
            {
                animator.SetBool("Fall", false);
                canJump = true;
                jumpTime = 0.75f;
            }

            if (timeDash > 0)
            {
                timeDash -= Time.deltaTime;
            }

            if (waitForDash > 0)
            {
                waitForDash -= Time.deltaTime;
            }

            Dash();

            // RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position, Vector2.down * 5f, 5f);

            // Debug.DrawRay(gameObject.transform.position, Vector2.down * 5f, Color.red);
            
            // if (hit.collider.gameObject.tag == "Ground")
            // {
            //     Debug.Log("Ground");

            //     canJump = true;
            //     jumpTime = 0.75f;
            //     animator.SetBool("Jump", false);
            //     animator.SetBool("Fall", false);

            //     if (!animator.GetBool("Landing"))
            //     {
            //         animator.SetBool("Landing", true);
            //     }
            // }
        }
        else {
            if (animator.GetBool("Landing"))
            {
                Dead();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
            }
            else {
                Time.timeScale = 1;
            }
        }

        if (makeControlsCanvasVisible && !animator.GetCurrentAnimatorStateInfo(0).IsName("Sword"))
        {
            ControlsCanvas.SetActive(true);
            makeControlsCanvasVisible = false;
        }
        
        if (makeAttackCanvasVisible && !animator.GetCurrentAnimatorStateInfo(0).IsName("Sword"))
        {
            AttackCanvas.SetActive(true);
            makeAttackCanvasVisible = false;
        }

        if (makeDashCanvasVisible && !animator.GetCurrentAnimatorStateInfo(0).IsName("Sword"))
        {
            DashCanvas.SetActive(true);
            makeDashCanvasVisible = false;
        }

        hpFiller.GetComponent<Image>().fillAmount = hp / 100f;
        hpValue.text = hp > 0 ? hp.ToString() : "0";
    }

    public void Combo()
    {
        
    }

    private void Jump()
    {
        if (jumpTime > 0 && canJump)
        {
            if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Space)) && Input.GetKey(KeyCode.LeftArrow))
            {
                jumpTime -= Time.deltaTime;
                animator.SetBool("Jump", true);
                gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
                gameObject.transform.position += new Vector3(-speed * 2f, jumpForce, 0) * Time.deltaTime;
            }
            else if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Space)) && Input.GetKey(KeyCode.RightArrow))
            {
                jumpTime -= Time.deltaTime;
                animator.SetBool("Jump", true);
                gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                gameObject.transform.position += new Vector3(speed * 2f, jumpForce, 0) * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Space))
            {
                jumpTime -= Time.deltaTime;
                animator.SetBool("Jump", true);
                gameObject.transform.Translate(0, jumpForce * Time.deltaTime, 0);
            }
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            jumpTime = 0;
            animator.SetBool("Jump", false);
            animator.SetBool("Fall", true);
        }
    }

    private void Move()
    {
        if (Time.timeScale == 1)
        {
            if (Input.GetKey(KeyCode.LeftArrow) && waitBeforeAttack <= 0)
            {
                animator.SetBool("Move", true);
                gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
                gameObject.transform.Translate(speed * Time.deltaTime, 0, 0);
            }
            else if (Input.GetKey(KeyCode.RightArrow) && waitBeforeAttack <= 0)
            {
                animator.SetBool("Move", true);
                gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                gameObject.transform.Translate(speed * Time.deltaTime, 0, 0);
            }
            else {
                animator.SetBool("Move", false);
            }
        }
    }

    private void ActivateDash()
    {
        if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.C) && Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.C) && Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (waitForDash <= 0)
            {
                timeDash = 0.4f;
                waitForDash = 1f;
                stunTime = 0.4f;
            }
        }
    }

    private void Dash()
    {
        if (timeDash > 0)
        {
            animator.SetBool("Move", false);
            animator.SetBool("Dash", true);

            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            gameObject.transform.Translate(distanceDash * Time.deltaTime, 0, 0);
        }
        else {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            animator.SetBool("Dash", false);

            if (!animator.GetBool("Landing"))
            {
                animator.SetBool("Fall", true);
            }
        }
    }

    private void ChangeModeAttack()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            modeAttack = (modeAttack + 1) % modeAttacks.Length;
            gunEffect.tag = modeAttacks[modeAttack];
            modeAttackImage.GetComponent<Image>().sprite = modeAttackSprites[modeAttack];
        }
    }

    private void Attack()
    {
        if (waitBeforeAttack <= 0)
        {
            if (Input.GetKeyDown(KeyCode.X) && animator.GetBool("Landing"))
            {
                waitBeforeAttack = modeAttacks[modeAttack] == "Normal" ? 1f : 1.5f;
                animator.SetBool("Gun", true);
                animator.SetBool("Fall", false);
                gunEffectAnimator.SetBool(modeAttacks[modeAttack], true);

                if (modeAttacks[modeAttack] == "Normal")
                {
                    audioSource.PlayOneShot(shotgunSound);
                }
                else {
                    audioSource.PlayOneShot(electricShotSound);
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                if (ControlsCanvas.activeSelf)
                {
                    makeControlsCanvasVisible = true;
                }

                if (AttackCanvas.activeSelf)
                {
                    makeAttackCanvasVisible = true;
                }

                if (DashCanvas.activeSelf)
                {
                    makeDashCanvasVisible = true;
                }

                audioSource.PlayOneShot(swordSound);

                animator.SetBool("Sword", true);
                animator.SetBool("Fall", false);

                waitBeforeAttack = 0.5f;
            }
        }
        else {
            animator.SetBool("Gun", false);
            animator.SetBool("Sword", false);
            gunEffectAnimator.SetBool(modeAttacks[modeAttack], false);
        }
    }

    private void Dead()
    {
        animator.SetBool("Dead", true);
        LoseCanvas.SetActive(true);
    }

    private void PushAttackSide(GameObject other)
    {
        if (hp > 0)
        {
            if (other.transform.position.x > gameObject.transform.position.x)
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else {
                gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
            }

            gameObject.transform.Translate(-pushAttackForce, 0.25f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Water")
        {
            others.Add(other.gameObject);
        }

        if (other.gameObject.tag == "Antonio")
        {
            animator.SetBool("Move", false);
        }

        if (other.gameObject.tag == "Antonio Electricity" && waitBeforeAttack <= 0)
        {
            Destroy(other.gameObject);

            audioSource.PlayOneShot(electricityStunSound);

            animator.SetBool("Move", false);
            animator.SetTrigger("Electricity");
            stunTime = 2f;

            if (others.Count > 0)
            {
                hp -= 40;
            }
            else {
                hp -= 15;
                PushAttackSide(other.gameObject);
            }
        }

        if (other.gameObject.tag == "Signal")
        {
            if (!AttackCanvas.activeSelf && !DashCanvas.activeSelf)
            {
                ControlsCanvas.SetActive(true);
            }
        }

        if (other.gameObject.tag == "Win")
        {
            WinCanvas.SetActive(true);
            Time.timeScale = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Water")
        {
            others.Remove(other.gameObject);
        }

        if (other.gameObject.tag == "Signal")
        {
            ControlsCanvas.SetActive(false);
            AttackCanvas.SetActive(false);
            DashCanvas.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            if (other.gameObject.transform.position.y < gameObject.transform.position.y)
            {
                canJump = true;
                jumpTime = 0.75f;
                animator.SetBool("Jump", false);
                animator.SetBool("Fall", false);

                if (!animator.GetBool("Landing"))
                {
                    animator.SetBool("Landing", true);
                }
            }
        }

        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Antonio")
        {
            animator.SetBool("Move", false);
            animator.SetTrigger("Damage");
            stunTime = 0.5f;
            hp -= 20;
            PushAttackSide(other.gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            animator.SetBool("Move", false);
            animator.SetTrigger("Damage");
            stunTime = 0.5f;
            hp -= 20;
            PushAttackSide(other.gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D other) 
    {
        if (other.gameObject.tag == "Ground")
        {
            animator.SetBool("Landing", false);

            if (!animator.GetBool("Jump"))
            {
                animator.SetBool("Fall", true);
            }
        }
    }
}
