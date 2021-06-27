using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatterySlotScript : MonoBehaviour
{
    private Camera camera;
    private Vector3 mousePos;
    private Vector2 mousePosWorld;
    private bool giveBattery = true;

    public GameObject itemPrefab;
    public GameObject inventory;
    public GameObject player;
    public GameObject escapeVan;
    public AudioClip closeClip;

    void Start() {
        this.gameObject.GetComponent<AudioSource>().volume = Camera.main.GetComponent<CameraScript>().volume;
        this.camera = Camera.main;
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
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

                    if (item.name == "Geladene Batterie") {
                        this.GetComponent<AudioSource>().clip = closeClip;
                        this.gameObject.GetComponent<AudioSource>().Play();
                        inventory.GetComponent<Inventory>().list.Remove(item);
                        Destroy(item);
                        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                        this.gameObject.GetComponent<Tooltip>().tooltip = "Die Batterie ist geladen. Ich sollte mich jetzt um die anderen Probleme kümmern.";
                        escapeVan.GetComponent<EscapeVan>().batterie = true;
                    }

                    if (item.name == "Leere Batterie")
                    {
                        player.GetComponent<TextResponses>().display("Ich muss die Batterie erst aufladen.");
                    }
                }

                else if (item == null && giveBattery == true) {
                    this.gameObject.GetComponent<AudioSource>().Play();
                    player.GetComponent<TextResponses>().display("Da war eine Batterie im Slot, aber sie ist leer...");
                    this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    GameObject newItem = Instantiate(itemPrefab, new Vector2(0, 0), Quaternion.identity); //new game object is created from prefab item
                    newItem.name = "Leere Batterie"; //name of new item is set to zucchini
                    newItem.GetComponent<ItemScript>().collected = true; //item is set to collected
                    newItem.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Crafting/CraftedItemSprites/BatterieLeer"); //item sprite is set to correlating image
                    newItem.GetComponent<Tooltip>().tooltip = "Ich brauche einen ordentlichen Schub an Energie um die aufzuladen...";
                    newItem.GetComponent<SpriteRenderer>().size = new Vector2(1f, 1f);
                    inventory.GetComponent<Inventory>().list.Add(newItem); //item is added to inventory list
                    escapeVan.GetComponent<EscapeVan>().batteryCheck = true;
                    giveBattery = false;
                }
            }
        }

        if (escapeVan.GetComponent<EscapeVan>().finished) {
            this.gameObject.GetComponent<Tooltip>().tooltip = "Der Van ist startklar und ich habe Proviant! Jetzt muss ich nur noch einsteigen.";
        }
    }
}
