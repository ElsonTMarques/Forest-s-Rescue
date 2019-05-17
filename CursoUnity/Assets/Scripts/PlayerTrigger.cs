using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTrigger : MonoBehaviour
{
    private Player playerScript;
    public Text enemiesKilled;

    private int count;
    public AudioClip fxCoin;

    // Start is called before the first frame update
    void Start()
    {
        playerScript = GameObject.Find("Player").GetComponent<Player>();
        count = 0;
        SetEnemiesKilledCount();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            playerScript.DamagePlayer();
        }

        if (collision.CompareTag("Water"))
        {
            playerScript.DamageWater();
        }

        if (collision.CompareTag("Coin"))
        {
            SoundManager.instance.playSound(fxCoin);
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("Crystal"))
        {
            count++;
            Destroy(collision.gameObject);
            SetEnemiesKilledCount();
        }
    }

    void SetEnemiesKilledCount()
    {
        enemiesKilled.text = "Crystals: " + count.ToString();
    }
}
