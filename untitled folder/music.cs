// Script running the background music

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class music : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "Gameplay")
        {
            gameObject.GetComponent<AudioSource>().volume = PlayerPrefs.GetInt("songvolume") / 100f;
        }
        else
        {
            gameObject.GetComponent<AudioSource>().volume = 0;
        }
    }
}
