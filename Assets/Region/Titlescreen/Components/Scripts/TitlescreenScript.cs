using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlescreenScript : MonoBehaviour {

    private float time = 0;
    public GameObject menu;
    public float screenTime = 3f;
    private bool booting = true;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if(booting) {
            time += Time.deltaTime;
            if (time >= screenTime) {
                this.menu.active = true;
                this.booting = false;
            }
        }
    }
}
