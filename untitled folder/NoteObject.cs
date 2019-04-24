// Script assigned to each note object

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteObject : MonoBehaviour {

    public bool canBePressed;

    public KeyCode KeyToPress;

    public SpriteRenderer theSR;
    readonly KeyCode[] keysToPress = {KeyCode.D, KeyCode.F, KeyCode.J, KeyCode.K};

    int arrayPos;

    public songmanager songmanager;

    public ScoreManager score;

    public bool moving = true;

    float currentpos;


    int xVal;

	// Use this for initialization
	void Start ()
    {
        songmanager = GameObject.FindGameObjectWithTag("songmanager").GetComponent<songmanager>();
        score = GameObject.FindGameObjectWithTag("songmanager").GetComponent<ScoreManager>();
        arrayPos = songmanager.nextIndex - 1;
        xVal = songmanager.notes[arrayPos].xIndex;
        KeyToPress = keysToPress[xVal];
        theSR = GetComponent<SpriteRenderer>();

	}

    // Update is called once per frame
    void Update()
    {
        if (songmanager.hasStarted)
        {

            if (arrayPos < songmanager.notes.Length && moving)
            {
                currentpos = ((songmanager.notes[arrayPos].timestamp - songmanager.songPosition)) * songmanager.approachRate;
                gameObject.transform.position = new Vector3(transform.position.x, currentpos, transform.position.z);
            }
        }
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.tag == "Activator")
        {
            canBePressed = true;
          /*  score.noteinplace[xVal] = 1; */
        }
	}

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Activator")
        {
            canBePressed = false;
        /*    score.noteinplace[xVal] = 0; */
        }
    }



    IEnumerator Miss() {
        theSR.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        var col = theSR.color;
        col.a = 0.5f;
        yield return new WaitForSeconds(0.1f);
        gameObject.SetActive(false);
    }

    IEnumerator Hit() {
        theSR.color = Color.green;
        yield return new WaitForSeconds(0.1f);
        var col = theSR.color;
        col.a = 0.5f;
        yield return new WaitForSeconds(0.1f);
        gameObject.SetActive(false);
    }
}
