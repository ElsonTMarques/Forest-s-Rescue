using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerTrigger : MonoBehaviour
{
    private Player playerScript;
    public Text enemiesKilled;
    public Text pontosCollecteds;
    public Text userMessage;
    private Animator animator;

    private int count;
    public int countPontos;
    public AudioClip fxCoin;
    private GameObject crystalVerify;

    // Start is called before the first frame update
    void Start()
    {
        playerScript = GameObject.Find("Player").GetComponent<Player>();
        crystalVerify = GameObject.Find("CrystalVerify");
        animator = GameObject.Find("Box").GetComponent<Animator>();
        count = 0;
        countPontos = 0;
        SetEnemiesKilledCount();
        SetPontosCount();
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
            countPontos = countPontos + 2;
            SoundManager.instance.playSound(fxCoin);
            Destroy(collision.gameObject);
            SetPontosCount();
        }

        if (collision.CompareTag("Crystal"))
        {
            count++;
            Destroy(collision.gameObject);
            SetEnemiesKilledCount();
        }


        if (collision.CompareTag("Box"))
        {
            if (count >= 15)
            {
                animator.SetBool("GotBox", true);
                Invoke("GoToNextLevel", 3f);
            }
            else
            {
                userMessage.GetComponent<Text>().text = "Você deve coletar 15 cristais para avançar!!!";
                Invoke("DisplayMessageVerify", 5f);
            }
        }

        if (collision.CompareTag("CrystalVerifyTag"))
        {
            if (count >= 15)
            {
                crystalVerify.SetActive(false);
            }
            else
            {
                userMessage.GetComponent<Text>().text = "Você deve coletar 15 cristais para avançar!!!";
                Invoke("DisplayMessageVerify", 5f);
            }
        }

        if (collision.CompareTag("GenericSign"))
        {
            userMessage.GetComponent<Text>().text = "Olá, Bem vindo ao Forest Rescue, Você agora é o Rei desta floresta e " +
                "estão tentando tomar ela de você, seu papel é impedir que isto aconteça, para isso você deve eliminar os inimigos " +
                "coletando no minimo 15 Cristais que aparecem quando você destroi um inimigo para avançar no jogo";
            Invoke("DisplayMessageVerify", 8f);
        }
    }


    void DisplayMessageVerify()
    {
        userMessage.GetComponent<Text>().text = "";
    }

    void SetEnemiesKilledCount()
    {
        enemiesKilled.GetComponent<Text>().text = "Crystals: " + count.ToString();
    }

    public void SetPontosCount()
    {
        pontosCollecteds.GetComponent<Text>().text = "Pontos: " + countPontos.ToString();
    }

    void GoToNextLevel()
    {
        animator.SetBool("GotBox", false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
