using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class mapmaker : MonoBehaviour {

    float time1;

    float time2;

    float timedif;

    string path = "Assets/Scenes/vals2.bit.txt";

    bool hasStarted;

    StreamWriter writer;

	// Use this for initialization
	void Start () {
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {

            if (!hasStarted)
            {
                hasStarted = true;
                time1 = Time.time;

            } else {
                writer = new StreamWriter(path, true);
                time2 = Time.time;
                timedif = time2 - time1;
                string line = timedif.ToString() + "|" + Random.Range(0, 4).ToString();
                writer.WriteLine(line);
                writer.Close();
            }
        }
    }
}
