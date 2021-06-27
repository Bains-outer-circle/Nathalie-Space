using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baumstumpf : MonoBehaviour {
    private Camera camera;
    private Vector3 mousePos;
    private Vector2 mousePosWorld;
    private bool giveBattery = true;


    public GameObject itemPrefab;
    public GameObject inventory;
    public GameObject player;
    public GameObject lightning;




    void Start() {
        this.camera = Camera.main;
    }

    void Update() {
        mousePos = Input.mousePosition; //Stores the mouse position
        Vector3 mouseWorldPos3D = camera.ScreenToWorldPoint(mousePos);
        mousePosWorld = new Vector2(mouseWorldPos3D.x, mouseWorldPos3D.y);

        RaycastHit2D hit = Physics2D.Raycast(mousePosWorld, Vector2.zero, Mathf.Infinity, 1 << this
        .gameObject.layer);

        if (Input.GetMouseButtonDown(0) && !this.inventory.GetComponent<Inventory>().inventoryOpen) {
            if (hit.collider != null && hit.collider.gameObject == this.gameObject) {

                GameObject item = inventory.GetComponent<Inventory>().firstItem;
                if (item != null) {

                    if (item.name == "Ast mit Batterie") {
                        inventory.GetComponent<Inventory>().list.Remove(item);
                        Destroy(item);

                        lightning.active = true;
                        lightning.transform.Find("Screen").gameObject.GetComponent<Lightningbolt>().startSequence();

                        GameObject newItem = Instantiate(itemPrefab, new Vector2(0, 0), Quaternion.identity); //new game object is created from prefab item
                        newItem.name = "Geladene Batterie"; //name of new item is set to zucchini
                        newItem.GetComponent<ItemScript>().collected = true; //item is set to collected
                        newItem.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Crafting/CraftedItemSprites/BatterieVoll"); //item sprite is set to correlating image
                        newItem.GetComponent<Tooltip>().tooltip = "Die sollte allemal genug geladen sein um hier weg zu kommen.";
                        newItem.GetComponent<SpriteRenderer>().size = new Vector2(1f, 1f);
                        inventory.GetComponent<Inventory>().list.Add(newItem); //item is added to inventory list
                    }

                    if (item.name == "Leere Batterie") {
                        player.GetComponent<TextResponses>().display("Stimmt! Der Blitz hat sicher genug wumms um die Batterie zu laden! Aber ich traue mich da nicht so nah ran...");
                    }

                }
            }
        }
    }
}
