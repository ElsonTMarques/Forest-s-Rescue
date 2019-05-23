using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Snake : MonoBehaviour
{
    private bool facingRight = true;
    private Rigidbody2D rigidBody2d;
    private SpriteRenderer sprite;
    private bool touchedWall = false;

    public int health;
    private float speed;
    public Transform wallCheck;
    private int counter;
    public GameObject crystal;

    // Start is called before the first frame update
    void Start()
    {
        speed = Mathf.FloorToInt(Random.Range(3f, 6f));
        sprite = GetComponent<SpriteRenderer>();
        rigidBody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        touchedWall = Physics2D.Linecast(transform.position, wallCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        if (touchedWall)
        {
            Flip();
            Debug.Log("Snake Speed: " + speed);
        }

    }

    private void FixedUpdate()
    {
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
            //Destroy(collision);
        }
    }

    IEnumerator DamageEffect()
    {
        float actualSpeed = speed;
        speed = speed * -1;
        sprite.color = Color.red;
        rigidBody2d.AddForce(new Vector2(0f, 200f));
        yield return new WaitForSeconds(0.1f);
        speed = actualSpeed;
        sprite.color = Color.white;
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
