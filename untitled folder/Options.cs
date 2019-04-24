using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{

    // public DiscordController discord;

    public Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();

    public Text key1, key2, key3, key4, restart, back, musicvol, effectsvol;

    public InputField setName;

    private GameObject currentKey;

    public Sprite unclicked, selecting;
    public Dropdown dropper;

    public Sprite[] backgrounds;
    public Image background;

    public Toggle relax;

    public Slider music, effects;

    public AudioSource click, hover;

    public Color textUnclicked, textSelecting;

    public GameObject faded;
    public fadeAnimator fadedImage;

    // Start is called before the first frame update
    void Start()
    {
        /*
        discord.presence.largeImageKey = "bitcircle";
        discord.presence.state = "Changing settings";
        DiscordRpc.UpdatePresence(discord.presence);
        */

        background.sprite = backgrounds[PlayerPrefs.GetInt("colour")];

        key1.text = PlayerPrefs.GetString("key1");
        key2.text = PlayerPrefs.GetString("key2");
        key3.text = PlayerPrefs.GetString("key3");
        key4.text = PlayerPrefs.GetString("key4");
        restart.text = PlayerPrefs.GetString("restart");
        back.text = PlayerPrefs.GetString("back");

        music.value = PlayerPrefs.GetInt("songvolume");
        effects.value = PlayerPrefs.GetInt("hitsoundvolume");
        dropper.value = PlayerPrefs.GetInt("colour");

        click.volume = effects.value / 100;
        hover.volume = effects.value / 100;

        musicvol.text = music.value.ToString();
        effectsvol.text = effects.value.ToString();

        fadedImage = faded.GetComponent<fadeAnimator>();

        print(PlayerPrefs.GetInt("easy"));

        if (PlayerPrefs.GetInt("easy") == 0) {

            relax.GetComponent<Toggle>().isOn = false;
            
        } else
        {
            relax.GetComponent<Toggle>().isOn = true;
        }

        print(relax.GetComponent<Toggle>().isOn);

        setName.transform.GetChild(0).GetComponent<Text>().text = PlayerPrefs.GetString("name");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        if(currentKey != null)
        {
            Event e = Event.current;
            if(e.isKey)
            {
                PlayerPrefs.SetString(currentKey.name, e.keyCode.ToString());
                currentKey.transform.GetChild(0).GetComponent<Text>().text = e.keyCode.ToString();
                currentKey.GetComponent<Image>().sprite = unclicked;
                currentKey.transform.GetChild(0).GetComponent<Text>().color = textUnclicked;
                currentKey = null;
                click.Play();
            }
        }
    }

    public void ChangeKey(GameObject clicked)
    {

        if(currentKey != null)
        {
            currentKey.GetComponent<Image>().sprite = unclicked;
            currentKey.transform.GetChild(0).GetComponent<Text>().color = textUnclicked;
        }

        currentKey = clicked;
        currentKey.GetComponent<Image>().sprite = selecting;
        currentKey.transform.GetChild(0).GetComponent<Text>().color = textSelecting;
        hover.Play();
    }

    public void ChangeName(Text name)
    {
        PlayerPrefs.SetString("name", name.text);
    }

    public void EasyMode(Toggle status)
    {
        if(status.isOn)
        {
            PlayerPrefs.SetInt("easy", 1);
        } else
        {
            PlayerPrefs.SetInt("easy", 0);
        }
        print(PlayerPrefs.GetInt("easy"));
        click.Play();
    }

    public void musicChange()
    {
        PlayerPrefs.SetInt("songvolume", Mathf.RoundToInt(music.value));
        musicvol.text = Mathf.RoundToInt(music.value).ToString();
        hover.Play();
    }

    public void effectsChange()
    {
        PlayerPrefs.SetInt("hitsoundvolume", Mathf.RoundToInt(effects.value));
        effectsvol.text = Mathf.RoundToInt(effects.value).ToString();
        click.volume = effects.value / 100;
        hover.volume = effects.value / 100;
        hover.Play();

    }

    public void BackToMenu()
    {
        fadedImage.StartCoroutine(fadedImage.FadeImage(false, "Main Menu"));
        click.Play();
    }

    public void DropdownChange(Dropdown dropdown)
    {
        background.sprite = backgrounds[dropdown.value];
        faded.GetComponent<Image>().sprite = backgrounds[dropdown.value];
        PlayerPrefs.SetInt("colour", dropdown.value);
        click.Play();
    }
}
