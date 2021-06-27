using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsScript : MonoBehaviour {

    private float time = 0;

    // Start is called before the first frame update
    void Start() {
        this.gameObject.GetComponent<AudioSource>().volume = Camera.main.GetComponent<CameraScript>().volume;
        this.gameObject.GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void FixedUpdate() {
        this.gameObject.transform.Find("Canvas").transform.position += new Vector3(0, Time.deltaTime / 2.7f, 0);
    }
}