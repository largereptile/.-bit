// Generates a score screen using the score manager

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;
using UnityEngine.UI;

public class scorescreen : MonoBehaviour {

    // public DiscordController discord;

    public Text scoretext, hit, miss, acc, combo, rank, title; 

    public Sprite s, a, b, c, d, f;

    public Dictionary<string, Sprite> rankImages = new Dictionary<string, Sprite>();

    public AudioSource click, hover;

    public Image bg;
    public Sprite[] bGs;

    public GameObject faded;
    public fadeAnimator fadedImage;

    public string scorelog, hitlog, misslog, acclog, combolog, ranklog, songname;

    // Use this for initialization
    void Start () {
        /*
        discord.presence.largeImageKey = "bitcircle";
        discord.presence.state = "Evaluating Performance";
        DiscordRpc.UpdatePresence(discord.presence);
        */
        fadedImage = faded.GetComponent<fadeAnimator>();
        bg.sprite = bGs[PlayerPrefs.GetInt("colour")];

        rankImages.Add("S", s);
        rankImages.Add("A", a);
        rankImages.Add("B", b);
        rankImages.Add("C", c);
        rankImages.Add("D", f);
        rankImages.Add("F", f);


        DirectoryInfo info = new DirectoryInfo("Scores");
        FileInfo[] files = info.GetFiles().OrderBy(p => p.CreationTime).ToArray();

        for (int i = 0; i < files.Length / 2; i++)
        {
            FileInfo tmp = files[i];
            files[i] = files[files.Length - i - 1];
            files[files.Length - i - 1] = tmp;
        }

        StreamReader reader = files[0].OpenText();


        String itemStrings = reader.ReadLine();

        while (itemStrings != null)
        {
            string[] sArray = itemStrings.Split(' ');
            if (sArray[0] == "Score:") {
                scoretext.text = sArray[1];
                scorelog = sArray[1];
            }
            if (sArray[0] == "Hits:") {
                hit.text = sArray[1];
                hitlog = sArray[1];
            }
            if (sArray[0] == "Accuracy:")
            {
                acc.text = sArray[1];
                acclog = sArray[1];
            }
            if (sArray[0] == "Misses:")
            {
                miss.text = sArray[1];
                misslog = sArray[1];
            }
            if (sArray[0] == "MaxCombo:")
            {
                combo.text = sArray[1] + "x";
                combolog = sArray[1];
            }
            if(sArray[0] == "Rank:")
            {
                rank.text = sArray[1];
            }
            if (sArray[0] == "Name:")
            {
                songname = sArray[1];
            }
            if (sArray[0] == "Title:")
            {
                string sep = " ";
                title.text = string.Join(sep, sArray, 1, sArray.Count() - 1);
            }

            itemStrings = reader.ReadLine();
        }

        if(!PlayerPrefs.HasKey(songname))
        {
            PlayerPrefs.SetString(songname, scorelog + "," + hitlog + "," + misslog + "," + acclog + "," + combolog + "," + ranklog + "," + PlayerPrefs.GetString("name"));
        } else
        {
            var score = PlayerPrefs.GetString(songname).Split(',')[0];
            if (int.Parse(scorelog) > int.Parse(score))
            {
                PlayerPrefs.SetString(songname, scorelog + "," + hitlog + "," + misslog + "," + acclog + "," + combolog + "," + ranklog + "," + PlayerPrefs.GetString("name"));
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void BackToMenu()
    {
        fadedImage.StartCoroutine(fadedImage.FadeImage(false, "Song Selection"));
        click.Play();
    }

    public void TryAgain()
    {
        fadedImage.StartCoroutine(fadedImage.FadeImage(false, "Gameplay"));
        click.Play();
    }

}
