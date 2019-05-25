using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Enemy : MonoBehaviour
{
    public bool facingRight = false;
    private Rigidbody2D rigidBody2d;
    private SpriteRenderer sprite;
    private bool touchedWall = false;

    public int health;
    private float speed;
    public Transform wallCheck;
    private int counter;
    public GameObject crystal;

    public float m_DamagedSpeed = 700.0f;

    private bool m_Damaged;

    private Transform m_PlayerPosition;

    // Start is called before the first frame update
    void Start()
    {
        m_PlayerPosition = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            
        speed = Mathf.FloorToInt(Random.Range(3f, 6f));
        sprite = GetComponent<SpriteRenderer>();
        rigidBody2d = GetComponent<Rigidbody2D>();
        Flip();
    }

    // Update is called once per frame kkkkkkkk teste.
    void Update()
    {
        touchedWall = Physics2D.Linecast(transform.position, wallCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        if (touchedWall)
        {
            Flip();
        }

    }

    private void FixedUpdate()
    {
        if (m_Damaged)
            return;

        rigidBody2d.velocity = new Vector2(speed, rigidBody2d.velocity.y);
    }

    void Flip()
    {
        facingRight = !facingRight;
        //transform no need to instanciate, it's already understanding the object's transform and manipulate as you want
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        speed *= -1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Attack"))
        {
            DamageEnemy();
        }
    }

    IEnumerator DamageEffect()
    {
        sprite.color = Color.red;
        m_Damaged = true;

        rigidBody2d.velocity = Vector2.zero;

        Vector3 direction = transform.position - m_PlayerPosition.position;
        rigidBody2d.AddForce(new Vector2(direction.normalized.x * m_DamagedSpeed, 200f));
        yield return new WaitForSeconds(0.1f);


        sprite.color = Color.white;
        m_Damaged = false;
    }

    void DamageEnemy()
    {
        health--;
        StartCoroutine(DamageEffect());

        if (health < 1)
        {
            EnemyDeath();
            Destroy(gameObject);
            counter++;
        }
    }

    void EnemyDeath()
    {
        GameObject cloneCrystal = Instantiate(crystal, transform.position, Quaternion.identity);
        Rigidbody2D rb2dCrown = cloneCrystal.GetComponent<Rigidbody2D>();
        rb2dCrown.AddForce(Vector3.up * 500);
    }

}
