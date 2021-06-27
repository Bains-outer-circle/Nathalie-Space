using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinker : MonoBehaviour {

    private float time = 0f;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {

        this.time += Time.deltaTime * 1.5f;
        float v = 1 - (Mathf.Sin(this.time) + 1) / 4;

        GameObject.Find("Player").GetComponent<SpriteRenderer>().color = new Color(1f, v, v);
        foreach (Transform go in this.transform) {
            if (go.GetComponent<SpriteRenderer>() != null) {
                go.GetComponent<SpriteRenderer>().color = new Color(1f, v, v);
            }
        }
    }
}
