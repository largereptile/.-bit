// Current main menu script

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine.SceneManagement;

public class MainMenuhover : MonoBehaviour
{
    GraphicRaycaster raycaster;

    // public DiscordController discord;

    public GameObject canvas;

    public Image background;
    public Sprite[] bGs;

    public GameObject[] buttons;
    public GameObject[] selectedButtons;
    public Text[] selectedTexts;

    public GameObject selectedButton;
    public GameObject selectedState;
    public Image selectedImage;
    public GameObject selectedFail;
    public Text selectedTextFail;
    public Text selectedText;
    public int selectedPos;
    public bool currentlySelected = false;

    public Color32 selectedColor;
    public Color32 unselectedColor;

    public GameObject faded;
    public fadeAnimator fadedImage;

    public AudioSource click;
    public AudioSource hover;

    public Dictionary<string, KeyCode> defaultKeybinds = new Dictionary<string, KeyCode>();
    public Dictionary<string, int> defaultPrefs = new Dictionary<string, int>();

    void Awake()
    {
        defaultKeybinds.Add("key1", KeyCode.D);
        defaultKeybinds.Add("key2", KeyCode.F);
        defaultKeybinds.Add("key3", KeyCode.J);
        defaultKeybinds.Add("key4", KeyCode.K);
        defaultKeybinds.Add("restart", KeyCode.R);
        defaultKeybinds.Add("back", KeyCode.Escape);

        defaultPrefs.Add("colours", 0);
        defaultPrefs.Add("songvolume", 100);
        defaultPrefs.Add("hitsoundvolume", 100);
        defaultPrefs.Add("easy", 0);
        defaultPrefs.Add("approach", 5);
        defaultPrefs.Add("colour", 0);

        // Get both of the components we need to do this
        this.raycaster = canvas.GetComponent<GraphicRaycaster>();

    }

    // Start is called before the first frame update
    void Start()
    {
        /*
        discord.presence.largeImageKey = "bitcircle";
        discord.presence.state = "Perusing Main Menu";
        DiscordRpc.UpdatePresence(discord.presence);
        */
        click.volume = PlayerPrefs.GetInt("hitsoundvolume") / 100f;
        hover.volume = PlayerPrefs.GetInt("hitsoundvolume") / 100f;

        foreach (KeyValuePair<string, KeyCode> entry in defaultKeybinds)
        {
            if(!PlayerPrefs.HasKey(entry.Key))
            {
                PlayerPrefs.SetString(entry.Key, entry.Value.ToString());
            }
        }

        foreach (KeyValuePair<string, int> entry in defaultPrefs)
        {
            if (!PlayerPrefs.HasKey(entry.Key))
            {
                PlayerPrefs.SetInt(entry.Key, entry.Value);
            }
        }

        if(!PlayerPrefs.HasKey("name"))
        {
            PlayerPrefs.SetString("name", "player");
        }

        fadedImage = faded.GetComponent<fadeAnimator>();
        selectedImage = selectedButton.GetComponent<Image>();
        background.sprite = bGs[PlayerPrefs.GetInt("colour")];
    }

    // Update is called once per frame
    void Update()
    {

        //Set up the new Pointer Event
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        pointerData.position = Input.mousePosition;

        this.raycaster.Raycast(pointerData, results);
        if (results.Count != 0)
        {

            currentlySelected = false;

            foreach (RaycastResult result in results)
            {

                if (Input.GetMouseButtonDown(0) && result.gameObject.name == "play")
                {
                    click.Play();
                    fadedImage.StartCoroutine(fadedImage.FadeImage(false, "Song Selection"));
                }
                else if (Input.GetMouseButtonDown(0) && result.gameObject.name == "opt")
                {
                    click.Play();
                    fadedImage.StartCoroutine(fadedImage.FadeImage(false, "Options"));
                }
                else if (Input.GetMouseButtonDown(0) && result.gameObject.name == "quit")
                {
                    click.Play();
                    Application.Quit();
                }

                for (int i = 0; i <= 2; i++)
                {
                    if (result.gameObject.name == buttons[i].name && result.gameObject.name != selectedButton.name)
                    {
                        hover.Play();
                        selectedButton = result.gameObject;
                        selectedState = selectedButtons[i];
                        selectedImage = selectedState.GetComponent<Image>();
                        selectedText = selectedTexts[i];
                        selectedPos = i;
                        StartCoroutine(FadeImage(false, selectedImage));
                        selectedText.color = selectedColor;

                    }
                }

                if (result.gameObject.name == selectedButton.name)
                {
                    currentlySelected = true;
                }

            }

        }

        if (!currentlySelected)
        {
            selectedButton = selectedFail;
            selectedState = selectedFail;
            selectedText.color = unselectedColor;
            StartCoroutine(FadeImage(true, selectedImage));
            selectedImage = selectedState.GetComponent<Image>();
        }
    }

    public IEnumerator FadeImage(bool fadeAway, Image image)
    {
        if (fadeAway)
        {
            for (float i = 0.5f; i >= 0; i -= Time.deltaTime * 2)
            {
                image.color = new Color(1, 1, 1, i);
                yield return null;
            }
        }
        else
        {
            for (float i = 0; i <= 1f; i += Time.deltaTime * 2)
            {
                image.color = new Color(1, 1, 1, i);
                yield return null;
            }
        }
    }
}
