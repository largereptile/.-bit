// Script to rotate circles on the main menu and score screen

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleRotator : MonoBehaviour
{

    public float z;

    public int speed;

    public RectTransform panel;
    public GameObject circle;

    // Start is called before the first frame update
    void Start()
    {
        panel = gameObject.GetComponent<RectTransform>();

    }

    // Update is called once per frame
    void Update()
    {
        z += Time.deltaTime * speed;
        panel.rotation = Quaternion.Euler(0, 0, z);

    }
}
