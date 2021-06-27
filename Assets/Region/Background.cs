using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour {
    public float cameraDistance;
    public bool walkable = true;
    public float playerSize;
    public AudioClip sound;
    public AudioClip enteringSound;

    public AudioClip ambience;

    // Start is called before the first frame update
    void Start() {
        this.gameObject.GetComponent<AudioSource>().volume = Camera.main.GetComponent<CameraScript>().volume;
        this.gameObject.GetComponent<AudioSource>().clip = ambience;
        this.gameObject.GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update() {
        
    }
}
