// Script that creates each box on the song selection screen

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public class songtext : MonoBehaviour
{

    public Text songPreview, lengthPreview, diffPreview;

    public string songpath;

    public TextAsset bitfile;

    public string title;

    public string artist;

    public string length;

    public float difficulty;

    public string difficultyText;

    public float approachRate;

    public float hpDrain;

    public float circleSize;



    public Color32 easyColor = new Color32(242, 114, 127, 255);
    public Color32 easyColor1 = new Color32(214, 209, 202, 255);
    public Color32 easyColor2 = new Color32(230, 239, 194, 255);
    public Color32 easyColor3 = new Color32(42, 78, 94, 255);


    public Color32 hardColor = new Color32(53, 94, 126, 255);
    public Color32 hardColor1 = new Color32(50, 69, 83, 255);
    public Color32 hardColor2 = new Color32(85, 124, 131, 255);
    public Color32 hardColor3 = new Color32(16, 32, 69, 255);

    public List<Color32> easycolours;
    public List<Color32> hardcolours;

    public Image img;



    RectTransform rectransform;

    // Start is called before the first frame update
    void Start()
    {
        easycolours.Add(easyColor);
        easycolours.Add(easyColor1);
        easycolours.Add(easyColor2);
        easycolours.Add(easyColor3);

        hardcolours.Add(hardColor);
        hardcolours.Add(hardColor1);
        hardcolours.Add(hardColor2);
        hardcolours.Add(hardColor3);



        songpath = "Songs/" + gameObject.name;

        bitfile = Resources.Load<TextAsset>(songpath);

        List<string> bitfilelist = TextAssetToList(bitfile);

        img = GetComponent<Image>();

        foreach (string line in bitfilelist)
        {
            
            if (line.Contains("Difficulty:"))
            {
                string[] sArray = line.Split(':');
                difficulty = float.Parse(sArray[1]);
            }
            if (line.Contains("Title:"))
            {
                string[] sArray = line.Split(':');
                title = sArray[1];
            }
            if (line.Contains("Length:"))
            {
                string[] sArray = line.Split(':');
                TimeSpan leng = TimeSpan.FromMinutes(float.Parse(sArray[1]));
                length = string.Format("{0:D2}:{1:D2}", leng.Minutes, leng.Seconds);
            }
            if (line.Contains("Artist:"))
            {
                string[] sArray = line.Split(':');
                artist = sArray[1];
            }

        }



        if (difficulty < 30)
        {
            difficultyText = "Easy";
        }
        else if (difficulty < 40 && difficulty >= 30)
        {
            difficultyText = "Normal";
        }
        else if (difficulty < 50 && difficulty >= 40)
        {
            difficultyText = "Hard";
        }
        else if (difficulty < 55 && difficulty >= 50)
        {
            difficultyText = "Insane";
        }
        else if (difficulty < 60 && difficulty >= 55)
        {
            difficultyText = "Expert";
        }
        else if (difficulty < 65 && difficulty >= 60)
        {
            difficultyText = "Pro";
        }
        else if (difficulty > 65)
        {
            difficultyText = "Master";
        }

        var diffcolor = Mathf.RoundToInt(difficulty / 10) / 10f;

        img.color = Color32.Lerp(easycolours[PlayerPrefs.GetInt("colour")], hardcolours[PlayerPrefs.GetInt("colour")], diffcolor);

        songPreview.text = title;
        lengthPreview.text = length;
        diffPreview.text = difficultyText;


     }

    public List<string> TextAssetToList(TextAsset ta)
    {
        var listToReturn = new List<string>();
        var arrayString = ta.text.Split('\n');
        foreach (var line in arrayString)
        {
            listToReturn.Add(line);
        }
        return listToReturn;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
