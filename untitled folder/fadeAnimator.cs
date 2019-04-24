// Script used for every fade transition

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine.SceneManagement;

public class fadeAnimator : MonoBehaviour
{

    public Image background;
    public RectTransform bringToFront;

    public Sprite[] backgrounds;

    bool fading;

    // Start is called before the first frame update
    void Start()
    {
        background = gameObject.GetComponent<Image>();
        bringToFront = gameObject.GetComponent<RectTransform>();
        background.sprite = backgrounds[PlayerPrefs.GetInt("colour")];
        StartCoroutine(FadeImage(true, "none"));
    }

    // Update is called once per frame
    void Update()
    {
        if(fading)
        {
            bringToFront.SetAsLastSibling();
        } else
        {
            bringToFront.SetAsFirstSibling();
        }
    }

    public IEnumerator FadeImage(bool fadeAway, string scene)
    {
        if (fadeAway)
        {
            fading = true;
            for (float i = 1f; i >= 0; i -= (Time.deltaTime * 2.25f))
            {
                background.color = new Color(1, 1, 1, i);
                yield return null;

            }
            fading = false;

        } else
        {
            fading = true;
            for (float i = 0; i <= 1f; i += (Time.deltaTime * 2.25f))
            {
                background.color = new Color(1, 1, 1, i);
                yield return null;

            }

            fading = false;

            SceneManager.LoadScene(scene);
        }
    }
}
