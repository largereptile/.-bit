// Menu script made for exhibition build
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CheckClicks : MonoBehaviour
{
    // Normal raycasts do not work on UI elements, they require a special kind
    GraphicRaycaster raycaster;


    void Awake()
    {
        // Get both of the components we need to do this
        this.raycaster = GetComponent<GraphicRaycaster>();
    }

    void Update()
    {
        //Check if the left Mouse button is clicked
        if (Input.GetMouseButtonDown(0))
        {
            print("mouse");
            //Set up the new Pointer Event
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            List<RaycastResult> results = new List<RaycastResult>();

            //Raycast using the Graphics Raycaster and mouse click position
            pointerData.position = Input.mousePosition;
            print(pointerData.position);
            this.raycaster.Raycast(pointerData, results);

            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.name == "dramaturgy") {
                    PlayerPrefs.SetString("songChoice", "drama");
                    SceneManager.LoadScene("Gameplay");
                }
                if (result.gameObject.name == "sidetracked day")
                {
                    PlayerPrefs.SetString("songChoice", "sidetrack");
                    SceneManager.LoadScene("Gameplay");
                }
                if (result.gameObject.name == "dogsong")
                {
                    PlayerPrefs.SetString("songChoice", "dog");
                    SceneManager.LoadScene("Gameplay");
                }
                if (result.gameObject.name == "mainmenu")
                {
                    SceneManager.LoadScene("Song Selection");
                }
            }
        }
    }
}
