using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemScript : MonoBehaviour {

    private Camera camera;
    private Vector3 mousePos;
    private Vector2 mousePosWorld;
    private GameObject player;
    private GameObject inventory;
    private Inventory inventoryScript;
    private List<GameObject> inventoryList;
    private float time = 0f;
    private GameObject text;
    private Pathfinding pathfinding;
    private long walkingId;
    private bool collectingRequest = false;
    public bool collected = false;
    public bool follow = false;
    public Vector3 initialInventoryPosition;
    public Sprite switchSprite;

    public AudioClip select;


	
    void Start() {
        this.gameObject.GetComponent<AudioSource>().volume = Camera.main.GetComponent<CameraScript>().volume;
        camera = Camera.main;
        this.player = GameObject.Find("Player");
        pathfinding = player.GetComponent<Pathfinding>();
        inventory = GameObject.Find("Inventory"); //inventory is set to inventory
        inventoryScript = inventory.GetComponent<Inventory>(); //referenced script is defined
        inventoryList = inventoryScript.list; //inventory list is linked to referenced script
        this.gameObject.transform.Find("Canvas").GetComponent<Canvas>().sortingLayerName = "WorldItemText"; // is put on correct sorting layer
        text = this.gameObject.transform.Find("Canvas").Find("Text").gameObject; //text is defined as text
        text.GetComponent<Text>().text = this.gameObject.name; 
        text.GetComponent<Text>().enabled = false; //text is set as non visible
        this.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(3.5f, 3.5f);
    }

    void Update() {

        mousePos = Input.mousePosition; //Stores the mouse position
        Vector3 mouseWorldPos3D = camera.ScreenToWorldPoint(mousePos);
        mousePosWorld = new Vector2(mouseWorldPos3D.x, mouseWorldPos3D.y);

        RaycastHit2D hit = Physics2D.Raycast(mousePosWorld, Vector2.zero, Mathf.Infinity, 1 << this
        .gameObject.layer);

        if (collected) {
            this.gameObject.transform.Find("Canvas").GetComponent<Canvas>().sortingLayerName = "SelectedInventoryItem"; //item is put on correct sorting layer
            if (this.gameObject.GetComponent<Tooltip>().collectedTooltip != "") {
                this.gameObject.GetComponent<Tooltip>().tooltip = this.gameObject.GetComponent<Tooltip>().collectedTooltip;
            }
            if (switchSprite != null) {
                this.gameObject.GetComponent<SpriteRenderer>().sprite = switchSprite;
                this.gameObject.GetComponent<SpriteRenderer>().size = new Vector2(1f, 1f); 
            }
        }

        text.GetComponent<Text>().enabled = hit.collider != null && hit.collider.gameObject == this.gameObject; //text is visible if a collider is hit and the gameobject it's hitting is this one


        if (Input.GetMouseButtonDown(0) && (!this.inventory.GetComponent<Inventory>().inventoryOpen || this.collected)) {

            if (hit.collider != null) {
                if (collected && hit.collider.gameObject == this.gameObject && inventoryScript.firstItem == null) { //if the item is clicked in the inventory and it is the first item to be clicked on
                    follow = true; //follow is set to true
                    this.gameObject.GetComponent<AudioSource>().clip = select;
                    this.gameObject.GetComponent<AudioSource>().Play();
                }
                if (!collected && hit.collider.gameObject == this.gameObject && hit.collider.gameObject.tag == "Item" && inventoryScript.firstItem == null && player.GetComponent<Movement>().canWalk) { //if this item is clicked on and it is not collected, has the tag item, and there is no item previously selected
                    player.GetComponent<Movement>().walkingPath = pathfinding.findPath(100, 100, player.transform.position, pathfinding.findClosestNode(100, 100, new Vector2(mousePosWorld.x, mousePosWorld.y)));
                    this.walkingId = player.GetComponent<Movement>().walkingId;
                    collectingRequest = true;
                }

                else if (!collected && hit.collider.gameObject == this.gameObject && hit.collider.gameObject.tag == "Item" && inventoryScript.firstItem == null && !player.GetComponent<Movement>().canWalk) {
                    collected = true;
                    if (this.gameObject.GetComponent<AudioSource>().clip != null) {
                        this.gameObject.GetComponent<AudioSource>().Play();
                    }
                    inventoryList.Add(this.gameObject);  //the hit item is put in the inventory list
                    this.gameObject.transform.parent = inventory.transform; //inventory is set to parent the item
                    this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                    this.gameObject.GetComponent<PolygonCollider2D>().enabled = false;
                }

                if (collected && hit.collider.gameObject == this.gameObject && inventoryScript.firstItem != null) {  //if the item is clicked on and one was previously selected

                    inventoryScript.secondItem = this.gameObject; //this item is set to be the second item
                }
            }
        }

        if (collectingRequest && player.GetComponent<Movement>().walkingPath.Count == 0 && this.walkingId == player.GetComponent<Movement>().walkingId) {
            if (this.gameObject.GetComponent<AudioSource>().clip != null) {
                this.gameObject.GetComponent<AudioSource>().Play();
            }
            collectingRequest = false;
            collected = true;
            inventoryList.Add(this.gameObject);  //the hit item is put in the inventory list
            this.gameObject.transform.parent = inventory.transform; //inventory is set to parent the item
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            this.gameObject.GetComponent<PolygonCollider2D>().enabled = false;
        }
		
        hover(hit);
        this.gameObject.GetComponent<PolygonCollider2D>().enabled = !collected && !follow;
        this.gameObject.GetComponent<BoxCollider2D>().enabled = collected && !follow;

        if (follow && Input.GetMouseButtonDown(1)) { //if right button is clicked while follow active
            follow = false; //follow is set to false
        }

        if (follow) { //if follow is true
            this.gameObject.transform.position = mousePosWorld; //position of gameobject set to cursor position
            inventoryScript.firstItem = this.gameObject; //item is set to first item
        }
    }

    void hover(RaycastHit2D hit) {
        double timeLimit = 0.2;
        float floatHeight = 0.3f;
        if (hit.collider != null && hit.collider.gameObject == this.gameObject && collected) {
            if (time < timeLimit) {
                time += Time.deltaTime;
                time = time > timeLimit ? (float)timeLimit : time;
            }
        } else {
            if (time > 0) {
                time -= Time.deltaTime;
                time = time < 0 ? 0 : time;
            }
        }

        double x = Math.PI / 2f / timeLimit * time;
        if (collected) {
            this.gameObject.transform.position = initialInventoryPosition + new Vector3(0f, (float)
            Math.Sin(x) * floatHeight, 0f);
            this.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1f, 1f + (float)
            Math.Sin(x) * floatHeight * 2);
        }
    }
}