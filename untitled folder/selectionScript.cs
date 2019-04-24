using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine.SceneManagement;

public class selectionScript : MonoBehaviour
{

    GraphicRaycaster raycaster;

    // public DiscordController discord;

    public string[] songList = { "soundscape", "superdriver", "undercover", "freedomdive" };

    public GameObject albumCover;
    public Image albumImage;
    public Sprite[] coverList;

    public GameObject song;
    public GameObject canvas;
    public int counter = 0;

    public GameObject spinner, circle;
    public Sprite[] diff1, diff2, diff3, diff4;
    public Sprite[] circle1, circle2, circle3, circle4;
    public Image circleImage;
    public Image backgroundImage;

    public Text title;
    public Text length;
    public Text od;
    public Text artext, arspeed;

    public Text topscore, score, topacc, topcomb;

    public GameObject selected;
    public songtext selectedtext;

    public GameObject faded;
    public fadeAnimator fadedImage;

    public AudioSource click;
    public AudioSource hover;

    public KeyCode back;

    public Slider approach;

    public List<Sprite[]> diffs = new List<Sprite[]>();
    public List<Sprite[]> circs = new List<Sprite[]>();

    void Awake()
    {
        // Get both of the components we need to do this
        this.raycaster = canvas.GetComponent<GraphicRaycaster>();
        
    }


    // Start is called before the first frame update
    void Start()
    {
        diffs.Add(diff1);
        diffs.Add(diff2);
        diffs.Add(diff3);
        diffs.Add(diff4);

        circs.Add(circle1);
        circs.Add(circle2);
        circs.Add(circle3);
        circs.Add(circle4);



        /*
        discord.presence.largeImageKey = "bitcircle";
        discord.presence.state = "Selecting a song";
        DiscordRpc.UpdatePresence(discord.presence);
        */

        back = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("back"));

        click.volume = PlayerPrefs.GetInt("hitsoundvolume") / 100f;
        hover.volume = PlayerPrefs.GetInt("hitsoundvolume") / 100f;

        backgroundImage.sprite = diffs[PlayerPrefs.GetInt("colour")][0];
        circleImage.sprite = circs[PlayerPrefs.GetInt("colour")][0];

        approach.value = PlayerPrefs.GetInt("approach");

        fadedImage = faded.GetComponent<fadeAnimator>();
        foreach (string songName in songList)
        {
            var songBox = Instantiate(song, Vector2.zero, Quaternion.identity);
            songBox.name = songName;
            if (counter == 0)
            {
                selected = songBox;
                selectedtext = songBox.GetComponent<songtext>();
            }
            songBox.transform.SetParent(canvas.transform);
            var rectransform = songBox.GetComponent<RectTransform>();
            float x_coord = Mathf.Abs(counter / 4) + 671;
            rectransform.offsetMin = new Vector2(x_coord, counter);
            rectransform.offsetMax = new Vector2(x_coord, counter);
            counter += 250;
        }
        circle.transform.SetAsLastSibling();
        spinner.transform.SetAsLastSibling();
        albumImage = albumCover.GetComponent<Image>();
        artext.text = "Approach Rate: " + PlayerPrefs.GetInt("approach").ToString();
        arspeed.text = "(" + GetAR(PlayerPrefs.GetInt("approach")) + ")";
    }

    // Update is called once per frame
    void Update()
    {
        //Set up the new Pointer Event
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        pointerData.position = Input.mousePosition;

        if (Input.GetKeyDown(back))
        {
            fadedImage.StartCoroutine(fadedImage.FadeImage(false, "Main Menu"));
        }

        this.raycaster.Raycast(pointerData, results);
        if (results.Count != 0)
        {
            foreach (RaycastResult result in results)
            {
                if (Input.GetMouseButtonDown(0) && result.gameObject.tag == "songbox")
                {
                    click.Play();
                    PlayerPrefs.SetString("songChoice", result.gameObject.name);
                    fadedImage.StartCoroutine(fadedImage.FadeImage(false, "Gameplay"));
                }

                if (result.gameObject.tag == "songbox" && result.gameObject.name != selected.name)
                {
                    hover.Play();
                    selected = result.gameObject;
                    selectedtext = selected.GetComponent<songtext>();
                    title.text = selectedtext.title + " - " + selectedtext.artist;
                    od.text = "Overall Difficulty: " + selectedtext.difficulty.ToString("F2");
                    length.text = "Length: " + selectedtext.length;

                    if(selectedtext.difficulty > 55)
                    {
                        backgroundImage.sprite = diffs[PlayerPrefs.GetInt("colour")][2];
                        circleImage.sprite = circs[PlayerPrefs.GetInt("colour")][2];
                    } else if(selectedtext.difficulty > 35)
                    {
                        backgroundImage.sprite = diffs[PlayerPrefs.GetInt("colour")][1];
                        circleImage.sprite = circs[PlayerPrefs.GetInt("colour")][1];
                    }
                    else
                    {
                        backgroundImage.sprite = diffs[PlayerPrefs.GetInt("colour")][0];
                        circleImage.sprite = circs[PlayerPrefs.GetInt("colour")][0];
                    }


                    foreach (Sprite cover in coverList)
                    {
                        if (cover.name == selected.name)
                        {
                            albumImage.sprite = cover;
                        }
                    }

                    if (PlayerPrefs.HasKey(selected.name))
                    {
                        var topscorestring = PlayerPrefs.GetString(selected.name).Split(',');
                        topscore.text = "Top Score: " + topscorestring[6];
                        score.text = "Score: " + topscorestring[0];
                        topacc.text = "Accuracy: " + topscorestring[3];
                        topcomb.text = "Combo: " + topscorestring[4] + "x";
                    } else
                    {
                        topscore.text = "Top Score: None";
                        score.text = "Score: 0";
                        topacc.text = "Accuracy: 0.00%";
                        topcomb.text = "Combo: 0x";
                    }
                }
            }
        }
    }

    public void arChange(Slider slide)
    {
        PlayerPrefs.SetInt("approach", Mathf.RoundToInt(slide.value));
        artext.text = "Approach Rate: " + Mathf.RoundToInt(slide.value).ToString();
        arspeed.text = "(" + GetAR(PlayerPrefs.GetInt("approach")) + ")";
        hover.Play();
    }

    public void BackToMenu()
    {
        fadedImage.StartCoroutine(fadedImage.FadeImage(false, "Main Menu"));
        click.Play();
    }

    public string GetAR(int approach)
    {
        if (approach < 2)
        {
            return "Very Slow";
        } else if (approach < 4)
        {
            return "Slow";

        } else if (approach < 7)
        {
            return "Medium";
        } else if (approach < 10)
        {
            return "Fast";

        } else { 
            return "Very Fast"; 
        }

    }

}
