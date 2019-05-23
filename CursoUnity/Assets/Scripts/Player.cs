using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    public float speed;
    public int jumpForce;
    public int health;
    public Transform groundCheck;

    private bool invunerable = false;
    private bool grounded = false;
    private bool jumping = false;
    private bool facingRight = true;

    private SpriteRenderer sprite;
    private Rigidbody2D rigidBody2d;
    private Animator animator;

    public float attackRate;
    public Transform spawnAttack;
    private float nextAttack = 0f;
    public GameObject attackPrefab;
    public GameObject crown;

    private Camera cameraScript;

    public AudioClip fxHurt;
    public AudioClip fxJump;
    public AudioClip fxAttack;

    private float move;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        rigidBody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        cameraScript = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        move = Input.GetAxis("FHorizontal");
       
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        
        if (Input.GetButtonDown("FJump") && grounded)
        {
            jumping = true;
            SoundManager.instance.playSound(fxJump);
        }

        SetAnimations();

        if (Input.GetButton("FAttack") && grounded && Time.time > nextAttack)
        {
            SoundManager.instance.playSound(fxAttack);
            Attack();
        }
        
    }

    // Function that has the same purpose of function Update, but recommended to work with RigidBodies
    private void FixedUpdate()
    {

        rigidBody2d.velocity = new Vector2(move * speed, rigidBody2d.velocity.y);
        if ((move < 0f && facingRight) || (move > 0f && !facingRight))
        {
            Flip();
        }

        if (jumping)
        {
            rigidBody2d.AddForce(new Vector2(0f, jumpForce));
            jumping = false;
        }
        
    }

    void Flip()
    {
        facingRight = !facingRight;
        //transform no need to instanciate, it's already understanding the object's transform and manipulate as you want
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

    }

    void SetAnimations()
    {
        animator.SetFloat("VelY", rigidBody2d.velocity.y);
        animator.SetBool("JumpFall", !grounded);
        animator.SetBool("Walk", rigidBody2d.velocity.x != 0f && grounded);
    }

    void Attack()
    {
        animator.SetTrigger("Punch");

        nextAttack = Time.time + attackRate;

        GameObject cloneAttack = Instantiate(attackPrefab, spawnAttack.position, spawnAttack.rotation);

        if (!facingRight)
        {
            cloneAttack.transform.eulerAngles = new Vector3(180, 0, 180);
        }

    }

    IEnumerator DamageEffect()
    {
        cameraScript.ShakeCamera(0.5f, 0.1f);

        for (float i = 0f; i < 1f; i += 0.1f)
        {
            sprite.enabled = false;
            yield return new WaitForSeconds(0.1f);
            sprite.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }

        invunerable = false;
    }

    public void DamagePlayer()
    {
        if (!invunerable)
        {
            invunerable = true;
            health--;
            StartCoroutine(DamageEffect());

            SoundManager.instance.playSound(fxHurt);
            Hud.instance.RefreshLife(health);

            //=================== para inimigo dropar item, utilizar a mesma regra da coroa

            if (health < 1)
            {
                KingDeath();
                Invoke("ReloadLevel", 3f);
                gameObject.SetActive(false);
            }
        }
    }

    public void DamageWater()
    {
        health = 0;
        Hud.instance.RefreshLife(health);
        KingDeath();
        Invoke("ReloadLevel", 3f);
        gameObject.SetActive(false);
    }

    void KingDeath()
    {
        GameObject cloneCrown = Instantiate(crown, transform.position, Quaternion.identity);
        Rigidbody2D rb2dCrown = cloneCrown.GetComponent<Rigidbody2D>();
        rb2dCrown.AddForce(Vector3.up * 500);
    }

    void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
}
