// Scrolls the song select screen

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scroller : MonoBehaviour
{

    RectTransform rectransform;
    public float scrollPos = 0;


    // Start is called before the first frame update
    void Start()
    {

        rectransform = GetComponent<RectTransform>();
       
    }

    // Update is called once per frame
    void Update()
    {
        if (0f <= (scrollPos + Input.mouseScrollDelta.y) && (scrollPos + Input.mouseScrollDelta.y) <= 26f)
        {
            scrollPos += Input.mouseScrollDelta.y;
            rectransform.localScale = new Vector3(0.3785258f, 0.1684436f, 0);
            rectransform.offsetMin -= new Vector2(0, Input.mouseScrollDelta.y * 100);
            rectransform.offsetMax -= new Vector2(0, Input.mouseScrollDelta.y * 100);
            float x_coord = Mathf.Abs(rectransform.offsetMin[1] / 4) + 545;
            rectransform.offsetMin = new Vector2(x_coord, rectransform.offsetMin[1]);
            rectransform.offsetMax = new Vector2(x_coord, rectransform.offsetMax[1]);
        }
    }
    
}
