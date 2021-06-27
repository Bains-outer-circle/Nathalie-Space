using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour {

    private float time = 0;
    public bool fading = false;
    private bool nextScene = false;

    private GameObject currentRegion, linkedRegion, player, text;
    private float fadeSpeed = 1f;

    void Update() {
        fade();
    }

    public void startFading(GameObject currentRegion, GameObject linkedRegion, GameObject player, GameObject text, float fadeSpeed) {
        this.currentRegion = currentRegion;
        this.linkedRegion = linkedRegion;
        this.player = player;
        this.text = text;
        this.fadeSpeed = fadeSpeed;
        this.fading = true;
    }

    void fade() {
        if (this.fading) {
            time += Time.deltaTime * fadeSpeed;
            time = Mathf.Min(time, 2);
            if (time >= 1f && !nextScene) {
                nextScene = true;
                time = 1f;
            }
            if (time == 1) {
                currentRegion.transform.parent.gameObject.active = false; //parent of this door is set inactive
                linkedRegion.active = true; //linked region is set to active
                text.GetComponent<Text>().text = linkedRegion.name; //text is set to linked region name
                if (player.GetComponent<Movement>().walkingPath != null) {
                    player.GetComponent<Movement>().walkingPath.Clear();
                }
                Transform spawn = linkedRegion.transform.Find("spawn");
            if (spawn != null) {
                    player.GetComponent<SpriteRenderer>().enabled = true;
                    player.transform.position = linkedRegion.transform.Find("spawn").transform.position;
                    player.GetComponent<Movement>().startingScale = player
                    .GetComponent<Movement>().initialStartingScale * linkedRegion
                    .transform.Find("Background").GetComponent<Background>().playerSize;
                } else {
                    player.GetComponent<SpriteRenderer>().enabled = false;
                }
                player.GetComponent<Movement>().initialDistanceToCamera = linkedRegion.transform.Find("Background").GetComponent<Background>().cameraDistance;
                player.GetComponent<Movement>().scalePlayer();

                foreach (Transform child in linkedRegion.transform) {
                    if (child.gameObject.GetComponent<SpriteRenderer>() != null) {
                        child.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
                    }
                }
            }

            if (time <= 1f) {
                GameObject.Find("Player").GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f - time);
                foreach (Transform child in currentRegion.transform.parent.transform) {
                    if (child.gameObject.GetComponent<SpriteRenderer>() != null) {
                        child.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f - time);
                    }
                }
            }

            if (time > 1f) {
                GameObject.Find("Player").GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, time - 1f);
                foreach (Transform child in linkedRegion.transform) {
                    if (child.gameObject.GetComponent<SpriteRenderer>() != null) {
                        child.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, time - 1f);
                    }
                }
            }

            if (time == 2) {
                GameObject.Find("Player").GetComponent<SpriteRenderer>().color = new Color(1f, 1f,
                1f, 1f);
                foreach (Transform child in linkedRegion.transform) {
                    if (child.gameObject.GetComponent<SpriteRenderer>() != null) {
                        child.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                    }
                }
                nextScene = false;
                fading = false;
                time = 0;
            }
        }
    }
}
