using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class buttoncontroller : MonoBehaviour {

    private SpriteRenderer theSR;
    private SpriteRenderer missSR;

    public Sprite defaultImage;
    public Sprite pressedImage;

    public Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();
    public KeyCode keyToPress;

    public Color originalColour;

    public songmanager songmanager;

    public ScoreManager score;

    public float missNumber = 255;
    public Color32 misscolor = new Color(255, 0, 0, 255);
    public Color32 misscolor2 = new Color(255, 255, 255, 127.5f);
    public Color32 newcolor;
    public float misstimeleft = 0;

    public GameObject missline;

    public List<Collider2D> notesinObject;

	// Use this for initialization
	void Start () {

        keys.Add("key1", (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("key1")));
        keys.Add("key2", (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("key2")));
        keys.Add("key3", (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("key3")));
        keys.Add("key4", (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("key4")));

        keyToPress = keys[gameObject.name];

        score = GameObject.FindGameObjectWithTag("songmanager").GetComponent<ScoreManager>();
        theSR = GetComponent<SpriteRenderer>();
        missSR = missline.GetComponent<SpriteRenderer>();
        theSR.sprite = defaultImage;
        originalColour = theSR.color;
        notesinObject = new List<Collider2D>();
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(keyToPress)){
            theSR.sprite = pressedImage;
        }
        if (Input.GetKeyUp(keyToPress))
        {
            theSR.sprite = defaultImage;
        }

        if (misstimeleft > Time.deltaTime)
        {
            newcolor = Color32.Lerp(misscolor, misscolor2, (Time.deltaTime / misstimeleft) * 2);
            missSR.material.color = newcolor;
            misstimeleft -= Time.deltaTime;
        }

        if (Input.GetKeyDown(keyToPress) && notesinObject.Count == 0)
        {
            score.consecutivenotes = 0;
        }

        if (songmanager.clones.Count != 0 && notesinObject.Count != 0)
        {

            if (notesinObject[0].name == songmanager.clones[0].name && Input.GetKeyDown(keyToPress))
            {

                notesinObject.Remove(notesinObject[0]);
                StartCoroutine("Hit", songmanager.clones[0]);
                songmanager.health += songmanager.hpDrain * 10;
                songmanager.clones.Remove(songmanager.clones[0]);
                score.notecount++;
                score.consecutivenotes++;
                score.score += Mathf.RoundToInt(score.combo * 100 * score.multiplier);
            }
        }


	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        notesinObject.Add(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        try
        {
            if (notesinObject[0].name == collision.name)
            {
                notesinObject.Remove(collision);
                misstimeleft = 0.25f;
                StartCoroutine("Miss", songmanager.clones[0]);
                songmanager.health -= 10;
                songmanager.clones.Remove(songmanager.clones[0]);
                score.misses++;
                score.consecutivenotes = 0;
                score.combo = 1;
            }
        } catch (System.ArgumentOutOfRangeException e) {
            
        }
    }

    IEnumerator Miss(NoteObject note)
    {
        note.GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(2);
        note.gameObject.SetActive(false);
    }

    IEnumerator Hit(NoteObject note)
    {
        note.moving = false;
        note.GetComponent<Collider2D>().enabled = false;
        songmanager.hitSound.Play();
        var col = note.theSR.color;
       
        for (float f = 1f; f >= 0; f -= Time.deltaTime * 2)
        {
            note.transform.localScale += new Vector3(Time.deltaTime, Time.deltaTime, 0);
            col.a = f;
            note.theSR.color = col;
            yield return null;
        }

        note.gameObject.SetActive(false);
    }

}



