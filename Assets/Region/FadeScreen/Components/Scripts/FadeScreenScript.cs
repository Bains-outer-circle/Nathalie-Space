using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeScreenScript : MonoBehaviour {
    private float time = 0;
    public bool fading = false;
    private bool nextScene = false;
    private float firstDelay = 3;
    private bool crashed = false;

    private GameObject currentRegion, linkedRegion, player, text;
    private float fadeSpeed = 1f;

    void Start()
    {
        this.gameObject.GetComponent<AudioSource>().volume = Camera.main.GetComponent<CameraScript>().volume;
    }


    void Update() {
        fade();
    }

    public void startFading(GameObject currentRegion, GameObject linkedRegion, GameObject player, GameObject text, float fadeSpeed) {
        this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
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
            time = Mathf.Min(time, 2 + this.firstDelay);
            if (time >= 1f && !nextScene) {
                nextScene = true;
                time = 1f;
            }
            if (time == 1) {
                currentRegion.transform.parent.gameObject.active = false; //parent of this door is set inactive
                linkedRegion.active = true; //linked region is set to active
                player.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                player.GetComponent<Movement>().canWalk = linkedRegion.transform.Find("Background").GetComponent<Background>().walkable;
                player.GetComponent<Movement>().walkSound = linkedRegion.transform.Find("Background").GetComponent<Background>().sound;
                text.GetComponent<Text>().text = linkedRegion.name; //text is set to linked region name
                if (player.GetComponent<Movement>().walkingPath != null) {
                    player.GetComponent<Movement>().walkingPath.Clear();
                }
                this.gameObject.GetComponent<AudioSource>().clip = this.linkedRegion.transform.Find("Background").GetComponent<Background>().enteringSound;

                if (!crashed) {
                    this.gameObject.GetComponent<AudioSource>().Play();
                    this.crashed = true;
                }

                Transform spawn = linkedRegion.transform.Find("spawn");
            if (spawn != null) {
                    player.GetComponent<SpriteRenderer>().enabled = true;
                    player.transform.position = linkedRegion.transform.Find("spawn").transform.position;
                    player.GetComponent<Movement>().startingScale = player.GetComponent<Movement>().initialStartingScale * linkedRegion.transform.Find("Background").GetComponent<Background>().playerSize;
                    player.GetComponent<Movement>().distanceToText = player.GetComponent<Movement>().initialDistanceToText * linkedRegion.transform.Find("Background").GetComponent<Background>().playerSize;
                } else {
                    player.GetComponent<SpriteRenderer>().enabled = false;
                }
                player.GetComponent<Movement>().initialDistanceToCamera = linkedRegion.transform.Find("Background").GetComponent<Background>().cameraDistance;
                player.GetComponent<Movement>().scalePlayer();
            }

            if (time <= 1f) {
                this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, time);
            }

            if (time > 1f + this.firstDelay) {
                this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f - (time - 1f - this.firstDelay));
            }

            if (time == 2 + this.firstDelay) {
                this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                nextScene = false;
                fading = false;
                time = 0;
                this.firstDelay = 0;
            }
        }
    }
}
