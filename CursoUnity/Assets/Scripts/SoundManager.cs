using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource fxSource;
    public AudioSource musicSource;

    public static SoundManager instance = null;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        //para continuidade na musica
        //DontDestroyOnLoad(gameObject);
    }

    public void playSound(AudioClip clip)
    {
        fxSource.clip = clip;
        fxSource.Play();
    }
}
