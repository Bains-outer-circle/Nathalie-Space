using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZucchiniScript: MonoBehaviour
{
    private Camera camera;
    private Vector3 mousePos;
    private Vector2 mousePosWorld;



    public GameObject itemPrefab;
    public GameObject inventory;
    public GameObject escapeVan;

    


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

        if (Input.GetMouseButtonDown(0) && !this.inventory.GetComponent<Inventory>().inventoryOpen) {
            if (hit.collider != null && hit.collider.gameObject == this.gameObject) {
                GameObject item = inventory.GetComponent<Inventory>().firstItem;
                if (item != null) {
                    if (item.name == "Axt") {
                        this.gameObject.GetComponent<AudioSource>().Play();
                        GameObject newItem = Instantiate(itemPrefab, new Vector2(0, 0), Quaternion.identity); //new game object is created from prefab item
                        newItem.name = "Zucchini"; //name of new item is set to zucchini
                        newItem.GetComponent<ItemScript>().collected = true; //item is set to collected
                        newItem.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Crafting/CraftedItemSprites/SwitchZucchini"); //item sprite is set to correlating image
                        newItem.GetComponent<Tooltip>().tooltip = "Uff, die ist aber schwer.";
                        newItem.GetComponent<SpriteRenderer>().size = new Vector2(1f, 1f);
                        inventory.GetComponent<Inventory>().list.Add(newItem); //item is added to inventory list
                        escapeVan.GetComponent<EscapeVan>().zucchini = true;
                        this.gameObject.GetComponent<PolygonCollider2D>().enabled = false;
                        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                    }
                }
            }
        }
    }
}
