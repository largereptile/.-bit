using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class background : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject camera = GameObject.Find("Main Camera");
        var videoPlayer = camera.AddComponent<UnityEngine.Video.VideoPlayer>();
        videoPlayer.playOnAwake = false;
        videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.CameraNearPlane;
        videoPlayer.targetCameraAlpha = 0.5F;
        videoPlayer.url = "Assets/Scenes/matrix.mp4";
        videoPlayer.isLooping = true;
        videoPlayer.Play();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
