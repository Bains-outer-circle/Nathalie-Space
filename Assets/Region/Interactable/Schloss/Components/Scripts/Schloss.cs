using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Schloss : MonoBehaviour {

    public GameObject hutDoor;
    private bool click = false;

    // Start is called before the first frame update
    void Start() {
        this.gameObject.GetComponent<AudioSource>().volume = Camera.main.GetComponent<CameraScript>().volume;
        this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

    // Update is called once per frame
    void Update() {
        if (hutDoor.GetComponent<DoorScript>().open) {
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            if (!click) {
                this.gameObject.GetComponent<AudioSource>().Play();
                click = true;
            }
        }
    }
}
