using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PictureScript : MonoBehaviour {

    public Sprite image1, image2, image3;
    public float changePoint1, changePoint2;
    private bool change1, change2;
    private Sprite nextSprite;
    private float time;
    private bool fading = false;
    private GameObject fadeObject;

    // Start is called before the first frame update
    void Start() {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = image1;
        this.fadeObject = this.gameObject.transform.Find("Fade").gameObject;
    }

    // Update is called once per frame
    void FixedUpdate() {
        float position = this.gameObject.transform.position.y;
        this.gameObject.transform.position += new Vector3(0f, Time.deltaTime / 2.7f, 0f);
        if (position >= this.changePoint1 && !this.change1) {
            this.change1 = true;
            nextSprite = image2;
            fading = true;
        }
        if (position >= this.changePoint2 && !this.change2) {
            this.change2 = true;
            nextSprite = image3;
            fading = true;
        }
        fade();
    }

    void fade() {
        if (fading) {
            time += Time.deltaTime;
            if (time >= 0f && time < 1f) {
                this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f - time);
                this.fadeObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, time);
                this.fadeObject.GetComponent<SpriteRenderer>().sprite = this.nextSprite;
            }
            if (time >= 1f) {
                fading = false;
                this.gameObject.GetComponent<SpriteRenderer>().sprite = this.nextSprite;
                this.fadeObject.GetComponent<SpriteRenderer>().sprite = null;
                this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                this.fadeObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                time = 0;
            }
        }
    }
}
