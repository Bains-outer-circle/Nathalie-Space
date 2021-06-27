using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScript : MonoBehaviour {

    public Sprite image0, image1, image2;
    public float screenTime = 2f;
    private float time = 0;
    public GameObject player;
    public GameObject inventory;
    public GameObject nextScene;

    private GameObject titleScreen;

    // Start is called before the first frame update
    void Start() {
        titleScreen = GameObject.Find("Titlescreen");
        titleScreen.active = false;
    }

    // Update is called once per frame
    void Update() {
        time += Time.deltaTime;
        if (time >= screenTime * 3 + 2f) {
            GameObject.Find("FadeScreen").active = true;
            titleScreen.active = true;
            this.gameObject.active = false;
        } else if (time >= screenTime * 3 + 1f) {
            this.inventory.active = true;
            this.nextScene.active = true;
            this.player.active = true;
            foreach (Transform go in nextScene.transform) {
                if (go.GetComponent<SpriteRenderer>() != null) {
                    go.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, Mathf.Min(time - (screenTime * 3 + 1), 1f));
                }
            }
            this.player.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, Mathf.Min(time - (screenTime * 3 + 1), 1f));
        } else if (time >= screenTime * 3) {
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, Mathf.Max(1f - (time - screenTime * 3), 0));
        } else if (time >= screenTime * 2) {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = image2;
        } else if (time >= screenTime) {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = image1;
        } else if (time >= 0) {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = image0;
        }
    }
}