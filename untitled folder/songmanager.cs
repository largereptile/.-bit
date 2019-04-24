using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using UnityEngine.UI;
using UnityEngine.Networking;

public class songmanager : MonoBehaviour
{

    public float songPosition;

    public float dsptimesong;

    // public DiscordController discord;

    public AudioSource dogsong;

    public AudioSource bonetrousle;

    public AudioSource megalovania;

    public AudioSource dramaturgy;

    public AudioSource sidetracked;

    public AudioSource hitSound;

    public AudioClip musicToLoad;

    public float health = 100;

    public float bpm;

    public int nextIndex = 0;

    public string songpath;

    float[] xCords = { -2.25f, -0.75f, 0.75f, 2.25f };

    public struct Note
    {
        public float timestamp;
        public int xIndex;
    };

    public Note[] notes;

    public bool offsetting = true;

    public int offset = 3;

    public float beatsShownInAdvance;

    public bool hasStarted;
    public bool hasEnded = false;

    public NoteObject note;

    public string songChoice;

    public List<NoteObject> clones = new List<NoteObject>();

    public ScoreManager score;

    public TextAsset bitfile;

    public AudioSource music;

    public AudioSource[] songbox;

    public GameObject hpbar;

    public GameObject overlay;

    public float approachRate;

    public KeyCode restart, back;

    public bool relax;

    public float hpDrain;

    StreamWriter writer;

    public bool loading = false;

    public string songTitle, songArtist;

    public GameObject faded;
    public fadeAnimator fadedImage;

    public SpriteRenderer bg;
    public Sprite[] bGs;

    // Use this for initialization
    void Start()
    {


        if(PlayerPrefs.GetInt("easy") == 0)
        {
            relax = false;

        } else
        {
            relax = true;
        }


        bg.sprite = bGs[PlayerPrefs.GetInt("colour")];

        fadedImage = faded.GetComponent<fadeAnimator>();

        songChoice = PlayerPrefs.GetString("songChoice");

        hitSound.volume = PlayerPrefs.GetInt("hitsoundvolume") / 100f;

        restart = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("restart"));
        back = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("back"));

        Directory.CreateDirectory("Scores");

        score = GameObject.FindGameObjectWithTag("songmanager").GetComponent<ScoreManager>();

        foreach (AudioSource a in songbox)
        {
            if (songChoice == a.name)
            {
                music = a;
                music.volume = PlayerPrefs.GetInt("songvolume")/100f;
            }
        }

        if (music.name != "lavender")
        {
            overlay.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        }

        songpath = "Songs/" + music.name;

        bitfile = Resources.Load<TextAsset>(songpath);

        List<string> bitfilelist = TextAssetToList(bitfile);

        var lineCount = bitfilelist.Count;

        int fileIndex = 0;

        notes = new Note[lineCount];

        foreach (string line in bitfilelist) {
            if (line.Contains("|")) {
                string[] sArray = line.Split('|');

                Note noot = new Note
                {
                    timestamp = float.Parse(sArray[0]),
                    xIndex = int.Parse(sArray[1])
                };


                notes[fileIndex] = noot;

                fileIndex++;
            }

            if (line.Contains("Title:"))
            {
                string[] sArray = line.Split(':');
                songTitle = sArray[1];
            }
            if (line.Contains("Artist:"))
            {
                string[] sArray = line.Split(':');
                songArtist = sArray[1];
            }

        }
        approachRate = PlayerPrefs.GetInt("approach");

        hpDrain = 0.4f;
        music.Play();
        music.Pause();


        beatsShownInAdvance = 10;

        System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        int cur_time = (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;
        /*
        discord.presence.largeImageKey = music.name;
        discord.presence.details = "Playing " + songTitle;
        discord.presence.state = "by " + songArtist;
        discord.presence.startTimestamp = cur_time;
        discord.presence.endTimestamp = cur_time + Mathf.RoundToInt(music.clip.length);
        DiscordRpc.UpdatePresence(discord.presence);
        */


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
        if (health > 100)
        {
            health = 100;
        } else if (health < 0 && relax) { }

        if (!hasStarted && !hasEnded)
        {
            hasStarted = true;
            StartCoroutine("Offset");
        }
        else if (!hasEnded)
        {
            hpbar.transform.localScale = new Vector3(health * 0.06f, 0.1f, 1);
            if (Input.GetKeyDown(restart)) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
            }

            if (Input.GetKeyDown(back)) {
                music.Pause();
                hasEnded = true;
                fadedImage.StartCoroutine(fadedImage.FadeImage(false, "Song Selection"));
            }

            if (offsetting)
            {
                songPosition = 0;
            }

            else
            {
                songPosition = (float)(AudioSettings.dspTime - dsptimesong);
            }

            if (songPosition > music.clip.length || (health <= 0 && !relax))
            {
                hasEnded = true;
                DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                int cur_time = (int)(DateTime.UtcNow - epochStart).TotalSeconds;
                writer = new StreamWriter("Scores/" + music.name + "_" + cur_time + ".txt", true);
                float accuracy = (float)(score.notecount / (float)(score.notecount + score.misses) * 100);
                writer.WriteLine("Name: " + music.name);
                if (health <= 0)
                {
                    writer.WriteLine("Rank: F");
                } else
                {
                    writer.WriteLine("Rank: " + score.rank);
                }
                writer.WriteLine("Title: " + songTitle);
                writer.WriteLine("Player: " + PlayerPrefs.GetString("name"));
                writer.WriteLine("Score: " + score.score.ToString());
                writer.WriteLine("Accuracy: " + accuracy.ToString("F2") + "%");
                writer.WriteLine("MaxCombo: " + score.maxcombo.ToString());
                writer.WriteLine("Hits: " + score.notecount);
                writer.WriteLine("Misses: " + score.misses);

                writer.Close();
                fadedImage.StartCoroutine(fadedImage.FadeImage(false, "Score Screen"));
            }

            if (nextIndex < notes.Length && notes[nextIndex].timestamp < songPosition + beatsShownInAdvance)
            {
                var clone = Instantiate(note, new Vector3(xCords[notes[nextIndex].xIndex], 10f, 0f), new Quaternion(0f, 0f, 0f, 0f));
                clone.name = notes[nextIndex].timestamp.ToString();
                clones.Add(clone);
                nextIndex++;
            }
        }

    }

    IEnumerator Offset()
    {
        yield return new WaitForSeconds(1);
        offsetting = false;
        music.Play();
        dsptimesong = (float)AudioSettings.dspTime;
    }

}
