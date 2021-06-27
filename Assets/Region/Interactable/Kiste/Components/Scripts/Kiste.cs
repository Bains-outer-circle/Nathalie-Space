using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kiste : MonoBehaviour
{
    private Camera camera;
    private Vector3 mousePos;
    private Vector2 mousePosWorld;

    public GameObject itemPrefab;
    public GameObject inventory;
    public GameObject player;
    public bool keycardTaken = false;

    void Start() {
        this.gameObject.GetComponent<AudioSource>().volume = Camera.main.GetComponent<CameraScript>().volume;
        this.camera = Camera.main;
    }

    void Update() {
        mousePos = Input.mousePosition; //Stores the mouse position
        Vector3 mouseWorldPos3D = camera.ScreenToWorldPoint(mousePos);
        mousePosWorld = new Vector2(mouseWorldPos3D.x, mouseWorldPos3D.y);

        RaycastHit2D hit = Physics2D.Raycast(mousePosWorld, Vector2.zero, Mathf.Infinity, 1 << this
        .gameObject.layer);

        if (Input.GetMouseButtonDown(0) && !this.inventory.GetComponent<Inventory>().inventoryOpen) { //leftclick 

            if (hit.collider != null && hit.collider.gameObject == this.gameObject && keycardTaken == false) { //object hit, keycard not taken

                this.gameObject.GetComponent<AudioSource>().Play();
                player.GetComponent<TextResponses>().display("Ah, da ist sie! Ich habe sie in mein Inventar gelegt."); //shoutout
                GameObject newItem = Instantiate(itemPrefab, new Vector2(0, 0), Quaternion.identity); //new game object is created from prefab item
                newItem.name = "Keycard"; //name of new item is set to keycard
                newItem.GetComponent<ItemScript>().collected = true; //item is set to collected
                newItem.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Crafting/CraftedItemSprites/Keycard"); //item sprite is set to correlating image
                newItem.GetComponent<Tooltip>().tooltip = "Eine Keycard zum Öffnen des Escape Pods."; //tooltip assigned
                newItem.GetComponent<SpriteRenderer>().size = new Vector2(1f, 1f); //sprite size
                inventory.GetComponent<Inventory>().list.Add(newItem); //item is added to inventory list
                keycardTaken = true; //keycard taken
                this.gameObject.GetComponent<Tooltip>().tooltip = "Ich hab die Keycard schon, ich muss weg hier!"; //tooltip this object

            }

            else if (hit.collider != null && hit.collider.gameObject == this.gameObject && keycardTaken == true) //keycard taken
            {
                player.GetComponent<TextResponses>().textToDisplay = "Ich hab die Keycard schon, ich muss weg hier!"; //shoutout

            }
        }
    }
}
