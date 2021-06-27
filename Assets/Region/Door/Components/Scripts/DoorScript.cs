using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DoorScript : MonoBehaviour {
    private Camera camera;
    private Vector3 mousePos;
    private Vector2 mousePosWorld;
    private GameObject inventory;
    private GameObject text;
    public bool open; //boolean open defined
    private bool discovered = false; //boolean discovered set to false
    private bool fading = false;
    private bool nextScene = false;
    private float time = 0;
    private Pathfinding pathfinding;
    private long walkingId;
    private bool walkingRequested = false;
    private bool requestEntering, requestOpening;
    private GameObject player;

    public string discoveredTooltip;
    public string discoveredShoutout;

    public AudioClip keySound = null;
    public AudioClip openSound = null;
    
    public GameObject linkedRegion; //gameobject linked region defined
    public string keyItemName; //string key item name defined

    // Start is called before the first frame update
    void Start() {
        this.gameObject.GetComponent<AudioSource>().volume = Camera.main.GetComponent<CameraScript>().volume;
        player = GameObject.Find("Player");
        camera = Camera.main;
        inventory = GameObject.Find("Inventory"); //inventory is set to inventory

        open = keyItemName == ""; //if key item name is empty open is true

        this.player = GameObject.Find("Player");
        this.pathfinding = player.GetComponent<Pathfinding>();
        this.gameObject.transform.Find("Canvas").GetComponent<Canvas>().sortingLayerName = "WorldItemText"; //set to correct sorting layer
        text = this.gameObject.transform.Find("Canvas").Find("Text").gameObject; //text is set to text
        text.GetComponent<Text>().enabled = false; //text is disabled
    }

    // Update is called once per frame
    void Update() {

        mousePos = Input.mousePosition; //Stores the mouse position
        Vector3 mouseWorldPos3D = camera.ScreenToWorldPoint(mousePos);
        mousePosWorld = new Vector2(mouseWorldPos3D.x, mouseWorldPos3D.y);

        RaycastHit2D hit = Physics2D.Raycast(mousePosWorld, Vector2.zero, Mathf.Infinity, 1 << this.gameObject.layer);
        text.GetComponent<Text>().enabled = hit.collider != null && hit.collider.gameObject == this.gameObject;

        if (Input.GetMouseButtonDown(0) && !this.inventory.GetComponent<Inventory>().inventoryOpen) {

            if (hit.collider != null) {
                player.GetComponent<Movement>().walkingPath = pathfinding.findPath(100, 100, player.transform.position, pathfinding.findClosestNode(100, 100, new Vector2(mousePosWorld.x, mousePosWorld.y)));
                this.walkingId = player.GetComponent<Movement>().walkingId;
                this.walkingRequested = true;

                if (hit.collider.gameObject == this.gameObject && hit.collider.gameObject.tag == "Door") { //if collider hit this object and the object has the tag door
                    if (open) { //if open true
                        requestEntering = true;
                    } else { //if open false
                        requestOpening = true;
                    }
                }
            }
        }

        if (walkingRequested && player.GetComponent<Movement>().walkingPath != null && player.GetComponent<Movement>().walkingPath.Count == 0 && this.walkingId == player.GetComponent<Movement>().walkingId) {
            if (requestEntering) {
                if (openSound != null) {
                    this.gameObject.GetComponent<AudioSource>().clip = openSound;
                    this.gameObject.GetComponent<AudioSource>().Play();
                } 
                GameObject.Find("FadeScreen").GetComponent<FadeScreenScript>().startFading(this.gameObject, this.linkedRegion, this.player, this.text, 1f);
                if (!discovered) {
                    if (this.gameObject.transform.parent.name == "Spaceship") {
                        player.GetComponent<TextResponses>().display(discoveredShoutout, 5f);
                    } else {
                        player.GetComponent<TextResponses>().display(discoveredShoutout, 2f); //2 sekunden (also so lange wie der fade)
                    }
                }
                discovered = true; //set discovered true
                this.gameObject.GetComponent<Tooltip>().tooltip = discoveredTooltip;
                requestEntering = false;
            } else if (requestOpening) {
                if (inventory.GetComponent<Inventory>().firstItem != null) { //if first item is not empty
                    GameObject item = inventory.GetComponent<Inventory>().firstItem; //item is set to first item
                    if (keyItemName == item.name) { //if key item name is item name
                        if (keySound != null) {
                            this.gameObject.GetComponent<AudioSource>().clip = keySound;
                            this.gameObject.GetComponent<AudioSource>().Play();
                        }
                        inventory.GetComponent<Inventory>().list.Remove(item); //item is removed from inventory
                        Destroy(item); //item is destroyed
                        open = true; //open is true
                    }
                }
                requestOpening = false;
            }
        }
    }
}