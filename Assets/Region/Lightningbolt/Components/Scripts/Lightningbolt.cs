using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightningbolt : MonoBehaviour {

    private bool started = false;
    private float time = 0;
    private GameObject oldBattery, newBattery;
    public Sprite otherSprite;

    // Start is called before the first frame update
    void Start() {
        oldBattery = this.transform.parent.Find("Battery Old").gameObject;
        newBattery = this.transform.parent.Find("Battery New").gameObject;
    }

    // Update is called once per frame
    void Update() {
        if (started) {
            time += Time.deltaTime;
            if (time >= 0.1f) {
                this.gameObject.GetComponent<SpriteRenderer>().sprite = otherSprite;
                float fadeFactor = Mathf.Min(time - 0.1f, 1f);
                oldBattery.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f - fadeFactor);
                newBattery.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, fadeFactor);
                if (fadeFactor == 1f) {
                    started = false;
                    time = 0;
                    this.gameObject.transform.parent.gameObject.active = false;
                }
            }
        }
    }

    public void startSequence() {
        this.gameObject.active = true;
        started = true;

    }
}