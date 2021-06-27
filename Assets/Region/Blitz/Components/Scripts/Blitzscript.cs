using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blitzscript : MonoBehaviour {

    private float time = 0;
    private float targetTime = 0;
    private bool flashing = false;
    public bool big = false;

    // Start is called before the first frame update
    void Start() {
        this.gameObject.GetComponent<AudioSource>().volume = Camera.main.GetComponent<CameraScript>().volume;
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }


    // Update is called once per frame
    void Update() {

        targetTime = targetTime == 0 && !flashing ? Random.Range(1, 10) : targetTime;
        targetTime = flashing ? 0.1f : targetTime;
        targetTime = flashing && big ? 0.3f : targetTime;
        time += Time.deltaTime;

        if (time >= targetTime) {
            time = 0;
            targetTime = 0;
            flashing = !flashing;
            this.gameObject.GetComponent<SpriteRenderer>().enabled = flashing;
            if (flashing) {
                this.gameObject.GetComponent<AudioSource>().Play();
            }
        }


    }
}
