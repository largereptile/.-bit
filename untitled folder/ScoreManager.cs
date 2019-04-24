// Manages score for the ending screen

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ScoreManager : MonoBehaviour {

    public songmanager songmanager;

    public int notecount;

    public float accuracy;

    public Text combotext;

    public Text scoretext;

    public Text accuracytext;

    public Text statetext;

    public int consecutivenotes;

    KeyCode[] keysToPress = { KeyCode.D, KeyCode.F, KeyCode.J, KeyCode.K };

    public int combo;

    public int maxcombo = 0;

    public int misses;

    public int score;

    public string rank;
    public float multiplier;

    public int[] noteinplace = { 0, 0, 0, 0 };

	// Use this for initialization
	void Start () {
        songmanager = GameObject.FindGameObjectWithTag("songmanager").GetComponent<songmanager>();
        combo = 1;
        combotext.text = combo.ToString() + "x";
        if (!songmanager.relax)
        {
            multiplier = 1 + (songmanager.approachRate * 0.02f);
        } else if (songmanager.relax)
        {
            multiplier = 0.5f;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (consecutivenotes > 32)
        {
            combo = 4;
        }
        else if (consecutivenotes > 16) {
            combo = 3;
        }
        else if (consecutivenotes > 8) {
            combo = 2;
        }
        else if (consecutivenotes > 0) {
            combo = 1;
        }

        if (consecutivenotes > maxcombo) {
            maxcombo = consecutivenotes;
        }
        try
        {
            accuracy = (float) notecount / (float) (notecount + misses) * 100;
        }
        catch (Exception e)
        {
            accuracy = 100f;
        }

        if (accuracy == 100.00)
        {
            rank = "S";
        }
        else if (accuracy < 100 && accuracy >= 95)
        {
            rank = "A";
        }
        else if (accuracy < 95 && accuracy >= 90)
        {
            rank = "B";
        }
        else if (accuracy < 90 && accuracy >= 80)
        {
            rank = "C";
        }
        else if (accuracy < 80)
        {
            rank = "D";
        }

        statetext.text = rank;
        combotext.text = combo.ToString() + "x";
        scoretext.text = score.ToString();
        accuracytext.text = accuracy.ToString("F2");

         }               
}
