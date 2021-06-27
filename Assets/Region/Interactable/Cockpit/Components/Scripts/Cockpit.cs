using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cockpit : MonoBehaviour {
    private Camera camera;
    private Vector3 mousePos;
    private Vector2 mousePosWorld;
    private bool open = false;
    public GameObject inventory;
    public GameObject player;
    public GameObject escapeVan;
    public AudioClip stickSound;

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
                if (item != null && open) {
                    if (item.name == "Stick") {
                        this.gameObject.GetComponent<AudioSource>().clip = stickSound;
                        this.gameObject.GetComponent<AudioSource>().Play();
                        inventory.GetComponent<Inventory>().list.Remove(item);
                        Destroy(item);
                        player.GetComponent<TextResponses>().display("So, das Navigationssystem läuft!");
                        escapeVan.GetComponent<EscapeVan>().stick = true;
                        
                    }
                }
                else if (item == null && !open) {
                    this.gameObject.GetComponent<AudioSource>().Play();
                    player.GetComponent<TextResponses>().display("Alles ist intakt, aber das Navigationssystem fehlt. Ohne das komm ich hier nie weg."); 
                    this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    open = true;
                    escapeVan.GetComponent<EscapeVan>().doorCheck = true;
                    this.gameObject.GetComponent<Tooltip>().tooltip = "Es fehlt ein Navigationssystem.";
                }

                if (this.escapeVan.GetComponent<EscapeVan>().finished) {
                    this.escapeVan.GetComponent<EscapeVan>().end.active = true;
                }
            }

            if (escapeVan.GetComponent<EscapeVan>().finished) {
                //hier ist das ende....
            }
        }

        if (escapeVan.GetComponent<EscapeVan>().stick) {
            this.gameObject.GetComponent<Tooltip>().tooltip = "Das Navigationssystem läuft, jetzt sollte ich mich um die anderen Probleme kümmern.";
        }

        if (escapeVan.GetComponent<EscapeVan>().finished) {
            this.gameObject.GetComponent<Tooltip>().tooltip = "Der Van kann wieder fliegen und ich habe genug Proviant, es kann losgehen!";
        }
    }

   
}
