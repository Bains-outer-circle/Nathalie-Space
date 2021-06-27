using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterBox : MonoBehaviour {
    private Camera camera;
    private Vector3 mousePos;
    private Vector2 mousePosWorld;
    private bool leaking = false;

    public GameObject inventory;
    public Sprite filledBottleSprite;
    public Sprite leakingSprite;
    public GameObject escapeVan;
    public AudioClip fillingBottleSound;

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
                if (item.name == "Leere Flasche" && leaking) {
                    this.gameObject.GetComponent<AudioSource>().clip = fillingBottleSound;
                    this.gameObject.GetComponent<AudioSource>().Play();
                    item.name = "Gefüllte Flasche";
                    item.transform.Find("Canvas").Find("Text").gameObject.GetComponent<Text>().text = item.name;
                    item.GetComponent<SpriteRenderer>().sprite = filledBottleSprite;
                    item.GetComponent<Tooltip>().tooltip = "Eine gefüllte Wasserflasche.";
                    this.gameObject.GetComponent<Tooltip>().tooltip = "Mehr Wasser kann ich nicht mitnehmen.";
                    escapeVan.GetComponent<EscapeVan>().water = true;
                }
                if (item.name == "Axt" && !leaking) {
                    this.gameObject.GetComponent<AudioSource>().Play();
                    leaking = true;
                    this.gameObject.GetComponent<SpriteRenderer>().sprite = leakingSprite;
                    this.gameObject.GetComponent<Tooltip>().tooltip = "Jetzt brauche ich ein Behältnis.";
                }
            }
        }
    }
}