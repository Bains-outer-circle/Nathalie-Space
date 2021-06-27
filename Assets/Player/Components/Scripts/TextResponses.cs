using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Message {

    private string text;
    private float delay;

    public Message(string text, float delay) {
        this.text = text;
        this.delay = delay;
    }

    public string getText() {
        return this.text;
    }
    public float getDelay() {
        return this.delay;
    }
}

public class TextResponses : MonoBehaviour {

    private Camera camera;
    public GameObject inventory;
    private GameObject playerText;
    private GameObject inventoryText;
    public string textToDisplay = "Oh nein! Ich bin hier auf diesem Raumschiff gestrandet!"; //string text to display is defined
    private string textDisplay = ""; //string text display is defined
    public bool displaying = false; //bool displaying is false //private
    private float delay = 0;
    private float time = 0.0f; //time is defined
    private List<Message> queue = new List<Message>();

    // Start is called before the first frame update
    void Start() {
        camera = Camera.main;
        //sorting layer
        playerText = this.gameObject.transform.Find("Canvas").gameObject.transform.Find("Text").gameObject; //text is text
        inventoryText = inventory.transform.Find("Canvas").gameObject.transform.Find("Text").gameObject;
    }

    // Update is called once per frame
    void Update() {

        if (inventory.GetComponent<Inventory>().inventoryOpen) {
            playerText.active = false;
            inventoryText.active = true;
        } else {
            inventoryText.active = false;
            playerText.active = true;
        }

        Vector3 mousePos = Input.mousePosition; //Stores the mouse position
        Vector3 mouseWorldPos3D = camera.ScreenToWorldPoint(mousePos);
        Vector2 mousePosWorld = new Vector2(mouseWorldPos3D.x, mouseWorldPos3D.y);

        string[] layers = new string[] { "Item", "Door", "Visual" };

        if (Input.GetMouseButtonDown(1)) { //RIGHT click
            for (int i = 0; i < layers.Length; i++) {
                RaycastHit2D hit = Physics2D.Raycast(mousePosWorld, Vector2.zero, Mathf.Infinity, 1 << LayerMask.NameToLayer(layers[i]));
                if (hit.collider != null) {
                    if (inventory.GetComponent<Inventory>().inventoryOpen) {
                        if (hit.collider.gameObject.GetComponent<ItemScript>() != null) {
                            if (hit.collider.gameObject.GetComponent<ItemScript>().collected) {
                                GameObject gameObject = hit.collider.gameObject; //game object defined as hit game object
                                if (gameObject.GetComponent<Tooltip>() != null) { //if tooltip not empty
                                    if (gameObject.GetComponent<Tooltip>().forceSkip || !displaying) { //if forceskip is true or displaying is false
                                        Message message = new Message(gameObject.GetComponent<Tooltip>().tooltip, 0f);
                                        this.queue.Add(message);
                                    }
                                }
                            }
                        }
                    } else {
                        GameObject gameObject = hit.collider.gameObject; //game object defined as hit game object
                        if (gameObject.GetComponent<Tooltip>() != null) { //if tooltip not empty
                            if (gameObject.GetComponent<Tooltip>().forceSkip || !displaying) { //if forceskip is true or displaying is false
                                textToDisplay = gameObject.GetComponent<Tooltip>().tooltip; //text to display is set to tooltip
                                StartCoroutine(textDisplayer()); //coroutine text displayer is called
                            }
                        }
                    }
                    break;
                }
            }
        }

        time += Time.deltaTime; //time is time plus time between frames

        if (time > 3.0f) { //if time is over 3
            time = 0.0f; //time is set to 0
            textDisplay = ""; //text display is empty
            playerText.GetComponent<Text>().text = textDisplay; //text is set to text display
            inventoryText.GetComponent<Text>().text = textDisplay;
        }

        if (this.queue.Count > 0 && !displaying) {
            displaying = true;
            Message message = this.queue[0];
            this.queue.Remove(message);

            this.textToDisplay = message.getText();
            this.delay = message.getDelay();
            StartCoroutine(textDisplayer());
        }
    }

    public void display(string text) {
        display(text, 0f);
    }

    public void display(string text, float delay) {
        Message message = new Message(text, delay);
        queue.Add(message);
    }
    
    IEnumerator textDisplayer() { 
        if (textToDisplay != "") { //if text to display is not empty
            yield return new WaitForSeconds(delay); //wait for 0.04 seconds
            delay = 0;
            displaying = true; //displaying is true
            textDisplay = ""; //text display is empty
            for (int i = 0; i < textToDisplay.Length; i++) { //for every character in text to display
                time = 0.0f; //time is set to 0
                textDisplay += textToDisplay[i]; //text display is text display plus i
                playerText.GetComponent<Text>().text = textDisplay; //text is text display
                inventoryText.GetComponent<Text>().text = textDisplay;
                yield return new WaitForSeconds(0.04f); //wait for 0.04 seconds
            }
        }
        textToDisplay = "";
        displaying = false; //displaying is false
    }
}
