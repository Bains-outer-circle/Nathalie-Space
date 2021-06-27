using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plakette : MonoBehaviour
{
    private Camera camera;
    private Vector3 mousePos;
    private Vector2 mousePosWorld;
    private bool bigboy = false;
    public Vector2 normalScale;
    public Vector2 normalPlace;

    private GameObject inventory;
    public bool first = true;

    public GameObject player;
    public GameObject skelette;
    public GameObject skelett;




    void Start() {
        this.camera = Camera.main;
        this.gameObject.transform.localScale = normalScale;
        this.gameObject.transform.position = normalPlace;
        this.inventory = GameObject.Find("Inventory");
    }

    void Update() {
        mousePos = Input.mousePosition; //Stores the mouse position
        Vector3 mouseWorldPos3D = camera.ScreenToWorldPoint(mousePos);
        mousePosWorld = new Vector2(mouseWorldPos3D.x, mouseWorldPos3D.y);
        RaycastHit2D hit = Physics2D.Raycast(mousePosWorld, Vector2.zero, Mathf.Infinity, 1 << this.gameObject.layer);

        if (Input.GetMouseButtonDown(0) && !this.inventory.GetComponent<Inventory>().inventoryOpen)  {
            if (hit.collider != null && hit.collider.gameObject == this.gameObject && bigboy == false) {
                bigboy = true;
                this.gameObject.transform.localScale = new Vector2 (1f, 1f);
                this.gameObject.transform.position = new Vector2 (0f, 0f);
                skelette.GetComponent<Tooltip>().tooltip = "Das muss der Sturm gewesen sein. Ich muss schleunigst hier weg!";
                skelett.GetComponent<Tooltip>().tooltip = "Der konnte wohl kein Englisch...";
                this.gameObject.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("SelectedInventoryItem");
				
                if (first)  {
                    player.GetComponent<TextResponses>().display("Der Sturm ist giftig?!Ist ja nicht so als ob ich sonst schon genug Probleme hätte!");
                    first = false;
                }
            }

            else if (hit.collider != null && hit.collider.gameObject == this.gameObject && bigboy == true) {
                bigboy = false;
                this.gameObject.transform.localScale = normalScale;
                this.gameObject.transform.position = normalPlace;
                this.gameObject.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("Item");

            }
        }
    }
}
