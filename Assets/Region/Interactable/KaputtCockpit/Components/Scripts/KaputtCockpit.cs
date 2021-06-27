using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KaputtCockpit : MonoBehaviour
{
    private Camera camera;
    private Vector3 mousePos;
    private Vector2 mousePosWorld;



    public GameObject itemPrefab;
    public GameObject inventory;
    public GameObject player;
    public bool stickTaken = false;




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

        if (Input.GetMouseButtonDown(0) && !this.inventory.GetComponent<Inventory>().inventoryOpen){

            if (hit.collider != null && hit.collider.gameObject == this.gameObject && stickTaken == false) {

                this.gameObject.GetComponent<AudioSource>().Play();
                player.GetComponent<TextResponses>().display("Aha! da war noch ein Stick in der Konsole! Laut Beschriftung ist da das Navigationssystem drauf...");
                GameObject newItem = Instantiate(itemPrefab, new Vector2(0, 0), Quaternion.identity); //new game object is created from prefab item
                newItem.name = "Stick"; //name of new item is set to zucchini
                newItem.GetComponent<ItemScript>().collected = true; //item is set to collected
                newItem.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Crafting/CraftedItemSprites/Stick"); //item sprite is set to correlating image
                newItem.GetComponent<Tooltip>().tooltip = "Ein Stick mit Navigationssystem.";
                newItem.GetComponent<SpriteRenderer>().size = new Vector2(1f, 1f);
                inventory.GetComponent<Inventory>().list.Add(newItem); //item is added to inventory list
                stickTaken = true;
                this.gameObject.GetComponent<Tooltip>().tooltip = "Jetzt ist wirklich nur noch M�ll da.";

                    }

            else if (hit.collider != null && hit.collider.gameObject == this.gameObject && stickTaken == true) {
                player.GetComponent<TextResponses>().textToDisplay = "Es gibt nichts anderes n�tzliches.";

            }
        }
    }
}
