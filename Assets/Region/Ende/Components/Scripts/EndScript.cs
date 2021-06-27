using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScript : MonoBehaviour {

    private float time = 0;
    public GameObject endScene;
    public Sprite escapeScreen, endScreen, creditsScreen;
    private GameObject credits;
    private GameObject player;

    // Start is called before the first frame update
    void Start() {
        this.gameObject.GetComponent<AudioSource>().volume = Camera.main.GetComponent<CameraScript>().volume;
        this.gameObject.GetComponent<AudioSource>().Play();
        this.credits = this.gameObject.transform.Find("Credits").gameObject;
        GameObject.Find("Titlescreen").active = false;
        GameObject.Find("FadeScreen").active = false;
        this.player = GameObject.Find("Player");
        this.gameObject.transform.Find("TheEnd").GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
    }

    // Update is called once per frame
    void Update() {
        this.time += Time.deltaTime;

        if (time <= 1f) {
            this.player.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, Mathf.Max(1f - time, 0));
            GameObject.Find("Cursor").GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, Mathf.Max(1f - time, 0));
            foreach (Transform go in endScene.transform) {
                if (go.GetComponent<SpriteRenderer>() != null) {
                    go.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, Mathf.Max(1f - time, 0));
                }
            }
        }

        if (time >= 1f) {
            this.player.active = false;
            this.endScene.active = false;
        }

        if (time >= 4.5f) {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = escapeScreen;
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, Mathf.Min(time - 4.5f, 1f));
        }

        if (time >= 8f) {
            this.gameObject.transform.Find("TheEnd").GetComponent<SpriteRenderer>().sprite = endScreen;
            this.gameObject.transform.Find("TheEnd").GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, Mathf.Min(time - 8f, 1f));
        }

        if (time >= 13f) {
            this.gameObject.transform.Find("TheEnd").GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, Mathf.Max(14f - time, 0f));
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, Mathf.Max(14f - time, 0f));
            credits.GetComponent<SpriteRenderer>().enabled = true;
            credits.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, Mathf.Min(time - 13f, 1));
            credits.active = true;
        }

        if (time >= 14f) {
            credits.GetComponent<CreditsScript>().enabled = true;
        }
    }
}
